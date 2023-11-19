using System.Collections.Generic;
using UnityEngine;
using Map;

namespace CustomLogic
{
    class CustomLogicTransformBuiltin: CustomLogicBaseBuiltin
    {
        public Transform Value;
        private Vector3 _internalRotation;
        private Vector3 _internalLocalRotation;
        private bool _needSetRotation = true;
        private bool _needSetLocalRotation = true;

        public CustomLogicTransformBuiltin(Transform transform): base("Transform")
        {
            Value = transform;
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (methodName == "GetTransform")
            {
                string name = (string)parameters[0];
                Transform transform = Value.Find(name);
                if (transform != null)
                {
                    return new CustomLogicTransformBuiltin(transform);
                }
            }
            else if (methodName == "PlayAnimation")
            {
                string anim = (string)parameters[0];
                var animation = Value.GetComponent<Animation>();
                float fade = 0.1f;
                if (parameters.Count > 1)
                    fade = (float)parameters[1];
                if (!animation.IsPlaying(anim))
                    animation.CrossFade(anim, fade);
            }
            else if (methodName == "GetAnimationLength")
            {
                string anim = (string)parameters[0];
                var animation = Value.GetComponent<Animation>();
                return animation[anim].length;
            }
            else if (methodName == "PlaySound")
            {
                var sound = Value.GetComponent<AudioSource>();
                if (!sound.isPlaying)
                    sound.Play();
            }
            else if (methodName == "StopSound")
            {
                var sound = Value.GetComponent<AudioSource>();
                if (sound.isPlaying)
                    sound.Stop();
            }
            else if (methodName == "ToggleParticle")
            {
                var particle = Value.GetComponent<ParticleSystem>();
                if (!particle.isPlaying)
                    particle.Play();
                particle.enableEmission = (bool)parameters[0];
            }
            return base.CallMethod(methodName, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "Position")
                return new CustomLogicVector3Builtin(Value.position);
            if (name == "LocalPosition")
                return new CustomLogicVector3Builtin(Value.localPosition);
            if (name == "Rotation")
            {
                if (_needSetRotation)
                {
                    _internalRotation = Value.rotation.eulerAngles;
                    _needSetRotation = false;
                }
                return new CustomLogicVector3Builtin(_internalRotation);
            }
            if (name == "LocalRotation")
            {
                if (_needSetLocalRotation)
                {
                    _internalLocalRotation = Value.localRotation.eulerAngles;
                    _needSetLocalRotation = false;
                }
                return new CustomLogicVector3Builtin(_internalLocalRotation);
            }
            if (name == "Forward")
                return new CustomLogicVector3Builtin(Value.forward.normalized);
            if (name == "Up")
                return new CustomLogicVector3Builtin(Value.up.normalized);
            if (name == "Right")
                return new CustomLogicVector3Builtin(Value.right.normalized);
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (name == "Position")
                Value.position = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "LocalPosition")
                Value.localPosition = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Rotation")
            {
                _internalRotation = ((CustomLogicVector3Builtin)value).Value;
                _needSetRotation = false;
                Value.rotation = Quaternion.Euler(_internalRotation);
            }
            else if (name == "LocalRotation")
            {
                _internalLocalRotation = ((CustomLogicVector3Builtin)value).Value;
                _needSetLocalRotation = false;
                Value.localRotation = Quaternion.Euler(_internalLocalRotation);
            }
            else if (name == "Forward")
                Value.forward = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Up")
                Value.up = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Right")
                Value.right = ((CustomLogicVector3Builtin)value).Value;
            else
                base.SetField(name, value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return Value == null;
            if (!(obj is CustomLogicTransformBuiltin))
                return false;
            return Value == ((CustomLogicTransformBuiltin)obj).Value;
        }
    }
}
