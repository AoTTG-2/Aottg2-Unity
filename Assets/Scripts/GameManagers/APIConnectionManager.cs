using UnityEngine;
using System.Collections;
using Photon.Pun;
using Utility;

namespace ApplicationManagers
{
    public class APIConnectionManager : MonoBehaviour
    {
        private static APIConnectionManager _instance;
        public static APIConnectionManager Instance => _instance;

        private AuthManager authManager;
        private APIManager apiManager;

        public static void Init()
        {
            if (_instance == null)
            {
                _instance = SingletonFactory.CreateSingleton(_instance);
                _instance.authManager = _instance.gameObject.AddComponent<AuthManager>();
                _instance.apiManager = _instance.gameObject.AddComponent<APIManager>();
            }
        }

        public void StartDiscordAuth()
        {
            StartCoroutine(DiscordAuthCoroutine());
        }

        private IEnumerator DiscordAuthCoroutine()
        {
            authManager.StartDiscordAuth();
            float timeout = 60f; // 60 seconds timeout
            float elapsed = 0f;

            while (!authManager.IsAuthenticated && elapsed < timeout)
            {
                yield return new WaitForSeconds(0.5f);
                elapsed += 0.5f;
            }

            if (elapsed >= timeout)
            {
                Debug.LogError("Authentication timed out");
                yield break;
            }

            string token = authManager.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                Debug.Log("Token successfully retrieved: " + token);
                PlayerPrefs.SetString("AuthToken", token);
                PlayerPrefs.Save();
                apiManager.SetAuthToken(token);

                yield return StartCoroutine(apiManager.GetUserInfo(OnUserInfoReceived));
            }
            else
            {
                Debug.LogError("Failed to receive authentication token");
            }
        }

        private void OnUserInfoReceived(APIManager.UserInfo userInfo)
        {
            if (userInfo != null)
            {
                Debug.Log($"Welcome, {userInfo.displayName}!");
                // Handle staff roles, apply badges, etc.
            }
            else
            {
                Debug.Log("Failed to fetch user info");
            }
        }
    }
}
