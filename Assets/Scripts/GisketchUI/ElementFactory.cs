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

        public static Button CreateButton(Transform parent, string label, UnityEngine.Events.UnityAction onClick)
        {
            Button button = Object.Instantiate(Resources.Load<Button>("GisketchUI/Prefabs/Button"), parent);
            button.SetLabel(label);
            button.AddListener(onClick);
            return button;
        }

        // Add more factory methods as needed
    }
}