using Characters;
using System;
using UnityEngine;

namespace CustomLogic
{
    // todo: all property setters should check IsMine()
    [CLType(Abstract = true)]
    abstract class CustomLogicCharacterBuiltin: CustomLogicClassInstanceBuiltin, ICustomLogicEquals
    {
        public readonly BaseCharacter Character;
        
        protected CustomLogicCharacterBuiltin(BaseCharacter character, string type = "Character"): base(type)
        {
            Character = character;
            Variables["IsCharacter"] = true;
        }

        [CLProperty(Description = "Player who owns this character.")]
        public CustomLogicPlayerBuiltin Player => new CustomLogicPlayerBuiltin(Character.Cache.PhotonView.Owner);

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
            set {
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
                if (Character.IsMine())
                    Character.Name = value;
            }
        }

        [CLProperty(Description = "The guild name of the character.")]
        public string Guild
        {
            get => Character.Guild;
            set
            {
                if (Character.IsMine())
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
        public void AddOutline(CustomLogicColorBuiltin color, string mode)
        {
            Outline.Mode outlineMode = (Outline.Mode)Enum.Parse(typeof(Outline.Mode), mode);
            Character.AddOutlineWithColor(color.Value.ToColor(), outlineMode);
        }

        [CLMethod(Description = "Removes the outline effect from the character.")]
        public void RemoveOutline()
        {
            Character.RemoveOutline();
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return Character == null;
            if (!(other is CustomLogicCharacterBuiltin))
                return false;
            return Character == ((CustomLogicCharacterBuiltin)other).Character;
        }



        public bool __Eq__(object other)
        {
            return Equals(other);
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
