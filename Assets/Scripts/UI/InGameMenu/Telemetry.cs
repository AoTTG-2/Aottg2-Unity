using CustomLogic;
using GameManagers;
using Photon.Pun;
using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class Telemetry : MonoBehaviour
    {
        private MultiTextLabel timePanel;
        private MultiTextLabel performancePanel;
        private ElementStyle _style;

        // Cache
        private StringBuilder _sb = new StringBuilder();
        private const string _gameTimeFormat = "{0:0.00}";

        // Syncing
        private const float MaxSyncDelay = 0.05f;
        private float _currentSyncDelay = 1f;

        public void Setup(ElementStyle _style)
        {
            // Get the vertical group component on this gameobject
            timePanel = ElementFactory.CreateMultiTextLabel(
                transform, _style, FontStyle.Normal, TextAnchor.MiddleLeft, 12f, 4
            ).GetComponent<MultiTextLabel>();

            timePanel.SetValue(0, "Game Time: ");
            timePanel.SetValue(1, string.Empty);
            timePanel.SetValue(2, "System: ");
            timePanel.SetValue(3, string.Empty);

            Color systemColor = ColorUtility.TryParseHtmlString(ChatManager.ColorTags[ChatTextColor.System], out Color color) ? color : Color.yellow;
            timePanel.ChangeTextColor(1, systemColor);
            timePanel.ChangeTextColor(3, systemColor);
            timePanel.SetEnabled(false);

            performancePanel = ElementFactory.CreateMultiTextLabel(
                transform, _style, FontStyle.Normal, TextAnchor.MiddleLeft, 12f, 4
            ).GetComponent<MultiTextLabel>();

            performancePanel.SetValue(0, "FPS: ");
            performancePanel.SetValue(1, string.Empty);
            performancePanel.SetValue(2, "Ping: ");
            performancePanel.SetValue(3, string.Empty);
            performancePanel.SetEnabled(false);
        }

        private void Update()
        {
            _currentSyncDelay -= Time.deltaTime;
            if (_currentSyncDelay <= 0f)
                Sync();
        }

        private void Sync()
        {
            _currentSyncDelay = MaxSyncDelay;
            if (SettingsManager.UISettings.ShowGameTime.Value)
            {
                timePanel.SetEnabled(true);
                UpdateGameTime(CustomLogicManager.Evaluator?.CurrentTime ?? 0);
                UpdateSystemTime(DateTime.Now);
            }
            else
                timePanel.SetEnabled(false);

            // disable panel if both fps and ping are disabled and then return
            if (SettingsManager.GraphicsSettings.ShowFPS.Value || SettingsManager.UISettings.ShowPing.Value)
            {
                performancePanel.SetEnabled(true);
                UpdatePerformance();
            }
            else
                performancePanel.SetEnabled(false);
        }

        private void UpdatePerformance()
        {
            if (SettingsManager.GraphicsSettings.ShowFPS.Value)
            {
                performancePanel.SetElementEnabled(0, true);
                performancePanel.SetElementEnabled(1, true);
                _sb.Clear();
                _sb.Append(UIManager.GetFPS());
                if (SettingsManager.UISettings.ShowPing.Value)
                    _sb.Append(", ");
                performancePanel.SetValue(1, _sb.ToString());
            }
            else
            {
                performancePanel.SetElementEnabled(0, false);
                performancePanel.SetElementEnabled(1, false);
            }

            if (SettingsManager.UISettings.ShowPing.Value)
            {
                performancePanel.SetElementEnabled(2, true);
                performancePanel.SetElementEnabled(3, true);
                performancePanel.SetValue(3, PhotonNetwork.GetPing().ToString());
            }
            else
            {
                performancePanel.SetElementEnabled(2, false);
                performancePanel.SetElementEnabled(3, false);
            }
            
            
        }

        private void UpdateGameTime(float currentTime)
        {
            // format time 0.00
            _sb.Clear();
            _sb.AppendFormat(_gameTimeFormat, currentTime);
            _sb.Append(", ");
            timePanel.SetValue(1, _sb.ToString());
        }

        private void UpdateSystemTime(DateTime dt)
        {
            _sb.Clear();
            _sb.Append((char)('0' + dt.Hour / 10));
            _sb.Append((char)('0' + dt.Hour % 10));
            _sb.Append(':');
            _sb.Append((char)('0' + dt.Minute / 10));
            _sb.Append((char)('0' + dt.Minute % 10));
            _sb.Append(':');
            _sb.Append((char)('0' + dt.Second / 10));
            _sb.Append((char)('0' + dt.Second % 10));

            timePanel.SetValue(3, _sb.ToString());
        }
    }
}
