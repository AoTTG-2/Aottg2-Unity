using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ApplicationManagers;
using Settings;
using Characters;
using GameManagers;
using System.Collections;

namespace UI
{
    class SnapshotViewerMenu: BaseMenu
    {
        private SnapshotViewerMainPanel _mainPanel;
        private SnapshotPopup _snapshotPopup;
        private KillScorePopup _killScorePopup;
        private Text _nameLabel;
        private Text _statusLabel;
        private int _width;
        private int _height;
        Texture2D _currentSnapshot;

        public override void Setup()
        {
            base.Setup();
            _mainPanel = ElementFactory.CreateHeadedPanel<SnapshotViewerMainPanel>(transform, enabled: true).GetComponent<SnapshotViewerMainPanel>();
            _snapshotPopup = ElementFactory.InstantiateAndSetupPanel<SnapshotPopup>(transform, "Prefabs/Snapshot/SnapshotPopup").GetComponent<SnapshotPopup>();
            float scale = 1.5f;
            _snapshotPopup.transform.localScale = new Vector2(scale, scale);
            float scaleFactor = UIManager.CurrentMenu.GetComponent<Canvas>().scaleFactor;
            _width = (int)(768f * scale * scaleFactor);
            _height = (int)(432f * scale * scaleFactor);
            _snapshotPopup.transform.localRotation = Quaternion.identity;
            _killScorePopup = ElementFactory.CreateDefaultPopup<KillScorePopup>(transform);
            _nameLabel = ElementFactory.CreateDefaultLabel(transform, new ElementStyle(), "").GetComponent<Text>();
            _statusLabel = ElementFactory.CreateDefaultLabel(transform, new ElementStyle(), "").GetComponent<Text>();
            ElementFactory.SetAnchor(_mainPanel.gameObject, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(20f, -20f));
            ElementFactory.SetAnchor(_snapshotPopup.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, new Vector2(0f, 0f));
            ElementFactory.SetAnchor(_killScorePopup.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, new Vector2(460f, 250f));
            ElementFactory.SetAnchor(_nameLabel.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, new Vector2(0f, -275f));
            ElementFactory.SetAnchor(_statusLabel.gameObject, TextAnchor.LowerCenter, TextAnchor.LowerCenter, new Vector2(0f, 20f));
            LoadSnapshot(0);
        }

        public void Save()
        {
            StartCoroutine(SaveCoroutine());
        }

        private IEnumerator SaveCoroutine()
        {
            _statusLabel.text = "Saving...";
            yield return new WaitForEndOfFrame();
            Texture2D texture = new Texture2D(_width, _height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect((Screen.width * 0.5f) - (texture.width * 0.5f), ((Screen.height * 0.5f) - (texture.height * 0.5f)), (float)texture.width, (float)texture.height), 0, 0);
            texture.Apply();
            string[] textArr = new string[] { DateTime.Today.Month.ToString(), DateTime.Today.Day.ToString(), DateTime.Today.Year.ToString(), "-", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), ".png" };
            string fileName = string.Concat(textArr);
            SnapshotManager.SaveSnapshotFinish(texture, fileName);
            Destroy(texture);
            _statusLabel.text = string.Format("Saved snapshot to {0}", SnapshotManager.SnapshotPath.Replace("\\", "/"));
        }

        public void LoadSnapshot(int index)
        {
            _statusLabel.text = "";
            _nameLabel.text = "";
            if (SnapshotManager.GetLength() > 0 && index < SnapshotManager.GetLength() && index >= 0)
            {
                Texture2D snapshot = SnapshotManager.GetSnapshot(index);
                _snapshotPopup.Load(snapshot);
                _snapshotPopup.ShowImmediate();
                if (SnapshotManager.GetDamage(index) > 0)
                    _killScorePopup.ShowSnapshotViewer(SnapshotManager.GetDamage(index));
                else
                    _killScorePopup.ShowSnapshotViewer(0);
                _nameLabel.text = SettingsManager.ProfileSettings.Name.Value + " " + DateTime.Today.ToShortDateString();
                _nameLabel.transform.SetAsLastSibling();
                if (_currentSnapshot != null)
                    Destroy(_currentSnapshot);
                _currentSnapshot = snapshot;
            }
            else
            {
                _snapshotPopup.Hide();
                _killScorePopup.Hide();
            }
        }
    }
}
