using UnityEngine;
using UnityEngine.UI;
using SimpleJSONFixed;
using UI;
using System.Collections;
using DentedPixel;

namespace GisketchUI
{
    public class TipPanel : UIElement
    {
        [SerializeField] private Text _label;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private float _slideDistance = 650f;
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private float _tipChangeDuration = 20f;

        private int _currentTipIndex = -1;
        private int _currentColorIndex = -1;
        private Vector2 _initialPosition;
        private Coroutine _changeTipCoroutine;

        private Color[] _colors = new Color[]
        {
            ColorPalette.Blue,
            ColorPalette.Orange,
            ColorPalette.Green,
            ColorPalette.Red,
            ColorPalette.Purple
        };

        public void Setup()
        {
            if (_label == null)
            {
                _label = GetComponentInChildren<Text>();
            }
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _backgroundImage = gameObject.GetComponent<Image>();
            if (_backgroundImage == null)
            {
                _backgroundImage = gameObject.AddComponent<Image>();
            }
            _initialPosition = _rectTransform.anchoredPosition;
        }

        private void Awake()
        {
            _initialPosition = _rectTransform.anchoredPosition;
        }

        public override void Show(float duration = 0.3f)
        {
            gameObject.SetActive(true);
            ChangeTextContent();
            _rectTransform.anchoredPosition = _initialPosition + new Vector2(_slideDistance, 0);
            LeanTween.moveX(_rectTransform, _initialPosition.x, duration).setEaseOutBack();

            // Start the coroutine to change tips periodically
            _changeTipCoroutine = StartCoroutine(ChangeTipPeriodically());
        }

        public override void Hide(float duration = 0.3f)
        {
            // Stop the coroutine when hiding
            if (_changeTipCoroutine != null)
            {
                StopCoroutine(_changeTipCoroutine);
                _changeTipCoroutine = null;
            }

            LeanTween.moveX(_rectTransform, _initialPosition.x + _slideDistance, duration)
                .setEaseInBack()
                .setOnComplete(() => gameObject.SetActive(false));
        }

        private IEnumerator ChangeTipPeriodically()
        {
            while (true)
            {
                yield return new WaitForSeconds(_tipChangeDuration);
                SetRandomTip();
            }
        }

        public void SetRandomTip()
        {
            SlideOutAndChangeContent(ChangeTextContent);
        }

        public void SetPressAnyKey()
        {
            SlideOutAndChangeContent(() => _label.text = UIManager.GetLocaleCommon("PressAnyKey"));
        }

        private void SlideOutAndChangeContent(System.Action changeContent)
        {
            LeanTween.moveX(_rectTransform, _initialPosition.x + _slideDistance, _animationDuration)
                .setEaseInOutQuad()
                .setOnComplete(() =>
                {
                    changeContent();
                    ChangeBackgroundColor();
                    LeanTween.moveX(_rectTransform, _initialPosition.x, _animationDuration)
                        .setEaseInOutQuad();
                });
        }

        private void ChangeTextContent()
        {
            var tips = MainMenu.MainBackgroundInfo["Tips"];
            int tipIndex = _currentTipIndex;
            while (tipIndex == _currentTipIndex)
                tipIndex = Random.Range(0, tips.Count);
            _currentTipIndex = tipIndex;
            _label.text = "Tip: " + tips[tipIndex].Value;
        }

        private void ChangeBackgroundColor()
        {
            _currentColorIndex = (_currentColorIndex + 1) % _colors.Length;
            _backgroundImage.color = _colors[_currentColorIndex];
        }

        private void OnDisable()
        {
            // Ensure the coroutine is stopped when the object is disabled
            if (_changeTipCoroutine != null)
            {
                StopCoroutine(_changeTipCoroutine);
                _changeTipCoroutine = null;
            }
        }
    }
}