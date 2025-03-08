using ApplicationManagers;
using Characters;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class EmoteEmojiPopup : EmoteTextPopup
    {
        protected RawImage _emojiImage;

        public override void Setup(BasePanel parent = null)
        {
            _emojiImage = transform.Find("Panel/Emoji").GetComponent<RawImage>();
            _transform = transform;
        }

        public override void Load(string text, float showTime, BaseCharacter character, Vector3 offset)
        {
            string iconPath;
            if (text == "Speaking")
                iconPath = "Icons/Game/Speaking";
            else
                iconPath = "Icons/Profile/" + UIManager.GetProfileIcon(text);
            _emojiImage.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, iconPath, true);
            ShowTimeLeft = showTime;
            Character = character;
            Offset = offset;
        }
    }
}
