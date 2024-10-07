using ApplicationManagers;
using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class SnapshotPopup : BasePopup
    {
        protected override float AnimationTime => 0.1f;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        private RawImage _image;

        public override void Setup(BasePanel parent = null)
        {
            _image = transform.Find("Image").GetComponent<RawImage>();
        }

        public virtual void Load(Texture2D texture)
        {
            _image.texture = texture;
        }
    }
}
