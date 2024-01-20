﻿using System;
using UnityEngine;
using ApplicationManagers;
using GameManagers;
using UnityEngine.UI;
using Settings;
using Characters;
using UI;
using CustomLogic;

namespace Controllers
{
    class BasePlayerController: MonoBehaviour
    {
        protected GeneralInputSettings _generalInput;
        protected InteractionInputSettings _interactionInput;
        protected InGameMenu _inGameMenu;
        protected BaseCharacter _character;
        protected InGameManager _gameManager;

        protected virtual void Awake()
        {
            _generalInput = SettingsManager.InputSettings.General;
            _interactionInput = SettingsManager.InputSettings.Interaction;
            _character = GetComponent<BaseCharacter>();
            _inGameMenu = (InGameMenu)UIManager.CurrentMenu;
            _gameManager = (InGameManager)SceneLoader.CurrentGameManager;
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
            if (_gameManager.GlobalPause)
                return;
            bool inMenu = InGameMenu.InMenu() || ChatManager.IsChatActive() || CustomLogicManager.Cutscene;
            UpdateMovementInput(inMenu);
            UpdateMenuInput(inMenu);
            UpdateActionInput(inMenu);
            UpdateUI(inMenu);
        }

        protected virtual void UpdateActionInput(bool inMenu)
        {
            if (!inMenu && !_character.Dead)
            {
                for (int i = 0; i < 8; i++)
                {
                    string name = "QuickSelect" + (i + 1).ToString();
                    if (((KeybindSetting)SettingsManager.InputSettings.Interaction.Settings[name]).GetKeyDown())
                    {
                        if (i < _character.Items.Count)
                            _character.UseItem(i);
                    }
                }
            }
        }

        protected virtual void UpdateMovementInput(bool inMenu)
        {
            if (inMenu)
            {
                _character.HasDirection = false;
                return;
            }
            int forward = 0;
            int right = 0;
            if (_generalInput.Forward.GetKey())
                forward = 1;
            else if (_generalInput.Back.GetKey())
                forward = -1;
            if (_generalInput.Left.GetKey())
                right = -1;
            else if (_generalInput.Right.GetKey())
                right = 1;
            if (forward != 0 || right != 0)
            {
                _character.TargetAngle = SceneLoader.CurrentCamera.Cache.Transform.rotation.eulerAngles.y + 90f - Mathf.Atan2(forward, right) * Mathf.Rad2Deg;
                _character.HasDirection = true;
            }
            else
                _character.HasDirection = false;
        }

        protected void UpdateMenuInput(bool inMenu)
        {
            if (ChatManager.IsChatActive() || CustomLogicManager.Cutscene || _character.Dead)
            {
                _inGameMenu.EmoteHandler.SetEmoteWheel(false);
                _inGameMenu.ItemHandler.SetItemWheel(false);
                return;
            }
            if (_interactionInput.EmoteMenu.GetKeyDown())
                _inGameMenu.EmoteHandler.ToggleEmoteWheel();
            if (_interactionInput.ItemMenu.GetKeyDown())
                _inGameMenu.ItemHandler.ToggleItemWheel();
            if (_interactionInput.MenuNext.GetKeyDown())
                _inGameMenu.EmoteHandler.NextEmoteWheel();
        }

        protected virtual void UpdateUI(bool inMenu)
        {
        }

        protected float GetTargetAngle(int forward, int right)
        {
            return SceneLoader.CurrentCamera.Cache.Transform.rotation.eulerAngles.y + 90f - Mathf.Atan2(forward, right) * Mathf.Rad2Deg;
        }
    }
}
