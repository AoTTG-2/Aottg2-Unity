using ApplicationManagers;
using Characters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class EmoteEmojiPopup : EmoteTextPopup
    {
        protected RawImage _emojiImage;
        protected bool _animated;
        protected bool _isFirstFrame;
        protected Texture _texture0;
        protected Texture _texture1;
        protected const float AnimationFrameTime = 0.5f;
        protected float _animationTimeLeft = 0f;

        public override void Setup(BasePanel parent = null)
        {
            _emojiImage = transform.Find("Panel/Emoji").GetComponent<RawImage>();
            _transform = transform;
        }

        public override void Load(string text, float showTime, BaseCharacter character, Vector3 offset)
        {
            ShowTimeLeft = showTime;
            Character = character;
            Offset = offset;
            _texture0 = null;
            _texture1 = null;
            _animated = false;
            string iconPath;
            if (text == "Speaking")
                iconPath = "Icons/Game/Speaking";
            else if (text.StartsWith("Emoji"))
            {
                iconPath = "Icons/Emotes/" + text;
                if (UIManager.AnimatedEmojis.Contains(text))
                {
                    _texture0 = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, iconPath + "_0", true);
                    _texture1 = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, iconPath + "_1", true);
                    _emojiImage.texture = _texture0;
                    _animated = true;
                    _animationTimeLeft = AnimationFrameTime;
                    return;
                }
            }
            else
                iconPath = "Icons/Profile/" + UIManager.GetProfileIcon(text);
            _emojiImage.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, iconPath, true);
        }

        protected void Update()
        {
            if (_animated)
            {
                _animationTimeLeft -= Time.deltaTime;
                if (_animationTimeLeft <= 0f)
                {
                    _animationTimeLeft = AnimationFrameTime;
                    if (_isFirstFrame)
                        _emojiImage.texture = _texture1;
                    else
                        _emojiImage.texture = _texture0;
                    _isFirstFrame = !_isFirstFrame;
                }
            }
        }
    }
}
