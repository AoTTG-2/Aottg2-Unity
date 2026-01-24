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

        /// <summary>
        /// Character's name.
        /// </summary>
        [CLProperty]
        public string Name
        {
            get => Character.Name;
            set
            {
                Character.Name = value;
            }
        }

        /// <summary>
        /// Character's guild.
        /// </summary>
        [CLProperty]
        public string Guild
        {
            get => Character.Guild;
            set
            {
                Character.Guild = value;
            }
        }

        /// <summary>
        /// Player who owns this character.
        /// </summary>
        [CLProperty]
        public CustomLogicPlayerBuiltin Player => new CustomLogicPlayerBuiltin(Character.Cache.PhotonView.Owner);

        /// <summary>
        /// Is this character AI?
        /// </summary>
        [CLProperty]
        public bool IsAI => Character.AI;

        /// <summary>
        /// Is this character alive?
        /// Value is set to false before despawn.
        /// </summary>
        [CLProperty]
        public bool IsAlive => Character != null ? Character.Dead : false;

        /// <summary>
        /// Network view ID of the character.
        /// </summary>
        [CLProperty]
        public int ViewID => Character.Cache.PhotonView.ViewID;

        /// <summary>
        /// Is this character mine?
        /// </summary>
        [CLProperty]
        public bool IsMine => Character.IsMine();

        /// <summary>
        /// Character belongs to my player and is the main character (the camera-followed player).
        /// </summary>
        [CLProperty]
        public bool IsMainCharacter => Character.IsMainCharacter();

        /// <summary>
        /// Unity transform of the character.
        /// </summary>
        [CLProperty]
        public CustomLogicTransformBuiltin Transform => new CustomLogicTransformBuiltin(Character.Cache.Transform);

        /// <summary>
        /// Position of the character.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Position
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.position);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.position = value.Value;
            }
        }

        /// <summary>
        /// Rotation of the character.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Rotation
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.rotation.eulerAngles);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.rotation = Quaternion.Euler(value.Value);
            }

        }

        /// <summary>
        /// Quaternion rotation of the character.
        /// </summary>
        [CLProperty]
        public CustomLogicQuaternionBuiltin QuaternionRotation
        {
            get => new CustomLogicQuaternionBuiltin(Character.Cache.Transform.rotation);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.rotation = value.Value;
            }
        }

        /// <summary>
        /// Velocity of the character.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Forward direction of the character.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Forward
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.forward);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.forward = value.Value;
            }
        }

        /// <summary>
        /// Right direction of the character.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Right
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.right);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.right = value.Value;
            }
        }

        /// <summary>
        /// Up direction of the character.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Up
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.up);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.up = value.Value;
            }
        }

        /// <summary>
        /// If the character has a target direction it is turning towards.
        /// </summary>
        [CLProperty]
        public bool HasTargetDirection => Character.HasDirection;

        /// <summary>
        /// The character's target direction.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin TargetDirection => new CustomLogicVector3Builtin(Character.GetTargetDirection());

        /// <summary>
        /// Team character belongs to. Using enum is not mandatory, value can be any string.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicTeamEnum) })]
        public string Team
        {
            get => Character.Team;
            set
            {
                if (Character.IsMine())
                    Character.SetTeam(value);
            }
        }

        /// <summary>
        /// Character's current health.
        /// </summary>
        [CLProperty]
        public float Health
        {
            get => Character.CurrentHealth;
            set
            {
                if (Character.IsMine())
                    Character.SetCurrentHealth((int)value);
            }
        }

        /// <summary>
        /// Character's maximum health.
        /// </summary>
        [CLProperty]
        public float MaxHealth
        {
            get => Character.MaxHealth;
            set
            {
                if (Character.IsMine())
                    Character.SetMaxHealth((int)value);
            }
        }

        /// <summary>
        /// Is custom damage dealing enabled.
        /// </summary>
        [CLProperty]
        public bool CustomDamageEnabled
        {
            get => Character.CustomDamageEnabled;
            set
            {
                if (Character.IsMine())
                    Character.CustomDamageEnabled = value;
            }
        }

        /// <summary>
        /// Amount of custom damage to deal per attack.
        /// </summary>
        [CLProperty]
        public int CustomDamage
        {
            get => Character.CustomDamage;
            set
            {
                if (Character.IsMine())
                    Character.CustomDamage = value;
            }
        }

        /// <summary>
        /// Character's current playing animation.
        /// </summary>
        [CLProperty]
        public string CurrentAnimation => Character.GetCurrentAnimation();

        /// <summary>
        /// Character's grounded status.
        /// </summary>
        [CLProperty]
        public bool Grounded => Character.Grounded;

        /// <summary>
        /// Character's rigidbody component (if available).
        /// </summary>
        [CLProperty]
        public CustomLogicRigidbodyBuiltin Rigidbody => new CustomLogicRigidbodyBuiltin(this, Character.Cache.Rigidbody);

        /// <summary>
        /// Kills the character.
        /// </summary>
        /// <param name="killer">Killer name.</param>
        [CLMethod]
        public void GetKilled(string killer) => Character.GetKilled(killer);

        /// <summary>
        /// Damages the character.
        /// </summary>
        /// <param name="killer">Killer name.</param>
        /// <param name="damage">Damage amount.</param>
        [CLMethod]
        public void GetDamaged(string killer, int damage) => Character.GetDamaged(killer, damage);

        /// <summary>
        /// Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.
        /// </summary>
        /// <param name="emote">Name of the emote to play.</param>
        [CLMethod]
        public void Emote(string emote)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.Emote(emote);
        }

        /// <summary>
        /// Causes the character to play an animation. Available animations can be found here: Human, Titan, Annie, Eren.
        /// Use the right-hand string value for the animation. Note that shifters also have all titan animations.
        /// </summary>
        /// <param name="animation">Name of the animation.</param>
        /// <param name="fade">Fade time. If provided, will crossfade the animation by this timestep.</param>
        [CLMethod]
        public void PlayAnimation([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string animation, float fade = 0.1f)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.CrossFadeIfNotPlaying(animation, fade);
        }

        /// <summary>
        /// Causes the character to play an animation at a specific time.
        /// </summary>
        /// <param name="animation">Name of the animation.</param>
        /// <param name="t">Time in the animation to start playing.</param>
        /// <param name="fade">Fade time.</param>
        /// <param name="force">Whether to force the animation even if it's already playing.</param>
        [CLMethod]
        public void PlayAnimationAt([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string animation, float t, float fade = 0.1f, bool force = false)
        {
            if (Character.IsMine() && !Character.Dead)
            {
                if (force)
                    Character.CrossFade(animation, fade, t);
                else
                    Character.CrossFadeIfNotPlaying(animation, fade, t);
            }
        }

        /// <summary>
        /// Gets the animation speed of a given animation.
        /// </summary>
        /// <param name="animation">Name of the animation.</param>
        /// <returns>1.0 if the character is not owned by the player or is dead, otherwise the animation speed.</returns>
        [CLMethod]
        public float GetAnimationSpeed([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string animation)
        {
            if (Character.IsMine() && !Character.Dead)
                return Character.GetAnimationSpeed(animation);
            return 1.0f;
        }

        /// <summary>
        /// Sets the animation speed of a given animation.
        /// </summary>
        /// <param name="animation">Name of the animation.</param>
        /// <param name="speed">The animation speed multiplier.</param>
        /// <param name="synced">Whether to sync the speed across the network.</param>
        [CLMethod]
        public void SetAnimationSpeed([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string animation, float speed, bool synced = true)
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

        /// <summary>
        /// Returns true if the animation is playing.
        /// </summary>
        /// <param name="animation">Name of the animation.</param>
        /// <returns>True if the animation is playing, false otherwise.</returns>
        [CLMethod]
        public bool IsPlayingAnimation([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string animation)
        {
            return Character.Animation.IsPlaying(animation);
        }

        /// <summary>
        /// Gets the normalized time of the currently playing animation.
        /// </summary>
        /// <param name="animation">Name of the animation.</param>
        /// <returns>The normalized time (0-1) of the animation.</returns>
        [CLMethod]
        public float GetAnimationNormalizedTime([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string animation)
        {
            if (!Character.Animation.IsPlaying(animation))
                return 1f;
            return Character.Animation.GetCurrentNormalizedTime();
        }

        /// <summary>
        /// Forces the character to play an animation, even if it's already playing.
        /// Available animations can be found here: Human, Titan, Annie, Eren.
        /// Use the right-hand string value for the animation. Note that shifters also have all titan animations.
        /// </summary>
        /// <param name="animation">Name of the animation.</param>
        /// <param name="fade">Fade time. If provided, will crossfade the animation by this timestep.</param>
        [CLMethod]
        public void ForceAnimation([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string animation, float fade = 0.1f)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.ForceAnimation(animation, fade);
        }

        /// <summary>
        /// Gets the length of animation.
        /// </summary>
        /// <param name="animation">Name of the animation.</param>
        /// <returns>The length of the animation in seconds.</returns>
        [CLMethod]
        public float GetAnimationLength([CLParam(Enum = new Type[] { typeof(CustomLogicHumanAnimationEnum), typeof(CustomLogicTitanAnimationEnum), typeof(CustomLogicAnnieAnimationEnum), typeof(CustomLogicErenAnimationEnum), typeof(CustomLogicWallColossalAnimationEnum), typeof(CustomLogicDummyAnimationEnum), typeof(CustomLogicHorseAnimationEnum) })] string animation)
        {
            return Character.Animation.GetLength(animation);
        }

        /// <summary>
        /// Returns true if the character is playing a sound.
        /// Available sound names can be found here: Humans, Shifters, Titans.
        /// Note that shifters also have all titan sounds.
        /// </summary>
        /// <param name="sound">Name of the sound.</param>
        /// <returns>True if the sound is playing, false otherwise.</returns>
        [CLMethod]
        public bool IsPlayingSound([CLParam(Enum = new Type[] { typeof(CustomLogicHumanSoundEnum), typeof(CustomLogicTitanSoundEnum), typeof(CustomLogicShifterSoundEnum) })] string sound)
        {
            return Character.IsPlayingSound(sound);
        }

        /// <summary>
        /// Plays a sound for the character.
        /// Available sound names can be found here: Human, Shifters, Titans.
        /// Note that shifters also have all titan sounds.
        /// </summary>
        /// <param name="sound">Name of the sound to play.</param>
        [CLMethod]
        public void PlaySound([CLParam(Enum = new Type[] { typeof(CustomLogicHumanSoundEnum), typeof(CustomLogicTitanSoundEnum), typeof(CustomLogicShifterSoundEnum) })] string sound)
        {
            if (Character.IsMine() && !Character.Dead && !Character.IsPlayingSound(sound))
                Character.PlaySound(sound);
        }

        /// <summary>
        /// Stops a sound for the character.
        /// </summary>
        /// <param name="sound">Name of the sound to stop.</param>
        [CLMethod]
        public void StopSound([CLParam(Enum = new Type[] { typeof(CustomLogicHumanSoundEnum), typeof(CustomLogicTitanSoundEnum), typeof(CustomLogicShifterSoundEnum) })] string sound)
        {
            if (Character.IsMine() && !Character.Dead && Character.IsPlayingSound(sound))
                Character.StopSound(sound);
        }

        /// <summary>
        /// Fades the sound volume to a specific volume between 0.0 and 1.0 over [time] seconds.
        /// Does not play or stop the sound.
        /// </summary>
        /// <param name="sound">Name of the sound.</param>
        /// <param name="volume">Target volume (0.0 to 1.0).</param>
        /// <param name="time">Time in seconds to fade over.</param>
        [CLMethod]
        public void FadeSound(
            [CLParam(Enum = new Type[] { typeof(CustomLogicHumanSoundEnum), typeof(CustomLogicTitanSoundEnum), typeof(CustomLogicShifterSoundEnum) })]
            string sound, 
            float volume, 
            float time)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.FadeSound(sound, volume, time);
        }

        /// <summary>
        /// Rotates the character such that it is looking towards a world position.
        /// </summary>
        /// <param name="position">The world position to look at.</param>
        [CLMethod]
        public void LookAt(CustomLogicVector3Builtin position)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.Cache.Transform.LookAt(position.Value);
        }

        /// <summary>
        /// Adds a force to the character with given force vector and optional mode.
        /// </summary>
        /// <param name="force">Force vector.</param>
        /// <param name="mode">Force mode. Default is Acceleration.</param>
        [CLMethod]
        public void AddForce(
            CustomLogicVector3Builtin force,
            // TODO: Migrate to int on the next update when CL developers will migrate to the new enum.
            [CLParam(Enum = new Type[] { typeof(CustomLogicForceModeEnum) }, Type = "int")] object mode)
        {
            if (!Character.IsMine()) return;

            Character.SetKinematic(false, 1f);
            if (mode is int forceModeInt)
            {
                if (!Enum.IsDefined(typeof(ForceMode), forceModeInt))
                    throw new ArgumentException($"Unknown force mode: {forceModeInt}");
                Character.Cache.Rigidbody.AddForce(force.Value, (ForceMode)forceModeInt);
            }
            else if (mode is string forceModeString)
            {
                if (!Enum.TryParse<ForceMode>(forceModeString, out var forceMode))
                    throw new ArgumentException($"Unknown force mode: {forceModeString}");
                Character.Cache.Rigidbody.AddForce(force.Value, forceMode);
            }
            else
            {
                throw new ArgumentException($"Invalid force mode: {mode}");
            }
        }

        /// <summary>
        /// Reveal the titan for a set number of seconds.
        /// </summary>
        /// <param name="delay">Delay in seconds before revealing.</param>
        [CLMethod]
        public void Reveal(float delay)
        {
            Character.Reveal(0, delay);
        }

        /// <summary>
        /// Adds an outline effect with the given color and mode.
        /// </summary>
        /// <param name="color">Outline color.</param>
        /// <param name="mode">Outline mode. Default is OutlineAll.</param>
        [CLMethod]
        public void AddOutline(
            CustomLogicColorBuiltin color = null,
            [CLParam(Enum = new Type[] { typeof(CustomLogicOutlineModeEnum) })] string mode = "OutlineAll")
        {
            Color outlineColor = Color.white;
            if (color != null)
                outlineColor = color.Value.ToColor();

            // TODO: Migrate from string to int on the next update when CL developers will migrate to the new enum.
            if (!Enum.TryParse<Outline.Mode>(mode, out var outlineMode))
                throw new ArgumentException($"Unknown outline mode: {mode}");

            Character.AddOutlineWithColor(outlineColor, outlineMode);
        }

        /// <summary>
        /// Removes the outline effect from the character.
        /// </summary>
        [CLMethod]
        public void RemoveOutline()
        {
            Character.RemoveOutline();
        }

        public override bool Equals(object other)
        {
            // Delegate to __Eq__ for consistency
            return __Eq__(this, other);
        }

        /// <summary>
        /// Checks if two characters are equal.
        /// </summary>
        /// <param name="self">The first character.</param>
        /// <param name="other">The second character.</param>
        /// <returns>True if the characters are equal, false otherwise.</returns>
        [CLMethod]
        public bool __Eq__(object self, object other)
        {
            // Extract character wrappers
            var selfChar = self as CustomLogicCharacterBuiltin;
            var otherChar = other as CustomLogicCharacterBuiltin;

            // Get underlying Character references (may be null if destroyed)
            var selfCharacter = selfChar?.Character;
            var otherCharacter = otherChar?.Character;

            // Compare underlying Characters
            if (selfCharacter == null && otherCharacter == null)
                return true;
            if (selfCharacter == null || otherCharacter == null)
                return false;

            return selfCharacter == otherCharacter;
        }

        /// <summary>
        /// Gets the hash code of the character.
        /// </summary>
        /// <returns>The hash code.</returns>
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
