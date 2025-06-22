using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ApplicationManagers;
using Discord;

namespace UI
{
    class DiscordLoginPopup : BasePopup
    {
        protected override string Title => "Discord Login";
        protected override float Width => 400f;
        protected override float Height => 300f;

        private Button _loginButton;
        private Button _logoutButton;
        private Text _statusLabel;
        private Text _userInfoLabel;
        private RawImage _avatarImage;
        private GameObject _loginPanel;
        private GameObject _loggedInPanel;
        
        public bool IsMandatory { get; set; } = false;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupUI();
            SubscribeToDiscordEvents();
            UpdateUI();
        }

        public override void Hide()
        {
            if (IsMandatory && (DiscordManager.discord == null || !DiscordManager.IsAuthenticated))
            {
                return;
            }
            
            base.Hide();
        }

        public void ShowMandatory()
        {
            IsMandatory = true;
            Show();
            
            if (_statusLabel != null && !DiscordManager.IsAuthenticated)
            {
                _statusLabel.text = "Discord authentication is required to play AOTTG2. Please log in to continue.";
                _statusLabel.color = Color.red;
            }
        }

        public void ShowOptional()
        {
            IsMandatory = false;
            Show();
            
            if (_statusLabel != null && !DiscordManager.IsAuthenticated)
            {
                _statusLabel.text = "Click 'Login with Discord' to authenticate with your Discord account.";
                _statusLabel.color = Color.white;
            }
        }

        private void SetupUI()
        {
            _loginPanel = new GameObject("LoginPanel");
            _loginPanel.transform.SetParent(SinglePanel);
            _loginPanel.AddComponent<RectTransform>();
            _loginPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 200f);
            ElementFactory.SetAnchor(_loginPanel, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, Vector2.zero);

            _statusLabel = ElementFactory.CreateDefaultLabel(_loginPanel.transform, ElementStyle.Default, 
                "Click 'Login with Discord' to authenticate with your Discord account.", alignment: TextAnchor.MiddleCenter).GetComponent<Text>();
            _statusLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(320f, 80f);
            ElementFactory.SetAnchor(_statusLabel.gameObject, TextAnchor.UpperCenter, TextAnchor.UpperCenter, new Vector2(0f, -20f));

            _loginButton = ElementFactory.CreateDefaultButton(_loginPanel.transform, ElementStyle.Default, 
                "Login with Discord").GetComponent<Button>();
            _loginButton.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 40f);
            ElementFactory.SetAnchor(_loginButton.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, new Vector2(0f, -20f));
            _loginButton.onClick.AddListener(OnLoginButtonClick);

            _loggedInPanel = new GameObject("LoggedInPanel");
            _loggedInPanel.transform.SetParent(SinglePanel);
            _loggedInPanel.AddComponent<RectTransform>();
            _loggedInPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(350f, 200f);
            ElementFactory.SetAnchor(_loggedInPanel, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, Vector2.zero);

            _avatarImage = ElementFactory.CreateRawImage(_loggedInPanel.transform, ElementStyle.Default, "", 64f, 64f).GetComponent<RawImage>();
            ElementFactory.SetAnchor(_avatarImage.gameObject, TextAnchor.UpperCenter, TextAnchor.UpperCenter, new Vector2(0f, -20f));

            _userInfoLabel = ElementFactory.CreateDefaultLabel(_loggedInPanel.transform, ElementStyle.Default, 
                "", alignment: TextAnchor.MiddleCenter).GetComponent<Text>();
            _userInfoLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(320f, 40f);
            ElementFactory.SetAnchor(_userInfoLabel.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, new Vector2(0f, -20f));

            _logoutButton = ElementFactory.CreateDefaultButton(_loggedInPanel.transform, ElementStyle.Default, 
                "Logout").GetComponent<Button>();
            _logoutButton.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 40f);
            ElementFactory.SetAnchor(_logoutButton.gameObject, TextAnchor.LowerCenter, TextAnchor.LowerCenter, new Vector2(0f, 20f));
            _logoutButton.onClick.AddListener(OnLogoutButtonClick);
        }

        private void SubscribeToDiscordEvents()
        {
            DiscordManager.OnUserAuthenticated += OnUserAuthenticated;
            DiscordManager.OnAuthenticationFailed += OnAuthenticationFailed;
            DiscordManager.OnUserLoggedOut += OnUserLoggedOut;
        }

        private void UnsubscribeFromDiscordEvents()
        {
            DiscordManager.OnUserAuthenticated -= OnUserAuthenticated;
            DiscordManager.OnAuthenticationFailed -= OnAuthenticationFailed;
            DiscordManager.OnUserLoggedOut -= OnUserLoggedOut;
        }

        private void OnLoginButtonClick()
        {
            try
            {
                DiscordManager.Init();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to initialize DiscordManager: {ex.Message}");
            }
            
            if (DiscordManager.discord == null)
            {
                _statusLabel.text = "Failed to initialize Discord SDK. Is Discord running?";
                _statusLabel.color = Color.red;
                return;
            }
            
            // Update UI to show we're starting authentication
            _statusLabel.text = "Authenticating with Discord...";
            _statusLabel.color = Color.yellow;
            _loginButton.interactable = false;
            
            try
            {
                DiscordManager.StartOAuth2Authentication();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Exception during authentication: {ex.Message}");
                _statusLabel.text = $"Authentication failed: {ex.Message}";
                _statusLabel.color = Color.red;
                _loginButton.interactable = true;
            }
        }

        private void OnLogoutButtonClick()
        {
            if (IsMandatory)
            {
                Debug.Log("Cannot logout - Discord authentication is mandatory");
                return;
            }
            
            DiscordManager.Logout();
            UpdateUI();
        }

        private void OnUserAuthenticated(User user)
        {
            Debug.Log($"User authenticated: {user.Username}#{user.Discriminator}");
            
            if (IsMandatory)
            {
                IsMandatory = false;
            }
            
            UpdateUI();
            StartCoroutine(LoadUserAvatar(user));
        }

        private void OnAuthenticationFailed(string error)
        {
            _statusLabel.text = $"Authentication failed: {error}";
            _statusLabel.color = Color.red;
            _loginButton.interactable = true;
            UpdateUI();
        }

        private void OnUserLoggedOut()
        {
            Debug.Log("User logged out");
            
            if (IsMandatory)
            {
                IsMandatory = true;
            }
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            bool isAuthenticated = DiscordManager.discord != null && DiscordManager.IsAuthenticated;
            
            _loginPanel.SetActive(!isAuthenticated);
            _loggedInPanel.SetActive(isAuthenticated);

            if (isAuthenticated)
            {
                _userInfoLabel.text = $"Logged in as:\n{DiscordManager.GetUserDisplayName()}";
                
                _logoutButton.gameObject.SetActive(!IsMandatory);
            }
            else
            {
                if (IsMandatory)
                {
                    _statusLabel.text = "Discord authentication is required to play AOTTG2. Please log in to continue.";
                    _statusLabel.color = Color.red;
                }
                else
                {
                    _statusLabel.text = "Click 'Login with Discord' to authenticate with your Discord account.";
                    _statusLabel.color = Color.white;
                }
                _loginButton.interactable = true;
            }
        }

        private IEnumerator LoadUserAvatar(User user)
        {
            string avatarUrl = DiscordManager.GetUserAvatarUrl();
            if (!string.IsNullOrEmpty(avatarUrl))
            {
                using (WWW www = new WWW(avatarUrl))
                {
                    yield return www;
                    
                    if (string.IsNullOrEmpty(www.error) && www.texture != null)
                    {
                        _avatarImage.texture = www.texture;
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to load avatar: {www.error}");
                    }
                }
            }
        }

        protected void OnDestroy()
        {
            UnsubscribeFromDiscordEvents();
        }
    }
} 