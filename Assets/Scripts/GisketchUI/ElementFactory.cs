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

        // Add more factory methods as needed
    }
}