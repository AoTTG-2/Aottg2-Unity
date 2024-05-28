using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Settings;
using ApplicationManagers;
using GameManagers;
using CustomLogic;
using Photon.Pun;
using Characters;

namespace UI
{
    class CharacterChangePopup : CharacterPopup
    {
        protected override void SetAllowedCategories()
        {
            _allowedCategories.Add("Human");
        }

        protected override void SetupBottomButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"),
                    onClick: () => OnBottomBarButtonClick("Back"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Join"),
                    onClick: () => OnBottomBarButtonClick("Join"));
        }

        private void OnBottomBarButtonClick(string name)
        {
            var manager = (InGameManager)SceneLoader.CurrentGameManager;
            switch (name)
            {
                case "Back":
                    Hide();
                    break;
                case "Join":
                    SettingsManager.InGameCharacterSettings.ChooseStatus.Value = (int)ChooseCharacterStatus.Chosen;
                    var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
                    if (gameManager.CurrentCharacter != null && gameManager.CurrentCharacter is Human)
                    {
                        var human = (Human)gameManager.CurrentCharacter;
                        if (!human.Dead && human.State != HumanState.Grab)
                        {
                            var position = human.Cache.Transform.position;
                            var rotation = human.Cache.Transform.rotation;
                            human.DieChangeCharacter();
                            manager.SpawnPlayerAt(false, position, rotation.eulerAngles.y);
                        }
                    }
                    else
                    {
                        bool canJoin = PhotonNetwork.IsMasterClient || CustomLogicManager.Evaluator.CurrentTime < SettingsManager.InGameCurrent.Misc.AllowSpawnTime.Value;
                        if (canJoin && !manager.HasSpawned)
                            manager.SpawnPlayer(false);
                    }
                    InGameManager.UpdateRoundPlayerProperties();
                    Hide();
                    break;
            }
        }
    }
}
