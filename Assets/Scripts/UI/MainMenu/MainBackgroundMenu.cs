using UnityEngine;
using System.Collections;

namespace UI
{
    class MainBackgroundMenu : BaseMenu
    {
        public MainMenuBackgroundPanel _mainBackgroundPanelBack;
        public MainMenuBackgroundPanel _mainBackgroundPanelFront;

        // Define smooth damping variables
        private Vector2 currentVelocity;
        public float smoothTime = 0.3f;
        public float backgroundScale = 1.1f;

        public override void Setup()
        {
            SetupMainBackground();
        }

        private void SetupMainBackground()
        {
            _mainBackgroundPanelBack = ElementFactory.CreateDefaultPopup<MainMenuBackgroundPanel>(transform);
            _mainBackgroundPanelFront = ElementFactory.CreateDefaultPopup<MainMenuBackgroundPanel>(transform);
            _mainBackgroundPanelBack.SetRandomBackground(loading: false, seasonal: true);
            _mainBackgroundPanelBack.GetComponent<RectTransform>().localScale = Vector3.one * backgroundScale;
            _mainBackgroundPanelFront.GetComponent<RectTransform>().localScale = Vector3.one * backgroundScale;
            _mainBackgroundPanelBack.ShowImmediate();
            _mainBackgroundPanelFront.BackgroundIndex = _mainBackgroundPanelBack.BackgroundIndex;

            AddParallaxEffect(_mainBackgroundPanelBack.gameObject, 0.5f);
            AddParallaxEffect(_mainBackgroundPanelFront.gameObject, 0.5f);
        }

        private void AddParallaxEffect(GameObject target, float intensity)
        {
            ParallaxEffect effect = target.AddComponent<ParallaxEffect>();
            effect.parallaxIntensity = intensity;
            effect.scale = 1.1f;
            effect.smoothTime = 0.6f;
        }

        private void Update()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 targetPosition = GetConstrainedTargetPosition(mousePosition);

            // Smoothly damp the position of the background panels
            _mainBackgroundPanelBack.GetComponent<RectTransform>().anchoredPosition =
                Vector2.SmoothDamp(_mainBackgroundPanelBack.GetComponent<RectTransform>().anchoredPosition,
                                   targetPosition, ref currentVelocity, smoothTime);

            _mainBackgroundPanelFront.GetComponent<RectTransform>().anchoredPosition =
                Vector2.SmoothDamp(_mainBackgroundPanelFront.GetComponent<RectTransform>().anchoredPosition,
                                   targetPosition, ref currentVelocity, smoothTime);
        }

        private Vector2 GetConstrainedTargetPosition(Vector2 mousePosition)
        {
            // Calculate the range for x and y considering the scale applied
            float maxMovementX = (backgroundScale - 1) * (Screen.width / 2);
            float maxMovementY = (backgroundScale - 1) * (Screen.height / 2);

            // Map mouse position to the constrained range
            float backgroundX = MapRange(mousePosition.x, 0, Screen.width, -maxMovementX, maxMovementX);
            float backgroundY = MapRange(mousePosition.y, 0, Screen.height, -maxMovementY, maxMovementY);

            return new Vector2(backgroundX, backgroundY);
        }

        private float MapRange(float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public void ChangeMainBackground()
        {
            _mainBackgroundPanelFront.SetRandomBackground(loading: false);
            _mainBackgroundPanelFront.Show();
            StartCoroutine(WaitAndFinishBackground());
        }

        private IEnumerator WaitAndFinishBackground()
        {
            yield return new WaitForSeconds(1.5f);
            _mainBackgroundPanelBack.SetBackground(loading: false, backgroundIndex: _mainBackgroundPanelFront.BackgroundIndex);
            _mainBackgroundPanelFront.HideImmediate();
        }
    }
}