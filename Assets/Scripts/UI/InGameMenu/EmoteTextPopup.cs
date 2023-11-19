using ApplicationManagers;
using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class EmoteTextPopup : BasePopup
    {
        protected override float AnimationTime => 0.25f;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        private Text _text;
        protected Transform _parent;
        protected Transform _transform;
        public float ShowTimeLeft = 0f;
        public BaseCharacter Character;
        public Vector3 Offset;

        public override void Setup(BasePanel parent = null)
        {
            _text = transform.Find("Panel/Text/Label").GetComponent<Text>();
            _transform = transform;
        }

        public virtual void Load(string text, float showTime, BaseCharacter character, Vector3 offset)
        {
            _text.text = text;
            ShowTimeLeft = showTime;
            Character = character;
            Offset = offset;
        }
    }
}
