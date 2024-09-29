using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace GisketchUI
{
    public class SidePanelManager : MonoBehaviour
    {
        private static SidePanelManager _instance;
        public static SidePanelManager Instance => _instance;

        private Dictionary<System.Type, string> panelPrefabPaths = new Dictionary<System.Type, string>();
        private List<SidePanel> activePanels = new List<SidePanel>();
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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void InitializePanelPaths()
        {
            panelPrefabPaths[typeof(IntroPanel)] = "GisketchUI/Prefabs/IntroPanel";
            panelPrefabPaths[typeof(SettingsPanel)] = "GisketchUI/Prefabs/SettingsPanel";
        }

        public void ShowPanel<T>() where T : SidePanel
        {
            HideCurrentPanel();

            if (panelPrefabPaths.TryGetValue(typeof(T), out string prefabPath))
            {
                T panelPrefab = Resources.Load<T>(prefabPath);
                if (panelPrefab != null)
                {
                    Canvas targetCanvas = typeof(T) == typeof(IntroPanel) ? nonScalingCanvas : mainCanvas;
                    currentPanel = Instantiate(panelPrefab, targetCanvas.transform);
                    currentPanel.Initialize();
                    currentPanel.Show();
                    activePanels.Add(currentPanel);
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
                StartCoroutine(DestroyPanelAfterDelay(currentPanel, 0.5f));
                activePanels.Remove(currentPanel);
                currentPanel = null;
            }
        }

        public void HideAllPanels()
        {
            foreach (var panel in activePanels.ToList())
            {
                if (panel != null)
                {
                    panel.Hide();
                    StartCoroutine(DestroyPanelAfterDelay(panel, 0.5f));
                }
            }
            activePanels.Clear();
            currentPanel = null;
        }

        private System.Collections.IEnumerator DestroyPanelAfterDelay(SidePanel panel, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (panel != null)
            {
                Destroy(panel.gameObject);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            HideAllPanels();
        }

        public void ShowSettingsPanel() => ShowPanel<SettingsPanel>();
        public void ShowIntroPanel() => ShowPanel<IntroPanel>();
    }
}