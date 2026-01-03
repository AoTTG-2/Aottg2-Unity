using System;
using ApplicationManagers;
using Cameras;
using Settings;

namespace CustomLogic
{
    [CLType(Name = "Camera", Abstract = true, Static = true, Description = "References the main game camera.")]
    partial class CustomLogicCameraBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCameraBuiltin(){}

        public static InGameCamera CurrentCamera => (InGameCamera)SceneLoader.CurrentCamera;

        [CLProperty(Description = "Is camera in manual mode.")]
        public static bool IsManual => CustomLogicManager.ManualCamera;

        [CLProperty(Description = "Position of the camera.")]
        public static CustomLogicVector3Builtin Position => new CustomLogicVector3Builtin(CurrentCamera.Cache.Transform.position);

        [CLProperty(Description = "Rotation of the camera.")]
        public static CustomLogicVector3Builtin Rotation => new CustomLogicVector3Builtin(CurrentCamera.Cache.Transform.rotation.eulerAngles);

        [CLProperty(Description = "Velocity of the camera.")]
        public static CustomLogicVector3Builtin Velocity => new CustomLogicVector3Builtin(CustomLogicManager.CameraVelocity);

        [CLProperty(Description = "Field of view of the camera.")]
        public static float FOV => CustomLogicManager.CameraFOV;

        [CLProperty(Description = "Current camera mode. TPS, Original, FPS.")]
        public static string CameraMode => (CustomLogicManager.CameraMode ?? CurrentCamera.CurrentCameraMode).ToString();

        [CLProperty(Description = "Forward vector of the camera.")]
        public static CustomLogicVector3Builtin Forward
        {
            get => new CustomLogicVector3Builtin(CurrentCamera.Cache.Transform.forward);
            set
            {
                CurrentCamera.Cache.Transform.forward = value.Value;
                CustomLogicManager.CameraRotation = CurrentCamera.Cache.Transform.rotation.eulerAngles;
            }
        }

        [CLProperty(Description = "Right vector of the camera.")]
        public static CustomLogicVector3Builtin Right
        {
            get => new CustomLogicVector3Builtin(CurrentCamera.Cache.Transform.right);
            set
            {
                CurrentCamera.Cache.Transform.right = value.Value;
                CustomLogicManager.CameraRotation = CurrentCamera.Cache.Transform.rotation.eulerAngles;
            }
        }

        [CLProperty(Description = "Up vector of the camera.")]
        public static CustomLogicVector3Builtin Up
        {
            get => new CustomLogicVector3Builtin(CurrentCamera.Cache.Transform.up);
            set
            {
                CurrentCamera.Cache.Transform.up = value.Value;
                CustomLogicManager.CameraRotation = CurrentCamera.Cache.Transform.rotation.eulerAngles;
            }
        }

        [CLProperty(Description = "Distance from the camera to the character.")]
        public static float FollowDistance
        {
            get => CurrentCamera.GetCameraDistance();
            set => CurrentCamera.SetCameraDistance(value);
        }

        [CLMethod("Sets the camera manual mode. If true, camera will only be controlled by custom logic. If false, camera will follow the spawned or spectated player and read input.")]
        public static void SetManual(
            [CLParam("True to enable manual mode, false to disable.")]
            bool manual)
        {
            CustomLogicManager.ManualCamera = manual;
        }

        [CLMethod("Sets camera position.")]
        public static void SetPosition(
            [CLParam("The world position to set the camera to.")]
            CustomLogicVector3Builtin position)
        {
            CustomLogicManager.CameraPosition = position.Value;
            ((InGameCamera)SceneLoader.CurrentCamera).SyncCustomPosition();
        }

        [CLMethod("Sets camera rotation.")]
        public static void SetRotation(
            [CLParam("The euler angles rotation to set the camera to.")]
            CustomLogicVector3Builtin rotation)
        {
            CustomLogicManager.CameraRotation = rotation.Value;
            ((InGameCamera)SceneLoader.CurrentCamera).SyncCustomPosition();
        }

        [CLMethod("Sets camera velocity.")]
        public static void SetVelocity(
            [CLParam("The velocity vector to set for the camera.")]
            CustomLogicVector3Builtin velocity)
        {
            CustomLogicManager.CameraVelocity = velocity.Value;
        }

        [CLMethod("Sets the camera forward direction such that it is looking at a world position.")]
        public static void LookAt(
            [CLParam("The world position to look at.")]
            CustomLogicVector3Builtin position)
        {
            CurrentCamera.Cache.Transform.LookAt(position.Value);
            CustomLogicManager.CameraRotation = CurrentCamera.Cache.Transform.rotation.eulerAngles;
        }

        [CLMethod("Sets the camera field of view.")]
        public static void SetFOV(
            [CLParam("The new field of view. Use 0 to use the default field of view.")]
            float fov)
        {
            CustomLogicManager.CameraFOV = fov;
        }

        [CLMethod("Forces the player to use a certain camera mode, taking priority over their camera setting.")]
        public static void SetCameraMode(
            [CLParam("The camera mode. Accepted values are TPS, Original, FPS.")]
            string mode)
        {
            if (mode == "null")
                CustomLogicManager.CameraMode = null;
            else
                CustomLogicManager.CameraMode = Enum.Parse<CameraInputMode>(mode);
        }

        [CLMethod("Resets the follow distance to player's settings.")]
        public static void ResetDistance() => CurrentCamera.ResetDistance();

        [CLMethod("Resets the camera mode to player's settings.")]
        public static void ResetCameraMode()
        {
            CustomLogicManager.CameraMode = null;
            CurrentCamera.ResetCameraMode();
        }

        [CLMethod("Locks or unlocks the camera to prevent or allow camera movement.")]
        public static void SetCameraLocked(
            [CLParam("If true, locks the camera to prevent movement.")]
            bool locked)
        {
            CustomLogicManager.CameraLocked = locked;
        }

        [CLMethod("Sets the visibility of the cursor.")]
        public static void SetCursorVisible(
            [CLParam("If true, makes the cursor visible.")]
            bool visible)
        {
            CustomLogicManager.CursorVisible = visible;
        }
    }
}
