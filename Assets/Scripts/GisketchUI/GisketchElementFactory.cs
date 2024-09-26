using UnityEngine;
using UnityEngine.UI;

namespace GisketchUI
{
    public static class GisketchElementFactory
    {
        public static GisketchPanelContentView CreatePanel(Transform parent, string headerText, ContentLayoutType layoutType)
        {
            GisketchPanelContentView panel = Object.Instantiate(Resources.Load<GisketchPanelContentView>("GisketchUI/Prefabs/GisketchPanelContentView"), parent);
            panel.Initialize(headerText, layoutType);
            return panel;
        }

        public static GisketchButton CreateButton(Transform parent, string label, UnityEngine.Events.UnityAction onClick)
        {
            GisketchButton button = Object.Instantiate(Resources.Load<GisketchButton>("GisketchUI/Prefabs/GisketchButton"), parent);
            button.SetLabel(label);
            button.AddListener(onClick);
            return button;
        }

        // Add more factory methods as needed
    }
}