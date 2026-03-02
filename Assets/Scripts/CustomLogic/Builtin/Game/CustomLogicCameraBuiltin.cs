using System;
using ApplicationManagers;
using Cameras;
using Settings;

namespace CustomLogic
{
    /// <summary>
    /// References the main game camera.
    /// </summary>
    [CLType(Name = "Camera", Abstract = true, Static = true)]
    partial class CustomLogicCameraBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCameraBuiltin(){}

        public static InGameCamera CurrentCamera => (InGameCamera)SceneLoader.CurrentCamera;

        /// <summary>
        /// Is camera in manual mode.
        /// </summary>
        [CLProperty]
        public static bool IsManual => CustomLogicManager.ManualCamera;

        /// <summary>
        /// Position of the camera.
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Position => new CustomLogicVector3Builtin(CurrentCamera.Cache.Transform.position);

        /// <summary>
        /// Rotation of the camera.
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Rotation => new CustomLogicVector3Builtin(CurrentCamera.Cache.Transform.rotation.eulerAngles);

        /// <summary>
        /// Velocity of the camera.
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Velocity => new CustomLogicVector3Builtin(CustomLogicManager.CameraVelocity);

        /// <summary>
        /// Field of view of the camera.
        /// </summary>
        [CLProperty]
        public static float FOV => CustomLogicManager.CameraFOV;

        /// <summary>
        /// Current camera mode. TPS, Original, FPS.
        /// </summary>
        [CLProperty]
        public static string CameraMode => (CustomLogicManager.CameraMode ?? CurrentCamera.CurrentCameraMode).ToString();

        /// <summary>
        /// Forward vector of the camera.
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Forward
        {
            get => new CustomLogicVector3Builtin(CurrentCamera.Cache.Transform.forward);
            set
            {
                CurrentCamera.Cache.Transform.forward = value.Value;
                CustomLogicManager.CameraRotation = CurrentCamera.Cache.Transform.rotation.eulerAngles;
            }
        }

        /// <summary>
        /// Right vector of the camera.
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Right
        {
            get => new CustomLogicVector3Builtin(CurrentCamera.Cache.Transform.right);
            set
            {
                CurrentCamera.Cache.Transform.right = value.Value;
                CustomLogicManager.CameraRotation = CurrentCamera.Cache.Transform.rotation.eulerAngles;
            }
        }

        /// <summary>
        /// Up vector of the camera.
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Up
        {
            get => new CustomLogicVector3Builtin(CurrentCamera.Cache.Transform.up);
            set
            {
                CurrentCamera.Cache.Transform.up = value.Value;
                CustomLogicManager.CameraRotation = CurrentCamera.Cache.Transform.rotation.eulerAngles;
            }
        }

        /// <summary>
        /// Distance from the camera to the character.
        /// </summary>
        [CLProperty]
        public static float FollowDistance
        {
            get => CurrentCamera.GetCameraDistance();
            set => CurrentCamera.SetCameraDistance(value);
        }

        /// <summary>
        /// Sets the camera manual mode. If true, camera will only be controlled by custom logic.
        /// If false, camera will follow the spawned or spectated player and read input.
        /// </summary>
        /// <param name="manual">True to enable manual mode, false to disable.</param>
        [CLMethod]
        public static void SetManual(bool manual)
        {
            CustomLogicManager.ManualCamera = manual;
        }

        /// <summary>
        /// Sets camera position.
        /// </summary>
        /// <param name="position">The world position to set the camera to.</param>
        [CLMethod]
        public static void SetPosition(CustomLogicVector3Builtin position)
        {
            CustomLogicManager.CameraPosition = position.Value;
            ((InGameCamera)SceneLoader.CurrentCamera).SyncCustomPosition();
        }

        /// <summary>
        /// Sets camera rotation.
        /// </summary>
        /// <param name="rotation">The euler angles rotation to set the camera to.</param>
        [CLMethod]
        public static void SetRotation(CustomLogicVector3Builtin rotation)
        {
            CustomLogicManager.CameraRotation = rotation.Value;
            ((InGameCamera)SceneLoader.CurrentCamera).SyncCustomPosition();
        }

        /// <summary>
        /// Sets camera velocity.
        /// </summary>
        /// <param name="velocity">The velocity vector to set for the camera.</param>
        [CLMethod]
        public static void SetVelocity(CustomLogicVector3Builtin velocity)
        {
            CustomLogicManager.CameraVelocity = velocity.Value;
        }

        /// <summary>
        /// Sets the camera forward direction such that it is looking at a world position.
        /// </summary>
        /// <param name="position">The world position to look at.</param>
        [CLMethod]
        public static void LookAt(CustomLogicVector3Builtin position)
        {
            CurrentCamera.Cache.Transform.LookAt(position.Value);
            CustomLogicManager.CameraRotation = CurrentCamera.Cache.Transform.rotation.eulerAngles;
        }

        /// <summary>
        /// Sets the camera field of view.
        /// </summary>
        /// <param name="fov">The new field of view. Use 0 to use the default field of view.</param>
        [CLMethod]
        public static void SetFOV(float fov)
        {
            CustomLogicManager.CameraFOV = fov;
        }

        /// <summary>
        /// Forces the player to use a certain camera mode, taking priority over their camera setting.
        /// </summary>
        /// <param name="mode">The camera mode. Accepted values are TPS, Original, FPS.</param>
        /// TODO: Migrate from string to int on the next update when CL developers will migrate to the new enum.
        [CLMethod]
        public static void SetCameraMode([CLParam(Enum = new Type[] { typeof(CustomLogicCameraModeEnum) })] string mode)
        {
            if (mode == "null")
                CustomLogicManager.CameraMode = null;
            else
                CustomLogicManager.CameraMode = Enum.Parse<CameraInputMode>(mode);
        }

        /// <summary>
        /// Resets the follow distance to player's settings.
        /// </summary>
        [CLMethod]
        public static void ResetDistance() => CurrentCamera.ResetDistance();

        /// <summary>
        /// Resets the camera mode to player's settings.
        /// </summary>
        [CLMethod]
        public static void ResetCameraMode()
        {
            CustomLogicManager.CameraMode = null;
            CurrentCamera.ResetCameraMode();
        }

        /// <summary>
        /// Locks or unlocks the camera to prevent or allow camera movement.
        /// </summary>
        /// <param name="locked">If true, locks the camera to prevent movement.</param>
        [CLMethod]
        public static void SetCameraLocked(bool locked)
        {
            CustomLogicManager.CameraLocked = locked;
        }

        /// <summary>
        /// Sets the visibility of the cursor.
        /// </summary>
        /// <param name="visible">If true, makes the cursor visible.</param>
        [CLMethod]
        public static void SetCursorVisible(bool visible)
        {
            CustomLogicManager.CursorVisible = visible;
        }
    }
}
