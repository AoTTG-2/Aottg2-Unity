using UnityEngine;
using Utility;
using System.Collections.Generic;
using System.IO;
using Settings;
using System.Linq;
using SimpleJSONFixed;
using UnityEngine.UI;
using System.Collections;
using ApplicationManagers;
using System;
using Events;
using GameManagers;
using Cameras;
using Characters;
using CustomLogic;
using Controllers;

namespace UI
{
    class CursorManager : MonoBehaviour
    {
        public static CursorState State;
        private static CursorManager _instance;
        // private static Texture2D _cursorPointer;
        private static Dictionary<CrosshairStyle, Texture2D> _crosshairs = new Dictionary<CrosshairStyle, Texture2D>();
        private bool _ready;
        private bool _crosshairWhite = true;
        private bool _lastCrosshairWhite = false;
        private string _crosshairText = string.Empty;
        private bool _forceNextCrosshairUpdate = false;
        private CrosshairStyle _lastCrosshairStyle = CrosshairStyle.Default;
        private Vector3 _arrowLeftPosition;
        private Vector3 _arrowRightPosition;
        private Quaternion _arrowLeftRotation;
        private Quaternion _arrowRightRotation;
        private bool _arrowLeftWhite;
        private bool _arrowRightWhite;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            foreach (CrosshairStyle style in Enum.GetValues(typeof(CrosshairStyle)))
            {
                Texture2D crosshair = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Cursors/Cursor" + style.ToString());
                _crosshairs.Add(style, crosshair);
            }
            _instance._ready = true;
        }

        private void Update()
        {
            if (SceneLoader.SceneName == SceneName.Startup || SceneLoader.SceneName == SceneName.MainMenu || 
                SceneLoader.SceneName == SceneName.CharacterEditor || SceneLoader.SceneName == SceneName.SnapshotViewer || 
                SceneLoader.SceneName == SceneName.Gallery || SceneLoader.SceneName == SceneName.Credits)
                SetPointer();
            else
            {
                if (UIManager.CurrentMenu == null || !(UIManager.CurrentMenu is InGameMenu))
                    return;
                var manager = (InGameManager)SceneLoader.CurrentGameManager;
                if (InGameMenu.InMenu() || !manager.IsFinishedLoading() || manager.GlobalPause || manager.Restarting)
                    SetPointer();
                else if (manager.CurrentCharacter != null && (manager.CurrentCharacter is Human || manager.CurrentCharacter is BasicTitan) && !manager.CurrentCharacter.Dead && !CustomLogicManager.Cutscene)
                {
                    var controller = manager.CurrentCharacter.GetComponent<BasePlayerController>();
                    if (controller != null && !controller.HideCursor)
                        SetCrosshair();
                    else
                        SetHidden();
                }
                else
                    SetHidden();
            }
        }

        public static void RefreshCursorLock()
        {
            return;
        }

        public static void SetPointer(bool force = false)
        {
            if (force || State != CursorState.Pointer)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                State = CursorState.Pointer;
            }
        }

        public static void SetHidden(bool force = false)
        {
            if (force || State != CursorState.Hidden)
            {
                Cursor.visible = false;
                State = CursorState.Hidden;
            }
            if (((InGameCamera)SceneLoader.CurrentCamera).CurrentCameraMode == CameraInputMode.TPS)
            {
                if (Cursor.lockState != CursorLockMode.Locked)
                    Cursor.lockState = CursorLockMode.Locked;
            }
            else if (Cursor.lockState != CursorLockMode.Confined)
                Cursor.lockState = CursorLockMode.Confined;
        }

        public static void SetCrosshair(bool force = false)
        {
            if (force || (State != CursorState.Crosshair))
            {
                Cursor.visible = false;
                State = CursorState.Crosshair;
            }
            if (((InGameCamera)SceneLoader.CurrentCamera).CurrentCameraMode == CameraInputMode.TPS)
            {
                if (Cursor.lockState != CursorLockMode.Locked)
                    Cursor.lockState = CursorLockMode.Locked;
            }
            else if (Cursor.lockState != CursorLockMode.Confined)
                Cursor.lockState = CursorLockMode.Confined;
        }

        public static void SetCrosshairColor(bool white)
        {
            if (_instance._crosshairWhite != white)
            {
                _instance._crosshairWhite = white;
            }
        }

        public static void SetCrosshairText(string text)
        {
            _instance._crosshairText = text;
        }

        public static void SetHookArrow(bool left, Vector3 position, Quaternion rotation, bool white)
        {
            if (left)
            {
                _instance._arrowLeftPosition = position;
                _instance._arrowLeftWhite = white;
                _instance._arrowLeftRotation = rotation;
            }
            else
            {
                _instance._arrowRightPosition = position;
                _instance._arrowRightWhite = white;
                _instance._arrowRightRotation = rotation;
            }
        }

        public static void UpdateHookArrows(Image hookArrowLeft, Image hookArrowRight)
        {
            if (_instance._ready)
            {
                if (State != CursorState.Crosshair)
                {
                    if (hookArrowLeft.gameObject.activeSelf)
                        hookArrowLeft.gameObject.SetActive(false);
                    if (hookArrowRight.gameObject.activeSelf)
                        hookArrowRight.gameObject.SetActive(false);
                    return;
                }
                var manager = (InGameManager)SceneLoader.CurrentGameManager;
                if (SettingsManager.UISettings.ShowCrosshairArrows.Value && manager.CurrentCharacter != null && manager.CurrentCharacter is Human)
                {
                    if (!hookArrowLeft.gameObject.activeSelf)
                        hookArrowLeft.gameObject.SetActive(true);
                    if (!hookArrowRight.gameObject.activeSelf)
                        hookArrowRight.gameObject.SetActive(true);
                    hookArrowLeft.transform.position = _instance._arrowLeftPosition;
                    hookArrowRight.transform.position = _instance._arrowRightPosition;
                    hookArrowLeft.transform.rotation = _instance._arrowLeftRotation;
                    hookArrowRight.transform.rotation = _instance._arrowRightRotation;
                    hookArrowLeft.color = _instance._arrowLeftWhite ? Color.white : Color.red;
                    hookArrowRight.color = _instance._arrowRightWhite ? Color.white : Color.red;
                }
                else
                {
                    if (hookArrowLeft.gameObject.activeSelf)
                        hookArrowLeft.gameObject.SetActive(false);
                    if (hookArrowRight.gameObject.activeSelf)
                        hookArrowRight.gameObject.SetActive(false);
                }
            }
        }

        public static void UpdateCrosshair(RawImage crosshairImageWhite, RawImage crosshairImageRed, Text crosshairLabelWhite,
            Text crosshairLabelRed, bool force = false)
        {
            if (_instance._ready)
            {
                if (State != CursorState.Crosshair)
                {
                    if (crosshairImageRed.gameObject.activeSelf)
                        crosshairImageRed.gameObject.SetActive(false);
                    if (crosshairImageWhite.gameObject.activeSelf)
                        crosshairImageWhite.gameObject.SetActive(false);
                    _instance._forceNextCrosshairUpdate = true;
                    return;
                }
                bool skin = SettingsManager.UISettings.CrosshairSkin.Value != "";
                if (!skin)
                {
                    CrosshairStyle style = (CrosshairStyle)SettingsManager.UISettings.CrosshairStyle.Value;
                    if (_instance._lastCrosshairStyle != style || force || _instance._forceNextCrosshairUpdate)
                    {
                        crosshairImageWhite.texture = _crosshairs[style];
                        crosshairImageRed.texture = _crosshairs[style];
                        _instance._lastCrosshairStyle = style;
                    }
                }
                if (_instance._crosshairWhite != _instance._lastCrosshairWhite || force || _instance._forceNextCrosshairUpdate)
                {
                    crosshairImageWhite.gameObject.SetActive(_instance._crosshairWhite);
                    crosshairImageRed.gameObject.SetActive(!_instance._crosshairWhite);
                    _instance._lastCrosshairWhite = _instance._crosshairWhite;
                }
                Text crosshairLabel = crosshairLabelWhite;
                RawImage crosshairImage = crosshairImageWhite;
                if (!_instance._crosshairWhite)
                {
                    crosshairLabel = crosshairLabelRed;
                    crosshairImage = crosshairImageRed;
                }
                crosshairLabel.text = _instance._crosshairText;
                Vector3 mousePosition = Input.mousePosition;
                Transform crosshairTransform = crosshairImage.transform;
                if (crosshairTransform.position != mousePosition)
                {
                    if (((InGameCamera)SceneLoader.CurrentCamera).CurrentCameraMode == CameraInputMode.TPS)
                    {
                        if (Math.Abs(crosshairTransform.position.x - mousePosition.x) > 1f || Math.Abs(crosshairTransform.position.y - mousePosition.y) > 1f)
                        {
                            crosshairTransform.position = mousePosition;
                        }
                    }
                    else
                    {
                        crosshairTransform.position = mousePosition;
                    }
                }
                _instance._forceNextCrosshairUpdate = false;
            }
        }
    }

    public enum CursorState
    {
        Pointer,
        Crosshair,
        Hidden
    }

    public enum CrosshairStyle
    {
        Default,
        Square,
        Plus,
        Target,
        Dot
    }
}
