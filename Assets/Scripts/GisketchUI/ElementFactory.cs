using UnityEngine;
using UnityEngine.UI;

namespace GisketchUI
{
    public static class ElementFactory
    {
        public static PanelContentView CreatePanel(Transform parent, string headerText, ContentLayoutType layoutType)
        {
            PanelContentView panel = Object.Instantiate(Resources.Load<PanelContentView>("GisketchUI/Prefabs/PanelContentView"), parent);
            panel.Initialize(headerText, layoutType);
            return panel;
        }

        public static ActionButton CreateButton(Transform parent, string label, UnityEngine.Events.UnityAction onClick, ActionButton.ButtonVariant variant = ActionButton.ButtonVariant.Neutral)
        {
            ActionButton button = Object.Instantiate(Resources.Load<ActionButton>("GisketchUI/Prefabs/Button"), parent);
            button.SetButtonVariant(variant);
            button.SetLabel(label);
            button.AddListener(onClick);
            return button;
        }

        public static SidePanelButton CreateSidePanelButton(Transform parent, string label, System.Action onClick, SidePanelButton.ButtonVariant variant = SidePanelButton.ButtonVariant.Default)
        {
            SidePanelButton button = Object.Instantiate(Resources.Load<SidePanelButton>("GisketchUI/Prefabs/SidePanelButton"), parent);
            button.SetLabel(label);
            button.OnClick += onClick;
            button.SetVariant(variant);
            return button;
        }

        public static TipPanel CreateTipPanel(Transform parent)
        {
            TipPanel tipPanel = Object.Instantiate(Resources.Load<TipPanel>("GisketchUI/Prefabs/TipPanel"), parent);

            if (tipPanel == null)
            {
                Debug.LogError("TipPanel prefab not found.");
                return null;
            }

            // Set up RectTransform for bottom-right positioning
            RectTransform rectTransform = tipPanel.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 0);
            rectTransform.pivot = new Vector2(1, 0);
            rectTransform.anchoredPosition = new Vector2(30, 20);
            rectTransform.sizeDelta = new Vector2(560, 180);

            tipPanel.Setup();
            return tipPanel;
        }

        // Add more factory methods as needed
    }
}