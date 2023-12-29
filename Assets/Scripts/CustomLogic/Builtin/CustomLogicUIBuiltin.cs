using ApplicationManagers;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicUIBuiltin: CustomLogicBaseBuiltin
    {
        private Dictionary<string, string> _lastSetLabels = new Dictionary<string, string>();

        public CustomLogicUIBuiltin(): base("UI")
        {
        }

        public void OnPlayerJoin(Player player)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (string key in _lastSetLabels.Keys)
                    RPCManager.PhotonView.RPC("SetLabelRPC", player, new object[] { key, _lastSetLabels[key], 0f });
            }
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            var menu = (InGameMenu)UIManager.CurrentMenu;
            if (name == "SetLabel")
            {
                string label = (string)parameters[0];
                string message = (string)parameters[1];
                InGameManager.SetLabel(label, message, 0);
            }
            else if (name == "SetLabelForTime")
            {
                string label = (string)parameters[0];
                string message = (string)parameters[1];
                float time = parameters[2].UnboxToFloat();
                InGameManager.SetLabel(label, message, time);
            }
            else if (name == "SetLabelAll")
            {
                string label = (string)parameters[0];
                string message = (string)parameters[1];
                if (PhotonNetwork.IsMasterClient)
                {
                    if (!_lastSetLabels.ContainsKey(label) || message != _lastSetLabels[label])
                        RPCManager.PhotonView.RPC("SetLabelRPC", RpcTarget.All, new object[] { label, message, 0f });
                    _lastSetLabels[label] = message;
                }
            }
            else if (name == "SetLabelForTimeAll")
            {
                string label = (string)parameters[0];
                string message = (string)parameters[1];
                float time = parameters[2].UnboxToFloat();
                if (PhotonNetwork.IsMasterClient)
                {
                    if (!_lastSetLabels.ContainsKey(label) || message != _lastSetLabels[label])
                        RPCManager.PhotonView.RPC("SetLabelRPC", RpcTarget.All, new object[] { label, message, time });
                    _lastSetLabels[label] = message;
                }
            }
            else if (name == "CreatePopup")
            {
                string popupName = (string)parameters[0];
                string title = (string)parameters[1];
                int width = parameters[2].UnboxToInt();
                int height = parameters[3].UnboxToInt();
                menu.CreateCustomPopup(popupName, title, width, height);
            }
            else if (name == "ShowPopup")
            {
                string popupName = (string)parameters[0];
                menu.GetCustomPopup(popupName).Show();
            }
            else if (name == "HidePopup")
            {
                string popupName = (string)parameters[0];
                menu.GetCustomPopup(popupName).Hide();
            }
            else if (name == "ClearPopup")
            {
                string popupName = (string)parameters[0];
                menu.GetCustomPopup(popupName).Clear();
            }
            else if (name == "AddPopupLabel")
            {
                string popupName = (string)parameters[0];
                menu.GetCustomPopup(popupName).AddLabel((string)parameters[1]);
            }
            else if (name == "AddPopupButton")
            {
                string popupName = (string)parameters[0];
                menu.GetCustomPopup(popupName).AddButton((string)parameters[1], (string)parameters[2]);
            }
            else if (name == "AddPopupBottomButton")
            {
                string popupName = (string)parameters[0];
                menu.GetCustomPopup(popupName).AddBottomButton((string)parameters[1], (string)parameters[2]);
            }
            else if (name == "AddPopupButtons")
            {
                string popupName = (string)parameters[0];
                CustomLogicListBuiltin names = (CustomLogicListBuiltin)parameters[1];
                CustomLogicListBuiltin titles = (CustomLogicListBuiltin)parameters[2];
                menu.GetCustomPopup(popupName).AddButtons(names.List, titles.List);
            }
            return base.CallMethod(name, parameters);
        }
    }
}
