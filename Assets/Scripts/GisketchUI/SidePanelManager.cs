using UnityEngine;
using System.Collections.Generic;
using Utility;

namespace GisketchUI
{
    public class SidePanelManager : MonoBehaviour
    {
        private static SidePanelManager _instance;
        public static SidePanelManager Instance => _instance;

        private Dictionary<System.Type, string> panelPrefabPaths = new Dictionary<System.Type, string>();
        private SidePanel currentPanel;

        private Canvas mainCanvas;
        private Canvas nonScalingCanvas;

        public static void Init()
        {
            if (_instance == null)
            {
                _instance = SingletonFactory.CreateSingleton(_instance);
                _instance.Initialize();
            }
        }

        private void Initialize()
        {
            mainCanvas = GisketchUIManager.Instance.MainCanvas;
            InitializePanelPaths();
            nonScalingCanvas = GisketchUIManager.Instance.CreateNonScalingCanvas("NonScalingCanvas");
        }

        private void InitializePanelPaths()
        {
            panelPrefabPaths[typeof(IntroPanel)] = "GisketchUI/Prefabs/IntroPanel";
            panelPrefabPaths[typeof(SettingsPanel)] = "GisketchUI/Prefabs/SettingsPanel";
        }

        public void ShowPanel<T>() where T : SidePanel
        {
            if (currentPanel != null)
            {
                Destroy(currentPanel.gameObject);
                currentPanel = null;
            }

            if (panelPrefabPaths.TryGetValue(typeof(T), out string prefabPath))
            {
                T panelPrefab = Resources.Load<T>(prefabPath);
                if (panelPrefab != null)
                {
                    Canvas targetCanvas = typeof(T) == typeof(IntroPanel) ? nonScalingCanvas : mainCanvas;
                    currentPanel = Instantiate(panelPrefab, targetCanvas.transform);
                    currentPanel.Initialize();
                    currentPanel.Show();
                }
                else
                {
                    Debug.LogError($"Failed to load panel prefab from path: {prefabPath}");
                }
            }
            else
            {
                Debug.LogError($"Panel of type {typeof(T)} not found.");
            }
        }

        public void HideCurrentPanel()
        {
            if (currentPanel != null)
            {
                currentPanel.Hide();
                Destroy(currentPanel.gameObject, 0.5f);
                currentPanel = null;
            }
        }

        public void ShowSettingsPanel()
        {
            ShowPanel<SettingsPanel>();
        }

        public void ShowIntroPanel()
        {
            ShowPanel<IntroPanel>();
        }
    }
}