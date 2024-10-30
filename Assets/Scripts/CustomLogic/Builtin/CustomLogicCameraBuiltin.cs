using ApplicationManagers;
using Cameras;
using Characters;
using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicCameraBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicCameraBuiltin(): base("Camera")
        {
        }

        public override object GetField(string name)
        {
            var camera = (InGameCamera)SceneLoader.CurrentCamera;
            if (name == "IsManual")
                return CustomLogicManager.ManualCamera;
            if (name == "Position")
                return new CustomLogicVector3Builtin(camera.Cache.Transform.position);
            if (name == "Rotation")
                return new CustomLogicVector3Builtin(camera.Cache.Transform.rotation.eulerAngles);
            if (name == "Velocity")
                return new CustomLogicVector3Builtin(CustomLogicManager.CameraVelocity);
            if (name == "FOV")
                return CustomLogicManager.CameraFOV;
            if (name == "CameraMode")
                return (CustomLogicManager.CameraMode ?? camera.CurrentCameraMode).ToString();
            if (name == "Forward")
                return new CustomLogicVector3Builtin(camera.Cache.Transform.forward);
            if (name == "FollowDistance")
                return camera.GetCameraDistance();
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            var camera = (InGameCamera)SceneLoader.CurrentCamera;
            if (name == "Forward")
            {
                var vectorBuiltin = (CustomLogicVector3Builtin)value;
                camera.Cache.Transform.forward = vectorBuiltin.Value;
                CustomLogicManager.CameraRotation = camera.Cache.Transform.rotation.eulerAngles;
            }
            else if (name == "FollowDistance")
            {
                camera.SetCameraDistance(value.UnboxToFloat());
            }
            else
                base.SetField(name, value);
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            var camera = (InGameCamera)SceneLoader.CurrentCamera;
            if (name == "SetManual")
            {
                if (parameters.Count > 0)
                    CustomLogicManager.ManualCamera = (bool)parameters[0];
                else
                    CustomLogicManager.ManualCamera = true;
                return null;
            }
            if (name == "SetPosition")
            {
                var vectorBuiltin = (CustomLogicVector3Builtin)parameters[0];
                CustomLogicManager.CameraPosition = vectorBuiltin.Value;
                camera.SyncCustomPosition();
                return null;
            }
            if (name == "SetRotation")
            {
                var vectorBuiltin = (CustomLogicVector3Builtin)parameters[0];
                CustomLogicManager.CameraRotation = vectorBuiltin.Value;
                camera.SyncCustomPosition();
                return null;
            }
            if (name == "SetVelocity")
            {
                var vectorBuiltin = (CustomLogicVector3Builtin)parameters[0];
                CustomLogicManager.CameraVelocity = vectorBuiltin.Value;
                return null;
            }
            if (name == "LookAt")
            {
                var vectorBuiltin = (CustomLogicVector3Builtin)parameters[0];
                camera.Cache.Transform.LookAt(vectorBuiltin.Value);
                CustomLogicManager.CameraRotation = camera.Cache.Transform.rotation.eulerAngles;
                return null;
            }
            if (name == "SetFOV")
            {
                CustomLogicManager.CameraFOV = parameters[0].UnboxToFloat();
                return null;
            }
            if (name == "SetCameraMode")
            {
                if (parameters[0] is string str)
                {
                    if (str == "null")
                        CustomLogicManager.CameraMode = null;
                    else
                        CustomLogicManager.CameraMode = Enum.Parse<CameraInputMode>(str);
                }
                else
                    CustomLogicManager.CameraMode = null;

                return null;
            }
            if (name == "ResetDistance")
            {
                camera.ResetDistance();
                return null;
            }
            if (name == "ResetCameraMode")
            {
                CustomLogicManager.CameraMode = null;
                camera.ResetCameraMode();
                return null;
            }
            return base.CallMethod(name, parameters);
        }
    }
}
