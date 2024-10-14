using UnityEngine;
using System;
using Utility;

namespace GameManagers
{
    public class UserService : MonoBehaviour
    {
        private static UserService _instance;
        public static UserService Instance => _instance;

        public event Action<UserInfo> OnUserInfoUpdated;

        private UserInfo _currentUser;
        public UserInfo CurrentUser => _currentUser;

        public bool IsLoggedIn => _currentUser != null;
        public bool IsAdmin => _currentUser?.isAdmin ?? false;

        public static void Init()
        {
            if (_instance == null)
            {
                _instance = SingletonFactory.CreateSingleton(_instance);
            }
        }

        public void SetUserInfo(UserInfo userInfo)
        {
            _currentUser = userInfo;
            OnUserInfoUpdated?.Invoke(_currentUser);
        }

        public void Logout()
        {
            _currentUser = null;
            OnUserInfoUpdated?.Invoke(null);
            PlayerPrefs.DeleteKey("AuthToken");
            PlayerPrefs.Save();
        }
    }

    [Serializable]
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