using UnityEngine;
using UnityEngine.UI;
using static LeanTween;

namespace GisketchUI
{
    public abstract class GisketchUIElement : MonoBehaviour
    {
        public virtual void Show(float duration = 0.3f) { }
        public virtual void Hide(float duration = 0.3f) { }
    }

    public abstract class GisketchUIPanel : GisketchUIElement
    {
        public RectTransform rectTransform;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public override void Show(float duration = 0.3f)
        {
            gameObject.SetActive(true);
            LeanTween.scale(rectTransform, Vector3.one, duration).setEaseOutBack();
        }

        public override void Hide(float duration = 0.3f)
        {
            LeanTween.scale(rectTransform, Vector3.zero, duration).setEaseInBack()
                .setOnComplete(() => gameObject.SetActive(false));
        }
    }
}