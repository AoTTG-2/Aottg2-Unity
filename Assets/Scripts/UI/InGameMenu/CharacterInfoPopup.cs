using ApplicationManagers;
using Characters;
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
        public BaseCharacter Character;
        public Vector3 Offset;
        public float Range;
        private bool _highlyVisible = false;

        public override void Setup(BasePanel parent = null)
        {
            _nameBackground = transform.Find("Name").GetComponent<Image>();
            _nameLabel = transform.Find("Name/NameLabel").GetComponent<Text>();
            _healthbar = transform.Find("Healthbar").gameObject;
            _healthbarFill = _healthbar.transform.Find("Fill").GetComponent<Image>();
            _healthbarLabel = _healthbar.transform.Find("Label").GetComponent<Text>();
        }

        public void Load(BaseCharacter character, Vector3 offset, float range)
        {
            Character = character;
            Offset = offset;
            Range = range;
        }

        public void ToggleName(bool toggle)
        {
            if (_nameBackground.gameObject.activeSelf != toggle)
                _nameBackground.gameObject.SetActive(toggle);
        }

        public void ToggleHealthbar(bool toggle)
        {
            if (_healthbar.gameObject.activeSelf != toggle)
                _healthbar.gameObject.SetActive(toggle);
        }

        public void SetName(string name, bool highlyVisibleEnabled, Color backgroundColor)
        {
            if (highlyVisibleEnabled && !_highlyVisible)
            {
                _nameBackground.color = backgroundColor;
                _highlyVisible = true;
            }
            else if (!highlyVisibleEnabled && _highlyVisible)
            {
                 _nameBackground.color = new Color(0,0,0,0);
                _highlyVisible = false;
            }

            _nameLabel.text = name;
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
