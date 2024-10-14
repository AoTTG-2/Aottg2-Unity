using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class APIManager : MonoBehaviour
{
    private const string API_URL = "https://api.gisketch.com";
    private string authToken;

    public void SetAuthToken(string token)
    {
        authToken = token;
    }

    public IEnumerator GetUserInfo(System.Action<UserInfo> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"{API_URL}/users/me"))
        {
            www.SetRequestHeader("Authorization", $"Bearer {authToken}");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                UserInfo userInfo = JsonUtility.FromJson<UserInfo>(www.downloadHandler.text);
                callback(userInfo);
            }
            else
            {
                Debug.LogError($"Error: {www.error}");
                callback(null);
            }
        }
    }

    [System.Serializable]
    public class UserInfo
    {
        public int id;
        public bool isAdmin;
        public string username;
        public string displayName;
        public string discordId;
        public string[] staffRoles;
        public bool isVerified;
    }
}