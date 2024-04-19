using ApplicationManagers;
using GameManagers;
using Settings;
using Photon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Characters;
using static UnityEditor.PlayerSettings;

namespace UI
{
    class ExpeditionPlayerPanel : SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;
        private Player selectedPlayer;
        private string tpCoords;
        private List<SaveableSettingsContainer> _saveableSettings = new List<SaveableSettingsContainer>();

        public override void Setup(BasePanel parent = null) //Zippy: Finish the player List
        {
            base.Setup(parent);
            ExpeditionPopup RolesPopup = (ExpeditionPopup)parent;
            string cat = RolesPopup.LocaleCategory; //zippy: fix the null ref error
            string sub = "Players";
            EMSettings settings = SettingsManager.EMSettings;

            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);

            #region Left Side

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                string name = ChatManager.GetIDString(player.ActorNumber, player.IsMasterClient) + player.GetStringProperty(PlayerProperty.Name);
                GameObject obj = ElementFactory.CreateDefaultButton(DoublePanelLeft, style, name, 400f, 25f,
                                                                    onClick: () => OnPlayerButtonClick(player));
            }

            #endregion

            #region Right Side

            ElementFactory.CreateDefaultButton(DoublePanelRight, style, "TP All To Me", 400f, 20f, onClick: () => OnTPPlayerButtonClick(0));
            ElementFactory.CreateDefaultButton(DoublePanelRight, style, "TP Selected player to me", 400f, 20f, onClick: () => OnTPPlayerButtonClick(1));
            ElementFactory.CreateDefaultButton(DoublePanelRight, style, "TP Me to selected player", 400f, 20f, onClick: () => OnTPPlayerButtonClick(2));
            ElementFactory.CreateDefaultButton(DoublePanelRight, style, "TP Selected player to Coords", 400f, 20f, onClick: () => OnTPPlayerButtonClick(3));
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.TPcoords, "Coordinates", tooltip: "Coordinates for TP button", elementWidth: 100f); 

            #endregion
        }

        private float timer = 0.0f;
        private void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
            if (timer >= 1.0f)
            {
                RebuildCategoryPanel();
                timer = 0.0f; // Reset the timer
            }
        }


        private void UpdateSettings()
        {
            _saveableSettings.Add(SettingsManager.EMSettings);

            foreach (SaveableSettingsContainer setting in _saveableSettings)
                setting.Load();
            RebuildCategoryPanel();

            _saveableSettings.Clear();
        }

        private void OnPlayerButtonClick(Player player)
        {
            selectedPlayer = player;
        }

        private void OnTPPlayerButtonClick(int setting)
        {
            GameObject TargetplayerGameObject = PhotonExtensions.GetPlayerFromID(selectedPlayer.ActorNumber);
            Vector3 Mypos = PhotonExtensions.GetMyPlayer().transform.position;

            switch (setting) 
            { 
                case 0: //TP all to me 
                    foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        go.GetComponent<Human>().photonView.RPC("moveToRPC", RpcTarget.Others, new object[] { Mypos.x, Mypos.y, Mypos.z });
                    }
                    break;
                case 1: //TP player to me
                    TargetplayerGameObject.GetComponent<Human>().photonView.RPC("moveToRPC", selectedPlayer, new object[] { Mypos.x, Mypos.y, Mypos.z });
                    break;
                case 2: //TP me to player
                    GameObject ME = PhotonExtensions.GetMyPlayer();
                    ME.transform.position = TargetplayerGameObject.transform.position;
                    break;
                case 3: //TP player to coords
                    UpdateSettings();
                    tpCoords = SettingsManager.EMSettings.TPcoords.Value;
                    string[] tpCoordsSplit = tpCoords.Split(' ');
                    TargetplayerGameObject.GetComponent<Human>().photonView.RPC("moveToRPC", selectedPlayer, new object[]
                    {
                    float.Parse(tpCoordsSplit[0]),
                    float.Parse(tpCoordsSplit[1]),
                    float.Parse(tpCoordsSplit[2])
                    });
                break;
            }
        }
    }
}
