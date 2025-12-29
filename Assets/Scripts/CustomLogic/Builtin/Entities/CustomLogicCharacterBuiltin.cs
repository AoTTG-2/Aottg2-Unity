using Characters;
using System;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Character is the base class that `Human`, `Titan`, and `Shifter` inherit from.
    /// Only character owner can modify properties and call functions unless otherwise specified.
    /// </summary>
    /// <code>
    /// function OnCharacterSpawn(character)
    /// {
    ///     if (character.IsMine)
    ///     {
    ///         # Character is owned (network-wise) by the person running this script.
    ///         # Ex: If user is host, this could either be their actual player character or AI titans/shifters.
    ///     }
    ///     
    ///     if (character.IsMainCharacter)
    ///     {
    ///         # Character is the main character (the camera-followed player).
    ///     }
    ///     
    ///     if (character.IsAI)
    ///     {
    ///         # Character is AI and likely controlled via MasterClient.
    ///         
    ///         if (character.Player.ID == Network.MasterClient.ID)
    ///         {
    ///             # Character is owned by masterclient, if we're not masterclient, we cannot modify props.    
    ///         }
    ///     }
    /// }
    /// </code>
    [CLType(Name = "Character", Abstract = true)]
    abstract partial class CustomLogicCharacterBuiltin : BuiltinClassInstance, ICustomLogicEquals
    {
        public readonly BaseCharacter Character;

        protected CustomLogicCharacterBuiltin(BaseCharacter character)
        {
            Character = character;
            Variables["IsCharacter"] = true;
        }

        [CLProperty(Description = "Character's name.")]
        public string Name
        {
            get => Character.Name;
            set
            {
                Character.Name = value;
            }
        }

        [CLProperty(Description = "Character's guild.")]
        public string Guild
        {
            get => Character.Guild;
            set
            {
                Character.Guild = value;
            }
        }

        [CLProperty(Description = "Player who owns this character.")]
        public CustomLogicPlayerBuiltin Player => new CustomLogicPlayerBuiltin(Character.Cache.PhotonView.Owner);

        [CLProperty(Description = "Is this character AI?")]
        public bool IsAI => Character.AI;

        [CLProperty(Description = "Network view ID of the character.")]
        public int ViewID => Character.Cache.PhotonView.ViewID;

        [CLProperty(Description = "Is this character mine?")]
        public bool IsMine => Character.IsMine();

        [CLProperty(Description = "Character belongs to my player and is the main character (the camera-followed player).")]
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
                if (!Character.IsMine()) return;
                Character.SetKinematic(false, 1f);
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

        [CLProperty(Description = "Character's rigidbody component (if available).")]
        public CustomLogicRigidbodyBuiltin Rigidbody => new CustomLogicRigidbodyBuiltin(this, Character.Cache.Rigidbody);

        /// <summary>
        /// Kills the character. Callable by non-owners.
        /// </summary>
        /// <param name="killer">Killer name</param>
        [CLMethod]
        public void GetKilled(string killer) => Character.GetKilled(killer);

        /// <summary>
        /// Damages the character and kills it if its health reaches 0. Callable by non-owners.
        /// </summary>
        /// <param name="killer">Killer name</param>
        /// <param name="damage">Damage amount</param>
        [CLMethod]
        public void GetDamaged(string killer, int damage) => Character.GetDamaged(killer, damage);

        /// <summary>
        /// Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.
        /// </summary>
        [CLMethod]
        public void Emote(string emote)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.Emote(emote);
        }

        /// <summary>
        /// Causes the character to play an animation.
        /// </summary>
        /// <param name="animation">Name of the animation.
        /// Available animations can be found here: [Human](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Human/HumanAnimations.cs), [Titan](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Titan/BasicTitanAnimations.cs), [Annie](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Shifters/Annie/AnnieAnimations.cs), [Eren](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Shifters/Eren/ErenAnimations.cs)
        /// 
        /// Use the right-hand string value for the animation.
        /// 
        /// Note that shifters also have all titan animations.
        /// </param>
        /// <param name="fade">Fade time. If provided, will crossfade the animation by this timestep</param>
        [CLMethod]
        public void PlayAnimation(string animation, float fade = 0.1f)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.CrossFadeIfNotPlaying(animation, fade);
        }

        [CLMethod(Description = "Causes the character to play an animation at a specific time.")]
        public void PlayAnimationAt(string animation, float t, float fade = 0.1f, bool force = false)
        {
            if (Character.IsMine() && !Character.Dead)
            {
                if (force)
                    Character.CrossFade(animation, fade, t);
                else
                    Character.CrossFadeIfNotPlaying(animation, fade, t);
            }
        }

        [CLMethod(Description = "Gets the animation speed of a given animation.")]
        public void GetAnimationSpeed(string animation)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.GetAnimationSpeed(animation);
        }

        [CLMethod(Description = "Sets the animation speed of a given animation.")]
        public void SetAnimationSpeed(string animation, float speed, bool synced = true)
        {
            if (Character.IsMine() && !Character.Dead)
            {
                if (synced)
                {
                    Character.SetAnimationSpeed(animation, speed);
                }
                else
                {
                    Character.SetAnimationSpeedNonRPC(animation, speed);
                }

            }
        }

        //[CLMethod(Description = "Causes the character to pause their animation.")]
        //public void PauseAnimations()
        //{
        //    if (Character.IsMine() && !Character.Dead)
        //        Character.PauseAnimations();
        //}

        //[CLMethod(Description = "Causes the character to continue their animation.")]
        //public void ContinueAnimations()
        //{
        //    if (Character.IsMine() && !Character.Dead)
        //        Character.ContinueAnimations();
        //}

        [CLMethod(Description = "Returns true if the animation is playing.")]
        public bool IsPlayingAnimation(string animation)
        {
            return Character.Animation.IsPlaying(animation);
        }

        [CLMethod(Description = "Returns true if the animation is playing.")]
        public float GetAnimationNormalizedTime(string animation)
        {
            if (!Character.Animation.IsPlaying(animation))
                return 1f;
            return Character.Animation.GetCurrentNormalizedTime();
        }

        /// <summary>
        /// Forces the character to play an animation.
        /// </summary>
        /// <param name="animation">Name of the animation.
        /// Available animations can be found here: [Human](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Human/HumanAnimations.cs), [Titan](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Titan/BasicTitanAnimations.cs), [Annie](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Shifters/Annie/AnnieAnimations.cs), [Eren](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/d631c1648d1432de6f95f07c2f158ff710cdd76d/Assets/Scripts/Characters/Shifters/Eren/ErenAnimations.cs)
        /// 
        /// Use the right-hand string value for the animation.
        /// 
        /// Note that shifters also have all titan animations.
        /// </param>
        /// <param name="fade">Fade time. If provided, will crossfade the animation by this timestep</param>
        [CLMethod]
        public void ForceAnimation(string animation, float fade = 0.1f)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.ForceAnimation(animation, fade);
        }

        [CLMethod(Description = "Gets the length of animation.")]
        public float GetAnimationLength(string animation)
        {
            return Character.Animation.GetLength(animation);
        }

        [CLMethod(Description = "Returns true if the character is playing a sound. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.")]
        public bool IsPlayingSound(string sound)
        {
            return Character.IsPlayingSound(sound);
        }

        /// <summary>
        /// Plays a sound if present in the character.
        /// </summary>
        /// <param name="sound">Name of the sound to play. 
        /// Available sound names can be found here: [Human](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/refs/heads/main/Assets/Scripts/Characters/Human/HumanSounds.cs), [Shifters](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/refs/heads/main/Assets/Scripts/Characters/Shifters/ShifterSounds.cs), [Titans](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/refs/heads/main/Assets/Scripts/Characters/Titan/TitanSounds.cs).
        /// 
        /// Note that shifters also have all titan sounds</param>
        [CLMethod]
        public void PlaySound(string sound)
        {
            if (Character.IsMine() && !Character.Dead && !Character.IsPlayingSound(sound))
                Character.PlaySound(sound);
        }

        /// <summary>
        /// Stops a sound if present in the character.
        /// </summary>
        /// <param name="sound">Name of the sound to stop.</param>
        [CLMethod]
        public void StopSound(string sound)
        {
            if (Character.IsMine() && !Character.Dead && Character.IsPlayingSound(sound))
                Character.StopSound(sound);
        }

        [CLMethod(Description = "Fades the sound volume to a specific volume between 0.0 and 1.0 over [time] seconds. Does not play or stop the sound.")]
        public void FadeSound(string sound, float volume, float time)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.FadeSound(sound, volume, time);
        }

        [CLMethod(Description = "Rotates the character such that it is looking towards a world position.")]
        public void LookAt(CustomLogicVector3Builtin position)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.Cache.Transform.LookAt(position.Value);
        }

        /// <summary>
        /// Adds a force to the character with given force vector and optional mode.
        /// </summary>
        /// <param name="force">Force vector</param>
        /// <param name="mode">Force mode. Valid modes are Force, Acceleration, Impulse, VelocityChange</param>
        [CLMethod]
        public void AddForce(CustomLogicVector3Builtin force, string mode = "Acceleration")
        {
            if (!Character.IsMine()) return;
            Character.SetKinematic(false, 1f);
            var useForceMode = Enum.TryParse(mode, out ForceMode forceMode) ? forceMode : ForceMode.Acceleration;
            Character.Cache.Rigidbody.AddForce(force.Value, useForceMode);
        }

        [CLMethod(description: "Reveal the titan for a set number of seconds.")]
        public void Reveal(float delay)
        {
            Character.Reveal(0, delay);
        }

        /// <summary>
        /// Adds an outline effect with the given color and mode.
        /// </summary>
        /// <param name="color">Outline color</param>
        /// <param name="mode">Outline mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor</param>
        [CLMethod]
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
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return Character == null;
            if (!(other is CustomLogicCharacterBuiltin))
                return false;
            var otherCharacter = ((CustomLogicCharacterBuiltin)other).Character;
            if (Character == null)
                return otherCharacter == null;
            return Character == otherCharacter;
        }

        [CLMethod]
        public bool __Eq__(object self, object other)
        {
            return self.Equals(other);
        }

        [CLMethod]
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
