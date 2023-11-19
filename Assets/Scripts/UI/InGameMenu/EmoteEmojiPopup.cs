using Characters;
using UnityEngine;
using UnityEngine.UI;

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
            _emojiImage.texture = EmoteHandler.EmojiTextures[text];
            ShowTimeLeft = showTime;
            Character = character;
            Offset = offset;
        }
    }
}
