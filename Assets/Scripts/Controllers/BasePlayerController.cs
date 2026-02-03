using System;
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
        protected bool _autorun;
        public bool HideCursor;
        protected int _lastRightInput;
        protected int _lastForwardInput;

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

        private string[] _quickSelectOptions = new string[8] { "QuickSelect1", "QuickSelect2", "QuickSelect3", "QuickSelect4", "QuickSelect5", "QuickSelect6", "QuickSelect7", "QuickSelect8" };
        protected virtual void UpdateActionInput(bool inMenu)
        {
            if (!inMenu && !_character.Dead)
            {
                for (int i = 0; i < _quickSelectOptions.Length; i++)
                {
                    string name = _quickSelectOptions[i];
                    if (((KeybindSetting)SettingsManager.InputSettings.Interaction.Settings[name]).GetKeyDown())
                    {
                        if (i < _character.Items.Count)
                            _character.UseItem(i);
                    }
                }
                if (SettingsManager.InputSettings.General.HideCursor.GetKeyDown())
                    HideCursor = !HideCursor;
            }
        }

        protected virtual void UpdateMovementInput(bool inMenu)
        {
            if (inMenu)
            {
                if (!_autorun)
                    _character.HasDirection = false;
                return;
            }
            int forward = 0;
            int right = 0;
            if (_generalInput.Autorun.GetKeyDown())
                _autorun = !_autorun;

            bool isRightHeld = _generalInput.Right.GetKey();
            bool isLeftHeld = _generalInput.Left.GetKey();
            bool isForwardHeld = _generalInput.Forward.GetKey();
            bool isBackHeld = _generalInput.Back.GetKey();

            if (_generalInput.Forward.GetKeyDown())
                _lastForwardInput = 1;
            else if (_generalInput.Back.GetKeyDown())
                _lastForwardInput = -1;

            if (_generalInput.Left.GetKeyDown())
                _lastRightInput = -1;
            else if (_generalInput.Right.GetKeyDown())
                _lastRightInput = 1;

            if (isRightHeld && isLeftHeld)
                right = _lastRightInput;
            else if (isLeftHeld)
                right = -1;
            else if (isRightHeld)
                right = 1;
            
            if (isForwardHeld &&  isBackHeld)
                forward = _lastForwardInput;
            else if (isForwardHeld)
                forward = 1;
            else if (isBackHeld)
                forward = -1;
            if (forward != 0 || right != 0)
                _autorun = false;
            if (_autorun)
            {
                forward = 1;
                right = 0;
            }
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

        protected float[] GetAimAngles()
        {
            float angleX = 0f;
            float angleY = 0f;
            Vector3 aim = SceneLoader.CurrentCamera.Camera.ScreenPointToRay(CursorManager.GetInGameMousePosition()).direction.normalized * 1000f;
            angleY = Mathf.Asin(aim.y / aim.magnitude) * Mathf.Rad2Deg;
            aim.y = 0f;
            angleX = -Mathf.Atan2(aim.z, aim.x) * Mathf.Rad2Deg;
            angleX = -Mathf.DeltaAngle(angleX, _character.Cache.Transform.eulerAngles.y - 90f);
            return new float[] { angleX, angleY };
        }

        protected float GetTargetAngle(Vector3 direction)
        {
            direction = new Vector3(direction.x, 0f, direction.z).normalized;
            return Quaternion.LookRotation(direction, Vector3.up).eulerAngles.y;
        }
    }
}
