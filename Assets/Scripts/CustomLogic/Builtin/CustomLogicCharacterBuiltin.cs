using Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomLogic
{
    // todo: all property setters should check IsMine()
    [CLType(Name = "Character", Abstract = true)]
    abstract partial class CustomLogicCharacterBuiltin : BuiltinClassInstance, ICustomLogicEquals
    {
        public readonly BaseCharacter Character;

        protected CustomLogicCharacterBuiltin(BaseCharacter character)
        {
            Character = character;
            Variables["IsCharacter"] = true;
        }

        [CLProperty(Description = "Player who owns this character.")]
        public CustomLogicPlayerBuiltin Player => new CustomLogicPlayerBuiltin(Character.Cache.PhotonView.Owner);
        // public override object CallMethod(string name, List<object> parameters)
        // {
        //     if (name == "GetKilled")
        //     {
        //         string killer = (string)parameters[0];
        //         Character.GetKilled(killer);
        //         return null;
        //     }
        //     if (name == "GetDamaged")
        //     {
        //         string killer = (string)parameters[0];
        //         int damage = parameters[1].UnboxToInt();
        //         Character.GetDamaged(killer, damage);
        //         return null;
        //     }
        //     if (name == "Emote")
        //     {
        //         string emote = (string)parameters[0];
        //         if (Character.IsMine() && !Character.Dead)
        //             Character.Emote(emote);
        //         return null;
        //     }
        //     if (name == "PlayAnimation")
        //     {
        //         string anim = (string)parameters[0];
        //         float fade = 0.1f;
        //         if (parameters.Count > 1)
        //             fade = (float)parameters[1];
        //         if (Character.IsMine() && !Character.Dead)
        //             Character.CrossFadeIfNotPlaying(anim, fade);
        //         return null;
        //     }
        //     if (name == "GetAnimationLength")
        //     {
        //         string anim = (string)parameters[0];
        //         return Character.Animation.GetLength(anim);
        //     }
        //     if (name == "PlaySound")
        //     {
        //         string sound = (string)parameters[0];
        //         if (Character.IsMine() && !Character.Dead && !Character.IsPlayingSound(sound))
        //             Character.PlaySound(sound);
        //         return null;
        //     }
        //     if (name == "StopSound")
        //     {
        //         string sound = (string)parameters[0];
        //         if (Character.IsMine() && !Character.Dead && Character.IsPlayingSound(sound))
        //             Character.StopSound(sound);
        //         return null;
        //     }
        //     if (name == "LookAt")
        //     {
        //         Vector3 position = ((CustomLogicVector3Builtin)parameters[0]).Value;
        //         if (Character.IsMine() && !Character.Dead)
        //             Character.Cache.Transform.LookAt(position);
        //         return null;
        //     }
        //     if (name == "AddForce")
        //     {
        //         Vector3 force = ((CustomLogicVector3Builtin)parameters[0]).Value;
        //         string forceMode = "Acceleration";
        //         if (parameters.Count > 1)
        //         {
        //             forceMode = (string)parameters[1];
        //         }
        //         ForceMode mode = ForceMode.Acceleration;
        //         switch (forceMode)
        //         {
        //             case "Force":
        //                 mode = ForceMode.Force;
        //                 break;
        //             case "Acceleration":
        //                 mode = ForceMode.Acceleration;
        //                 break;
        //             case "Impulse":
        //                 mode = ForceMode.Impulse;
        //                 break;
        //             case "VelocityChange":
        //                 mode = ForceMode.VelocityChange;
        //                 break;
        //         }
        //         if (Character.IsMine())
        //         {
        //             Character.SetKinematic(false, 1f);
        //             Character.Cache.Rigidbody.AddForce(force, mode);
        //         }
        //         return null;
        //     }
        //     if (name == "Reveal")
        //     {
        //         Character.Reveal(0, parameters[0].UnboxToFloat());
        //         return null;
        //     }
        //     if (name == "AddOutline")
        //     {
        //         Color color = Color.white;
        //         Outline.Mode mode = Outline.Mode.OutlineAll;
        //         if (parameters.Count > 0)
        //             color = ((CustomLogicColorBuiltin)parameters[0]).Value.ToColor();
        //         if (parameters.Count > 1)
        //             mode = (Outline.Mode)Enum.Parse(typeof(Outline.Mode), (string)parameters[1]);

        [CLProperty(Description = "Is this character AI?")]
        public bool IsAI => Character.AI;

        [CLProperty(Description = "Network view ID of the character.")]
        public int ViewID => Character.Cache.PhotonView.ViewID;

        [CLProperty(Description = "Is this character mine?")]
        public bool IsMine => Character.IsMine();

        [CLProperty]
        public bool IsMainCharacter => Character.IsMainCharacter();

        [CLProperty(Description = "Unity transform of the character.")]
        public CustomLogicTransformBuiltin Transform => new CustomLogicTransformBuiltin(Character.Cache.Transform);

        [CLProperty(Description = "Position of the character.")]
        public CustomLogicVector3Builtin Position
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.position);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.position = value.Value;
            }
        }

        [CLProperty(Description = "Rotation of the character.")]
        public CustomLogicVector3Builtin Rotation
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.rotation.eulerAngles);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.rotation = Quaternion.Euler(value.Value);
            }

        }

        [CLProperty(Description = "Quaternion rotation of the character.")]
        public CustomLogicQuaternionBuiltin QuaternionRotation
        {
            get => new CustomLogicQuaternionBuiltin(Character.Cache.Transform.rotation);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.rotation = value.Value;
            }
        }

        [CLProperty(Description = "Velocity of the character.")]
        public CustomLogicVector3Builtin Velocity
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Rigidbody.velocity);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Rigidbody.velocity = value.Value;
            }
        }

        [CLProperty(Description = "Forward direction of the character.")]
        public CustomLogicVector3Builtin Forward
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.forward);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.forward = value.Value;
            }
        }

        [CLProperty(Description = "Right direction of the character.")]
        public CustomLogicVector3Builtin Right
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.right);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.right = value.Value;
            }
        }

        [CLProperty(Description = "Up direction of the character.")]
        public CustomLogicVector3Builtin Up
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.up);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.up = value.Value;
            }
        }

        [CLProperty(Description = "If the character has a target direction it is turning towards.")]
        public bool HasTargetDirection => Character.HasDirection;

        [CLProperty(Description = "The character's target direction.")]
        public CustomLogicVector3Builtin TargetDirection => new CustomLogicVector3Builtin(Character.GetTargetDirection());

        [CLProperty(Description = "Team character belongs to.")]
        public string Team
        {
            get => Character.Team;
            set
            {
                if (Character.IsMine())
                    Character.SetTeam(value);
            }
        }

        [CLProperty(Description = "The display name of the character.")]
        public string Name
        {
            get => Character.Name;
            set
            {
                Character.Name = value;
            }
        }

        [CLProperty(Description = "The guild name of the character.")]
        public string Guild
        {
            get => Character.Guild;
            set
            {
                Character.Guild = value;
            }
        }

        [CLProperty(Description = "Character's current health.")]
        public float Health
        {
            get => Character.CurrentHealth;
            set
            {
                if (Character.IsMine())
                    Character.SetCurrentHealth((int)value);
            }
        }

        [CLProperty(Description = "Character's maximum health.")]
        public float MaxHealth
        {
            get => Character.MaxHealth;
            set
            {
                if (Character.IsMine())
                    Character.SetMaxHealth((int)value);
            }
        }

        [CLProperty(Description = "Is custom damage dealing enabled.")]
        public bool CustomDamageEnabled
        {
            get => Character.CustomDamageEnabled;
            set
            {
                if (Character.IsMine())
                    Character.CustomDamageEnabled = value;
            }
        }

        [CLProperty(Description = "Amount of custom damage to deal per attack.")]
        public int CustomDamage
        {
            get => Character.CustomDamage;
            set
            {
                if (Character.IsMine())
                    Character.CustomDamage = value;
            }
        }

        [CLProperty(Description = "Character's current playing animation.")]
        public string CurrentAnimation => Character.GetCurrentAnimation();

        [CLProperty(Description = "Character's grounded status.")]
        public bool Grounded => Character.Grounded;

        [CLMethod(Description = "Kills the character. Callable by non-owners.")]
        public void GetKilled(string killer) => Character.GetKilled(killer);

        [CLMethod(Description = "Damages the character and kills it if its health reaches 0. Callable by non-owners.")]
        public void GetDamaged(string killer, int damage) => Character.GetDamaged(killer, damage);

        [CLMethod(Description = "Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.")]
        public void Emote(string emote)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.Emote(emote);
        }

        [CLMethod(Description = "Causes the character to play an animation.  If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.")]
        public void PlayAnimation(string animation, float fade = 0.1f)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.CrossFadeIfNotPlaying(animation, fade);
        }

        [CLMethod(Description = "Gets the length of animation.")]
        public float GetAnimationLength(string animation)
        {
            return Character.Cache.Animation[animation].length;
        }

        [CLMethod(Description = "Plays a sound if present in the character. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.")]
        public void PlaySound(string sound)
        {
            if (Character.IsMine() && !Character.Dead && !Character.IsPlayingSound(sound))
                Character.PlaySound(sound);
        }

        [CLMethod(Description = "Stops the sound.")]
        public void StopSound(string sound)
        {
            if (Character.IsMine() && !Character.Dead && Character.IsPlayingSound(sound))
                Character.StopSound(sound);
        }

        [CLMethod(Description = "Rotates the character such that it is looking towards a world position.")]
        public void LookAt(CustomLogicVector3Builtin position)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.Cache.Transform.LookAt(position.Value);
        }

        [CLMethod(Description = "Adds a force to the character with given force vector and optional mode. Valid modes are Force, Acceleration, Impulse, VelocityChange with default being Acceleration.")]
        public void AddForce(CustomLogicVector3Builtin force, string mode = "Acceleration")
        {
            // parse mode as Forcemode enum
            var useForceMode = Enum.TryParse(mode, out ForceMode forceMode) ? forceMode : ForceMode.Acceleration;
            Character.Cache.Rigidbody.AddForce(force.Value, useForceMode);
        }

        [CLMethod(description: "Reveaal the titan for a set number of seconds.")]
        public void Reveal(float delay)
        {
            Character.Reveal(0, delay);
        }

        [CLMethod(Description = "Adds an outline effect with the given color and mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor")]
        public void AddOutline(CustomLogicColorBuiltin color = null, string mode = "OutlineAll")
        {
            Color outlineColor = Color.white;
            if (color != null)
                outlineColor = color.Value.ToColor();

            Outline.Mode outlineMode = (Outline.Mode)Enum.Parse(typeof(Outline.Mode), mode);

            Character.AddOutlineWithColor(outlineColor, outlineMode);
        }

        [CLMethod(Description = "Removes the outline effect from the character.")]
        public void RemoveOutline()
        {
            Character.RemoveOutline();
            // if (name == "Name")
            //     Character.Name = (string)value;
            // else if (name == "Guild")
            //     Character.Guild = (string)value;
            // if (!Character.IsMine())
            //     return;
            // if (name == "Position")
            //     Character.Cache.Transform.position = ((CustomLogicVector3Builtin)value).Value;
            // else if (name == "Rotation")
            //     Character.Cache.Transform.rotation = Quaternion.Euler(((CustomLogicVector3Builtin)value).Value);
            // else if (name == "QuaternionRotation")
            //     Character.Cache.Transform.rotation = ((CustomLogicQuaternionBuiltin)value).Value;
            // else if (name == "Velocity")
            // {
            //     Character.SetKinematic(false, 1f);
            //     Character.Cache.Rigidbody.velocity = ((CustomLogicVector3Builtin)value).Value;
            // }
            // else if (name == "Forward")
            //     Character.Cache.Transform.forward = ((CustomLogicVector3Builtin)value).Value;
            // else if (name == "Right")
            //     Character.Cache.Transform.right = ((CustomLogicVector3Builtin)value).Value;
            // else if (name == "Up")
            //     Character.Cache.Transform.up = ((CustomLogicVector3Builtin)value).Value;
            // else if (name == "Health")
            //     Character.SetCurrentHealth(value.UnboxToInt());
            // else if (name == "MaxHealth")
            //     Character.SetMaxHealth(value.UnboxToInt());
            // else if (name == "Team")
            //     Character.SetTeam((string)value);
            // else if (name == "CustomDamageEnabled")
            //     Character.CustomDamageEnabled = (bool)value;
            // else if (name == "CustomDamage")
            //     Character.CustomDamage = value.UnboxToInt();
            // else if (name == "Name")
            //     Character.Name = (string)value;
            // else if (name == "Guild")
            //     Character.Guild = (string)value;
            // else
            //     base.SetField(name, value);
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return Character == null;
            if (!(other is CustomLogicCharacterBuiltin))
                return false;
            return Character == ((CustomLogicCharacterBuiltin)other).Character;
        }



        public bool __Eq__(object self, object other)
        {
            return self.Equals(other);
        }

        public int __Hash__()
        {

            return GetHashCode();
        }
        public override int GetHashCode()
        {
            // TODO: Implement a better hash code
            // May want to override this to view id or something else for better player comparisons instead of unity player object comparison
            // ex: dict[player] = 1; player.die(); dict[OnPlayerSpawn player] = KEY ERROR.
            return Character.GetHashCode();
        }
    }
}
