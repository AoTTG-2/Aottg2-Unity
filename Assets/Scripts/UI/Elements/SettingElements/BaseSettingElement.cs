using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections.Generic;

namespace UI
{
    abstract class BaseSettingElement: MonoBehaviour
    {
        protected BaseSetting _setting;
        protected SettingType _settingType;
        protected ElementStyle _style;
        protected virtual HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>();

        public virtual void Setup(BaseSetting setting, ElementStyle style, string title, string tooltip)
        {
            _setting = setting;
            _settingType = GetSettingType(setting);
            _style = style;
            if (!SupportedSettingTypes.Contains(_settingType))
                throw new ArgumentException("Unsupported setting type being used for UI element.");
            SetupTitle(title, style.FontSize, style.TitleWidth);
            SetupTooltip(tooltip, style);
            SyncElement();
        }

        protected void SetupTooltip(string tooltip, ElementStyle style)
        {
            GameObject icon = transform.Find("TooltipIcon").gameObject;
            if (tooltip == string.Empty)
            {
                icon.SetActive(false);
                return;
            }
            TooltipButton button = icon.AddComponent<TooltipButton>();
            button.Setup(tooltip, style);
        }

        public abstract void SyncElement();

        protected SettingType GetSettingType(BaseSetting setting)
        {
            return SettingsUtil.GetSettingType(setting);
        }

        protected void SetupTitle(string title, int fontSize, float titleWidth)
        {
            GameObject label = gameObject.transform.Find("Label").gameObject;
            if (title == string.Empty)
                label.SetActive(false);
            else
            {
                SetupLabel(label, title, fontSize);
                label.GetComponent<LayoutElement>().preferredWidth = titleWidth;
                if (titleWidth <= 0f)
                    label.GetComponent<LayoutElement>().preferredWidth = -1;
                label.GetComponent<Text>().color = UIManager.GetThemeColor(_style.ThemePanel, "DefaultSetting", "TextColor");
            }
        }

        protected void SetupLabel(GameObject obj, string title, int fontSize)
        {
            Text text = obj.GetComponent<Text>();
            text.text = title;
            text.fontSize = fontSize;
        }

        protected void SetupLabel(GameObject obj, string title)
        {
            Text text = obj.GetComponent<Text>();
            text.text = title;
        }
    }
}
