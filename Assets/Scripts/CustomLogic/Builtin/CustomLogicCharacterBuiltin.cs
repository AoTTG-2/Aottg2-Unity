using Characters;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CustomLogic
{
    abstract class CustomLogicCharacterBuiltin: CustomLogicBaseBuiltin
    {
        public BaseCharacter Character;
        public CustomLogicCharacterBuiltin(BaseCharacter character, string type = "Character"): base(type)
        {
            Character = character;
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "GetKilled")
            {
                string killer = (string)parameters[0];
                Character.GetKilled(killer);
                return null;
            }
            if (name == "GetDamaged")
            {
                string killer = (string)parameters[0];
                int damage = parameters[1].UnboxToInt();
                Character.GetDamaged(killer, damage);
                return null;
            }
            if (name == "Emote")
            {
                string emote = (string)parameters[0];
                if (Character.IsMine() && !Character.Dead)
                    Character.Emote(emote);
                return null;
            }
            if (name == "PlayAnimation")
            {
                string anim = (string)parameters[0];
                float fade = 0.1f;
                if (parameters.Count > 1)
                    fade = (float)parameters[1];
                if (Character.IsMine() && !Character.Dead)
                    Character.CrossFadeIfNotPlaying(anim, fade);
                return null;
            }
            if (name == "GetAnimationLength")
            {
                string anim = (string)parameters[0];
                return Character.Cache.Animation[anim].length;
            }
            if (name == "PlaySound")
            {
                string sound = (string)parameters[0];
                if (Character.IsMine() && !Character.Dead && !Character.IsPlayingSound(sound))
                    Character.PlaySound(sound);
                return null;
            }
            if (name == "StopSound")
            {
                string sound = (string)parameters[0];
                if (Character.IsMine() && !Character.Dead && Character.IsPlayingSound(sound))
                    Character.StopSound(sound);
                return null;
            }
            if (name == "LookAt")
            {
                Vector3 position = ((CustomLogicVector3Builtin)parameters[0]).Value;
                if (Character.IsMine() && !Character.Dead)
                    Character.Cache.Transform.LookAt(position);
                return null;
            }
            if (name == "AddForce")
            {
                Vector3 force = ((CustomLogicVector3Builtin)parameters[0]).Value;
                string forceMode = "Acceleration";
                if (parameters.Count > 1)
                {
                    forceMode = (string)parameters[1];
                }
                ForceMode mode = ForceMode.Acceleration;
                switch (forceMode)
                {
                    case "Force":
                        mode = ForceMode.Force;
                        break;
                    case "Acceleration":
                        mode = ForceMode.Acceleration;
                        break;
                    case "Impulse":
                        mode = ForceMode.Impulse;
                        break;
                    case "VelocityChange":
                        mode = ForceMode.VelocityChange;
                        break;
                }   
                Character.Cache.Rigidbody.AddForce(force, mode);
                return null;
            }
            if (name == "Reveal")
            {
                Character.Reveal(0, parameters[0].UnboxToFloat());
                return null;
            }
            if (name == "AddOutline")
            {
                Color color = Color.white;
                Outline.Mode mode = Outline.Mode.OutlineAll;
                if (parameters.Count > 0)
                    color = ((CustomLogicColorBuiltin)parameters[0]).Value.ToColor();
                if (parameters.Count > 1)
                    mode = (Outline.Mode)Enum.Parse(typeof(Outline.Mode), (string)parameters[1]);

                Character.AddOutlineWithColor(color, mode);
                return null;
            }
            if (name == "RemoveOutline")
            {
                Character.RemoveOutline();
                return null;
            }
            return base.CallMethod(name, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "Player")
                return new CustomLogicPlayerBuiltin(Character.Cache.PhotonView.Owner);
            if (name == "ViewID")
                return Character.Cache.PhotonView.ViewID;
            if (name == "IsMine")
                return Character.IsMine();
            if (name == "Name")
                return Character.Name;
            if (name == "IsMainCharacter")
                return Character.IsMainCharacter();
            if (name == "Position")
                return new CustomLogicVector3Builtin(Character.Cache.Transform.position);
            if (name == "Rotation")
                return new CustomLogicVector3Builtin(Character.Cache.Transform.rotation.eulerAngles);
            if (name == "Velocity")
                return new CustomLogicVector3Builtin(Character.Cache.Rigidbody.velocity);
            if (name == "Forward")
                return new CustomLogicVector3Builtin(Character.Cache.Transform.forward);
            if (name == "Right")
                return new CustomLogicVector3Builtin(Character.Cache.Transform.right);
            if (name == "Up")
                return new CustomLogicVector3Builtin(Character.Cache.Transform.up);
            if (name == "HasTargetDirection")
                return Character.HasDirection;
            if (name == "TargetDirection")
                return new CustomLogicVector3Builtin(Character.GetTargetDirection());
            if (name == "Team")
                return Character.Team;
            if (name == "IsCharacter")
                return true;
            if (name == "Health")
                return Character.CurrentHealth;
            if (name == "MaxHealth")
                return Character.MaxHealth;
            if (name == "CustomDamageEnabled")
                return Character.CustomDamageEnabled;
            if (name == "CustomDamage")
                return Character.CustomDamage;
            if (name == "CurrentAnimation")
                return Character.GetCurrentAnimation();
            if (name == "IsAI")
                return Character.AI;
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (!Character.IsMine())
                return;
            if (name == "Position")
                Character.Cache.Transform.position = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Rotation")
                Character.Cache.Transform.rotation = Quaternion.Euler(((CustomLogicVector3Builtin)value).Value);
            else if (name == "Velocity")
                Character.Cache.Rigidbody.velocity = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Forward")
                Character.Cache.Transform.forward = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Right")
                Character.Cache.Transform.right = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Up")
                Character.Cache.Transform.up = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Health")
                Character.SetCurrentHealth(value.UnboxToInt());
            else if (name == "MaxHealth")
                Character.SetMaxHealth(value.UnboxToInt());
            else if (name == "Team")
                Character.SetTeam((string)value);
            else if (name == "CustomDamageEnabled")
                Character.CustomDamageEnabled = (bool)value;
            else if (name == "CustomDamage")
                Character.CustomDamage = value.UnboxToInt();
            else
                base.SetField(name, value);
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return Character == null;
            if (!(other is CustomLogicCharacterBuiltin))
                return false;
            return Character == ((CustomLogicCharacterBuiltin)other).Character;
        }
    }
}
