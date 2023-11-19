using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;

namespace UI
{
    class WheelPopup : BasePopup
    {
        protected override float AnimationTime => 0.2f;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        private Text _centerText;
        private List<GameObject> _buttons = new List<GameObject>();
        public int SelectedItem = 0;
        private UnityAction _callback;

        public override void Setup(BasePanel parent = null)
        {
            _centerText = transform.Find("Panel/Center/Label").GetComponent<Text>();
            for (int i = 0; i < 8; i++)
            {
                _buttons.Add(ElementFactory.InstantiateAndBind(transform.Find("Panel/Buttons"), "Prefabs/InGame/WheelButton").gameObject);
                int index = i;
                _buttons[index].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(index));
            }

            ElementFactory.SetAnchor(_buttons[0], TextAnchor.MiddleCenter, TextAnchor.LowerCenter, new Vector2(0f, 180f));
            ElementFactory.SetAnchor(_buttons[1], TextAnchor.MiddleCenter, TextAnchor.LowerLeft, new Vector2(135f, 90f));
            ElementFactory.SetAnchor(_buttons[2], TextAnchor.MiddleCenter, TextAnchor.MiddleLeft, new Vector2(180f, 0f));
            ElementFactory.SetAnchor(_buttons[3], TextAnchor.MiddleCenter, TextAnchor.UpperLeft, new Vector2(135f, -90f));
            ElementFactory.SetAnchor(_buttons[4], TextAnchor.MiddleCenter, TextAnchor.UpperCenter, new Vector2(0f, -180f));
            ElementFactory.SetAnchor(_buttons[5], TextAnchor.MiddleCenter, TextAnchor.UpperRight, new Vector2(-135f, -90f));
            ElementFactory.SetAnchor(_buttons[6], TextAnchor.MiddleCenter, TextAnchor.MiddleRight, new Vector2(-180f, 0f));
            ElementFactory.SetAnchor(_buttons[7], TextAnchor.MiddleCenter, TextAnchor.LowerRight, new Vector2(-135f, 90f));
        }

        public void Show(string openKey, List<string> options, UnityAction callback)
        {
            if (gameObject.activeSelf)
            {
                StopAllCoroutines();
                SetTransformAlpha(MaxFadeAlpha);
            }
            SetCenterText(openKey);
            _callback = callback;
            for (int i = 0; i < options.Count; i++)
            {
                _buttons[i].SetActive(true);
                KeybindSetting keybind = (KeybindSetting)SettingsManager.InputSettings.Interaction.Settings["QuickSelect" + (i + 1).ToString()];
                _buttons[i].transform.Find("Text").GetComponent<Text>().text = keybind.ToString() + " - " + options[i];
            }
            for (int i = options.Count; i < _buttons.Count; i++)
                _buttons[i].SetActive(false);
            base.Show();
        }

        private void SetCenterText(string openKey)
        {
            _centerText.text = SettingsManager.InputSettings.Interaction.MenuNext.ToString() + " - " + UIManager.GetLocaleCommon("Next") + "\n";
            _centerText.text += openKey + " - " + UIManager.GetLocaleCommon("Cancel");
        }

        private void OnButtonClick(int index)
        {
            SelectedItem = index;
            _callback.Invoke();
        }

        private void Update()
        {
            for (int i = 0; i < 8; i++)
            {
                KeybindSetting keybind = (KeybindSetting)SettingsManager.InputSettings.Interaction.Settings["QuickSelect" + (i + 1).ToString()];
                if (keybind.GetKeyDown())
                    OnButtonClick(i);
            }
        }
    }
}
