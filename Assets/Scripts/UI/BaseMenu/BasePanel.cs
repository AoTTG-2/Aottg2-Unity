using ApplicationManagers;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class BasePanel : MonoBehaviour
    {
        protected virtual string ThemePanel => "DefaultPanel";
        protected virtual float Width => 800f;
        protected virtual float Height => 600f;
        protected virtual float BorderVerticalPadding => 0f;
        protected virtual float BorderHorizontalPadding => 0f;
        protected virtual int VerticalPadding => 30;
        protected virtual int HorizontalPadding => 40;
        protected virtual float VerticalSpacing => 30f;
        protected virtual TextAnchor PanelAlignment => TextAnchor.UpperLeft;
        protected virtual bool DoublePanel => false;
        protected virtual bool DoublePanelDivider => true;
        protected virtual bool ScrollBar => false;
        protected virtual bool CategoryPanel => false;
        protected virtual bool UseLastCategory => true;
        protected virtual bool HasPremadeContent => false;
        protected Transform SinglePanel;
        protected Transform DoublePanelLeft;
        protected Transform DoublePanelRight;
        protected List<BasePopup> _popups = new List<BasePopup>();
        protected GameObject _currentCategoryPanel;
        protected StringSetting _currentCategoryPanelName = new StringSetting(string.Empty);
        protected Dictionary<string, Type> _categoryPanelTypes = new Dictionary<string, Type>();
        protected virtual string DefaultCategoryPanel => string.Empty;
        protected RawImage MaskBackground;
        public BasePanel Parent;

        protected void OnEnable()
        {
            if (transform.Find("Border") != null)
                transform.Find("Border").GetComponent<CanvasGroup>().blocksRaycasts = false;
            if (_currentCategoryPanel != null)
                _currentCategoryPanel.SetActive(true);
        }

        public virtual void Setup(BasePanel parent = null)
        {
            Parent = parent;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GetWidth(), GetHeight());
            if (transform.Find("Background") != null)
            {
                MaskBackground = transform.Find("Background").GetComponent<RawImage>();
                MaskBackground.texture = UIManager.GetThemeTexture(ThemePanel, "MainBody", "BackgroundTexture");
                MaskBackground.color = new Color(0f, 0f, 0f, 0.05f);
            }
            if (!CategoryPanel && !HasPremadeContent)
            {
                if (DoublePanel)
                {
                    GameObject panel = CreateDoublePanel(ScrollBar, DoublePanelDivider);
                    DoublePanelLeft = GetDoublePanelLeftTransform(panel);
                    DoublePanelRight = GetDoublePanelRightTransform(panel);
                }
                else
                {
                    SinglePanel = GetSinglePanelTransform(CreateSinglePanel(ScrollBar));
                }
            }
            else if (HasPremadeContent)
            {
                SetupPremadePanel();
            }
            SetupPopups();
            if (CategoryPanel)
            {
                RegisterCategoryPanels();
                string lastCategory = UIManager.GetLastcategory(GetType());
                if (UseLastCategory && lastCategory != string.Empty)
                    SetCategoryPanel(lastCategory);
                else
                    SetCategoryPanel(DefaultCategoryPanel);
            }
            UIManager.NeedResizeText = true;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            HideAllPopups();
            gameObject.SetActive(false);
        }

        public virtual void SyncSettingElements()
        {
            foreach (BaseSettingElement setting in GetComponentsInChildren<BaseSettingElement>())
            {
                setting.SyncElement();
            }
        }

        protected virtual void SetupPremadePanel()
        {
            if (DoublePanel)
            {
                GameObject panel = transform.Find("DoublePanelContent").gameObject;
                DoublePanelLeft = GetDoublePanelLeftTransform(panel);
                DoublePanelRight = GetDoublePanelRightTransform(panel);
                BindPanel(panel, ScrollBar);
                SetPanelPadding(GetDoublePanelLeftTransform(panel).gameObject);
                SetPanelPadding(GetDoublePanelRightTransform(panel).gameObject);
            }
            else
            {
                GameObject panel = transform.Find("SinglePanelContent").gameObject;
                SinglePanel = GetSinglePanelTransform(panel);
                BindPanel(panel, ScrollBar);
                SetPanelPadding(GetSinglePanelTransform(panel).gameObject);
            }
        }

        protected virtual void SetupPopups()
        {
        }

        protected virtual void HideAllPopups()
        {
            foreach (BasePopup popup in _popups)
                popup.Hide();
        }

        protected virtual void RegisterCategoryPanels()
        {
        }

        public virtual void SetCategoryPanel(string name)
        {
            HideAllPopups();
            if (_currentCategoryPanel != null)
                Destroy(_currentCategoryPanel);
            Type t = _categoryPanelTypes[name];
            _currentCategoryPanelName.Value = name;
            _currentCategoryPanel = ElementFactory.CreateEmptyPanel(transform, t, enabled: true);
            _currentCategoryPanel.SetActive(false);
            _currentCategoryPanel.transform.localPosition = Vector3.up * GetPanelVerticalOffset();
            if (MaskBackground != null)
                MaskBackground.color = UIManager.GetThemeColor(ThemePanel, "MainBody", "BackgroundColor");
            StartCoroutine(WaitAndEnableCategoryPanel());
            UIManager.SetLastCategory(GetType(), name);
        }

        private IEnumerator WaitAndEnableCategoryPanel()
        {
            yield return new WaitForEndOfFrame();
            if (MaskBackground != null)
                MaskBackground.color = new Color(0f, 0f, 0f, 0.05f);
            _currentCategoryPanel.SetActive(true);
            UIManager.NeedResizeText = true;
        }

        public string GetCurrentCategoryName()
        {
            return _currentCategoryPanelName.Value;
        }

        public void RebuildCategoryPanel()
        {
            SetCategoryPanel(_currentCategoryPanelName);
        }

        public IEnumerator WaitAndRebuildCategoryPanel(float time)
        {
            yield return new WaitForSeconds(time);
            RebuildCategoryPanel();

        }

        public void SetCategoryPanel(StringSetting setting)
        {
            SetCategoryPanel(setting.Value);
        }

        protected GameObject CreateHorizontalDivider(Transform parent, float height = 1f)
        {
            float width;
            if (DoublePanel)
                width = GetPanelWidth() * 0.5f - HorizontalPadding * 2f;
            else
                width = GetPanelWidth() - HorizontalPadding * 2f;
            return ElementFactory.CreateHorizontalLine(parent, new ElementStyle(themePanel: ThemePanel), width, height);
        }

        protected Transform GetSinglePanelTransform(GameObject singlePanel)
        {
            return singlePanel.transform.Find("ScrollView/Panel");
        }

        protected Transform GetDoublePanelLeftTransform(GameObject doublePanel)
        {
            return doublePanel.transform.Find("ScrollView/LeftPanel");
        }

        protected Transform GetDoublePanelRightTransform(GameObject doublePanel)
        {
            return doublePanel.transform.Find("ScrollView/RightPanel");
        }

        protected GameObject CreateSinglePanel(bool scrollBar)
        {
            GameObject panel = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Prefabs/Panels/SinglePanelContent");
            GetSinglePanelTransform(panel).GetComponent<LayoutElement>().preferredWidth = GetPanelWidth();
            BindPanel(panel, scrollBar);
            SetPanelPadding(GetSinglePanelTransform(panel).gameObject);
            return panel;
        }

        protected GameObject CreateDoublePanel(bool scrollBar, bool divider)
        {
            GameObject panel = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Prefabs/Panels/DoublePanelContent");
            GetDoublePanelLeftTransform(panel).GetComponent<LayoutElement>().preferredWidth = GetPanelWidth() * 0.5f;
            GetDoublePanelRightTransform(panel).GetComponent<LayoutElement>().preferredWidth = GetPanelWidth() * 0.5f;
            if (divider)
            {
                Transform line = panel.transform.Find("ScrollView/VerticalLine");
                line.GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "MainBody", "LineColor");
                line.gameObject.AddComponent<VerticalLineScaler>();
            }
            else
            {
                panel.transform.Find("ScrollView/VerticalLine").gameObject.SetActive(false);
            }
            BindPanel(panel, scrollBar);
            SetPanelPadding(GetDoublePanelLeftTransform(panel).gameObject);
            SetPanelPadding(GetDoublePanelRightTransform(panel).gameObject);
            return panel;
        }

        protected virtual void BindPanel(GameObject panel, bool scrollBar)
        {
            panel.transform.SetParent(gameObject.transform, false);
            panel.transform.localPosition = Vector3.up * GetPanelVerticalOffset();
            float height = GetPanelHeight();
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(GetPanelWidth(), height);
            panel.transform.Find("ScrollView").GetComponent<LayoutElement>().minHeight = height;
            Scrollbar scrollbar = panel.transform.Find("Scrollbar").GetComponent<Scrollbar>();
            scrollbar.value = 1f;
            if (!scrollBar)
            {
                panel.GetComponent<ScrollRect>().verticalScrollbar = null;
                scrollbar.gameObject.SetActive(false);
            }
            panel.GetComponent<RawImage>().texture = UIManager.GetThemeTexture(ThemePanel, "MainBody", "BackgroundTexture");
            panel.GetComponent<RawImage>().color = UIManager.GetThemeColor(ThemePanel, "MainBody", "BackgroundColor");
            scrollbar.colors = UIManager.GetThemeColorBlock(ThemePanel, "MainBody", "Scrollbar");
            scrollbar.GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "MainBody", "ScrollbarBackgroundColor");
        }

        protected void SetPanelPadding(GameObject panel)
        {
            panel.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(HorizontalPadding, HorizontalPadding, VerticalPadding, VerticalPadding);
            panel.GetComponent<VerticalLayoutGroup>().spacing = VerticalSpacing;
            panel.GetComponent<VerticalLayoutGroup>().childAlignment = PanelAlignment;
        }

        public virtual float GetPanelWidth()
        {
            return GetWidth();
        }

        public virtual float GetPanelHeight()
        {
            return GetHeight();
        }

        public virtual float GetPanelVerticalOffset()
        {
            return 0f;
        }

        protected virtual float GetWidth()
        {
            return Width;
        }

        protected virtual float GetHeight()
        {
            return Height;
        }

        public float GetPhysicalWidth()
        {
            return GetComponent<RectTransform>().sizeDelta.x * UIManager.CurrentMenu.GetComponent<Canvas>().scaleFactor;
        }

        public float GetPhysicalHeight()
        {
            return GetComponent<RectTransform>().sizeDelta.y * UIManager.CurrentMenu.GetComponent<Canvas>().scaleFactor;
        }
    }
}
