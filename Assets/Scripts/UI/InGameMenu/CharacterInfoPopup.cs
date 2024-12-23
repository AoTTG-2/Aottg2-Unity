using ApplicationManagers;
using Characters;
using GameManagers;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class CharacterInfoPopup : BasePopup
    {
        protected override float AnimationTime => 0.25f;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        protected Text _nameLabel;
        protected Image _nameBackground;
        protected GameObject _healthbar;
        protected Image _healthbarFill;
        protected Text _healthbarLabel;
        protected UnityEngine.UI.Outline _textOutline;
        public BaseCharacter Character;
        public Vector3 Offset;
        public float Range;
        
        private Color _settingsNameColor = new Color(255, 255, 255);
        private Color _settingsBackgroundColor = new Color(0, 0, 0, 100);
        private NameStyleType _settingsStyleType = NameStyleType.Off;
        private bool _settingsApplyToMe = false;
        private bool _teamsEnabled = false;
        private string _name = string.Empty;

        public override void Setup(BasePanel parent = null)
        {
            _nameBackground = transform.Find("Name").GetComponent<Image>();
            _nameLabel = transform.Find("Name/NameLabel").GetComponent<Text>();
            _healthbar = transform.Find("Healthbar").gameObject;
            _healthbarFill = _healthbar.transform.Find("Fill").GetComponent<Image>();
            _healthbarLabel = _healthbar.transform.Find("Label").GetComponent<Text>();
            _textOutline = _nameLabel.GetComponent<UnityEngine.UI.Outline>();
            SettingsManager.OnSettingsChanged += SettingsManager_OnSettingsChanged;
        }

        public void OnDestroy()
        {
            SettingsManager.OnSettingsChanged -= SettingsManager_OnSettingsChanged;
            if (Character != null)
                Character.OnPlayerPropertiesChanged -= Character_OnPlayerPropertiesChanged;
        }

        /// <summary>
        /// Since CharacterInfoPopup objects are not directly tied to their respective characters, we need to use events to check when the state of said popup should change.
        /// </summary>
        /// <param name="changedProps"></param>
        private void Character_OnPlayerPropertiesChanged(ExitGames.Client.Photon.Hashtable changedProps)
        {
            string newTeam = null;
            string newName = null;
            if (changedProps.ContainsKey(PlayerProperty.Team))
            {
                newTeam = (string)changedProps[PlayerProperty.Team];
                Character.Team = newTeam;
            }
            if (changedProps.ContainsKey(PlayerProperty.Name))
            {
                newName = (string)changedProps[PlayerProperty.Name];
                Character.Name = newName.StripIllegalRichText();
            }

            SettingsManager_OnSettingsChanged();
        }

        private void SettingsManager_OnSettingsChanged()
        {
            ShowMode showMode = (ShowMode)SettingsManager.UISettings.NameOverrideTarget.Value;
            _settingsApplyToMe = showMode == ShowMode.All || (showMode == ShowMode.Mine && Character.IsMainCharacter()) ||
                (showMode == ShowMode.Others && !Character.IsMainCharacter());
            _settingsStyleType = (NameStyleType)SettingsManager.UISettings.NameBackgroundType.Value;
            _settingsBackgroundColor = SettingsManager.UISettings.ForceBackgroundColor.Value.ToColor();
            _settingsNameColor = SettingsManager.UISettings.ForceNameColor.Value.ToColor();
            _teamsEnabled = SettingsManager.InGameCurrent.Misc.PVP.Value == (int)PVPMode.Team;

            _name = Character.Name;
            if (_settingsApplyToMe)
            {
                _textOutline.enabled = _settingsStyleType == NameStyleType.Outline;
                _nameBackground.enabled = _settingsStyleType == NameStyleType.Background;
                _textOutline.effectColor = _settingsBackgroundColor;
                _nameBackground.color = _settingsBackgroundColor;
                _nameLabel.color = _settingsNameColor;
                if (!_teamsEnabled)
                    _name = Character.VisibleName;
                else
                {
                    _name = Character.VisibleName;
                    string color = TeamInfo.GetTeamColor(Character.Team);
                    _name = $"<color={color}>{_name}</color>";

                }
            }
            else
            {
                _textOutline.enabled = false;
                _nameBackground.enabled = false;
                _nameLabel.color = _settingsNameColor;
                if (!_teamsEnabled)
                    _name = Character.Name;
                else
                {
                    _name = Character.VisibleName;
                    string color = TeamInfo.GetTeamColor(Character.Team);
                    _name = $"<color={color}>{_name}</color>";
                }
            }
            SetName(_name);
        }

        public void SetName(string name)
        {
            _name = name;
            if (Character.Guild != "")
                _name = Character.Guild + "\n" + _name;
            _nameLabel.text = _name;
        }

        public void Load(BaseCharacter character, Vector3 offset, float range)
        {
            Character = character;
            Offset = offset;
            Range = range;
            _name = character.Name;
            Character.OnPlayerPropertiesChanged += Character_OnPlayerPropertiesChanged;
            SettingsManager_OnSettingsChanged();
        }

        public void ToggleName(bool toggle)
        {
            if (_nameBackground.gameObject.activeSelf != toggle)
                _nameBackground.gameObject.SetActive(toggle);
            if (_nameLabel.gameObject.activeSelf != toggle)
                _nameLabel.gameObject.SetActive(toggle);
        }

        public void ToggleHealthbar(bool toggle)
        {
            if (_healthbar.gameObject.activeSelf != toggle)
                _healthbar.gameObject.SetActive(toggle);
        }

        public void SetHealthbar(int currentHealth, int maxHealth, Color color)
        {
            if (maxHealth <= 0)
                _healthbarFill.fillAmount = 0;
            else
                _healthbarFill.fillAmount = Mathf.Clamp((float)currentHealth / (float)maxHealth, 0f, 1f);
            _healthbarLabel.text = currentHealth.ToString() + "/" + maxHealth.ToString();
            if (_healthbarFill.color != color)
                _healthbarFill.color = color;
        }
    }
}
