using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Settings;
using System.Collections;
using ApplicationManagers;

namespace UI
{
    abstract class BaseMenu: MonoBehaviour
    {
        protected List<BasePopup> _popups = new List<BasePopup>();
        public TooltipPopup TooltipPopup;
        public MessagePopup MessagePopup;
        public ConfirmPopup ConfirmPopup;
        public ExternalLinkPopup ExternalLinkPopup;
        public ColorPickPopup ColorPickPopup;
        public Vector3Popup Vector3Popup;
        public ExportPopup ExportPopup;
        public ImportPopup ImportPopup;
        public NewImportPopup NewImportPopup;
        public KeybindPopup KeybindPopup;
        public SetNamePopup SetNamePopup;
        public SelectListPopup SelectListPopup;

        public virtual void Setup()
        {
            SetupPopups();
        }

        public void ApplyScale(SceneName sceneName)
        {
            StartCoroutine(WaitAndApplyScale(sceneName));
        }

        protected IEnumerator WaitAndApplyScale(SceneName sceneName)
        {
            float scaleFactor = 1f / SettingsManager.UISettings.UIMasterScale.Value;
            if (sceneName == SceneName.CharacterEditor || sceneName == SceneName.MapEditor || sceneName == SceneName.SnapshotViewer)
                scaleFactor = 1f;
            GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920 * scaleFactor, 1080 * scaleFactor);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            UIManager.CurrentCanvasScale = GetComponent<RectTransform>().localScale.x;
            foreach (BaseScaler scaler in GetComponentsInChildren<BaseScaler>(includeInactive: true))
            {
                scaler.ApplyScale();
            }
        }

        protected virtual void SetupPopups()
        {
            TooltipPopup = ElementFactory.CreateTooltipPopup(transform).GetComponent<TooltipPopup>();
            MessagePopup = ElementFactory.CreateDefaultPopup<MessagePopup>(transform).GetComponent<MessagePopup>();
            ConfirmPopup = ElementFactory.CreateDefaultPopup<ConfirmPopup>(transform).GetComponent<ConfirmPopup>();
            ExternalLinkPopup = ElementFactory.CreateDefaultPopup<ExternalLinkPopup>(transform).GetComponent<ExternalLinkPopup>();
            ColorPickPopup = ElementFactory.CreateDefaultPopup<ColorPickPopup>(transform).GetComponent<ColorPickPopup>();
            Vector3Popup = ElementFactory.CreateDefaultPopup<Vector3Popup>(transform).GetComponent<Vector3Popup>();
            ExportPopup = ElementFactory.CreateDefaultPopup<ExportPopup>(transform).GetComponent<ExportPopup>();
            ImportPopup = ElementFactory.CreateDefaultPopup<ImportPopup>(transform).GetComponent<ImportPopup>();
            NewImportPopup = ElementFactory.CreateDefaultPopup<NewImportPopup>(transform).GetComponent<NewImportPopup>();
            KeybindPopup = ElementFactory.CreateDefaultPopup<KeybindPopup>(transform).GetComponent<KeybindPopup>();
            SetNamePopup = ElementFactory.CreateDefaultPopup<SetNamePopup>(transform).GetComponent<SetNamePopup>();
            SelectListPopup = ElementFactory.CreateDefaultPopup<SelectListPopup>(transform).GetComponent<SelectListPopup>();
            _popups.Add(TooltipPopup);
            _popups.Add(MessagePopup);
            _popups.Add(ConfirmPopup);
            _popups.Add(ExternalLinkPopup);
            _popups.Add(ColorPickPopup);
            _popups.Add(Vector3Popup);
            _popups.Add(ExportPopup);
            _popups.Add(ImportPopup);
            _popups.Add(NewImportPopup);
            _popups.Add(KeybindPopup);
            _popups.Add(SetNamePopup);
            _popups.Add(SelectListPopup);
        }

        protected virtual void HideAllPopups()
        {
            foreach (BasePopup popup in _popups)
            {
                popup.Hide();
            }
        }
    }
}
