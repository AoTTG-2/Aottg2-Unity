using Characters;
using System;
using UnityEngine;

namespace CustomLogic
{
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
    [CLType(Name = "Character", Abstract = true, Description = "Character is the base class that `Human`, `Titan`, and `Shifter` inherit from. Only character owner can modify properties and call functions unless otherwise specified.")]
    abstract partial class CustomLogicCharacterBuiltin : BuiltinClassInstance, ICustomLogicEquals
    {
        public readonly BaseCharacter Character;

        protected CustomLogicCharacterBuiltin(BaseCharacter character)
        {
            Character = character;
            Variables["IsCharacter"] = true;
        }

        [CLProperty("Character's name.")]
        public string Name
        {
            get => Character.Name;
            set
            {
                Character.Name = value;
            }
        }

        [CLProperty("Character's guild.")]
        public string Guild
        {
            get => Character.Guild;
            set
            {
                Character.Guild = value;
            }
        }

        [CLProperty("Player who owns this character.")]
        public CustomLogicPlayerBuiltin Player => new CustomLogicPlayerBuiltin(Character.Cache.PhotonView.Owner);

        [CLProperty("Is this character AI?")]
        public bool IsAI => Character.AI;

        [CLProperty("Network view ID of the character.")]
        public int ViewID => Character.Cache.PhotonView.ViewID;

        [CLProperty("Is this character mine?")]
        public bool IsMine => Character.IsMine();

        [CLProperty("Character belongs to my player and is the main character (the camera-followed player).")]
        public bool IsMainCharacter => Character.IsMainCharacter();

        [CLProperty("Unity transform of the character.")]
        public CustomLogicTransformBuiltin Transform => new CustomLogicTransformBuiltin(Character.Cache.Transform);

        [CLProperty("Position of the character.")]
        public CustomLogicVector3Builtin Position
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.position);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.position = value.Value;
            }
        }

        [CLProperty("Rotation of the character.")]
        public CustomLogicVector3Builtin Rotation
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.rotation.eulerAngles);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.rotation = Quaternion.Euler(value.Value);
            }

        }

        [CLProperty("Quaternion rotation of the character.")]
        public CustomLogicQuaternionBuiltin QuaternionRotation
        {
            get => new CustomLogicQuaternionBuiltin(Character.Cache.Transform.rotation);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.rotation = value.Value;
            }
        }

        [CLProperty("Velocity of the character.")]
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

        [CLProperty("Forward direction of the character.")]
        public CustomLogicVector3Builtin Forward
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.forward);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.forward = value.Value;
            }
        }

        [CLProperty("Right direction of the character.")]
        public CustomLogicVector3Builtin Right
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.right);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.right = value.Value;
            }
        }

        [CLProperty("Up direction of the character.")]
        public CustomLogicVector3Builtin Up
        {
            get => new CustomLogicVector3Builtin(Character.Cache.Transform.up);
            set
            {
                if (Character.IsMine())
                    Character.Cache.Transform.up = value.Value;
            }
        }

        [CLProperty("If the character has a target direction it is turning towards.")]
        public bool HasTargetDirection => Character.HasDirection;

        [CLProperty("The character's target direction.")]
        public CustomLogicVector3Builtin TargetDirection => new CustomLogicVector3Builtin(Character.GetTargetDirection());

        [CLProperty("Team character belongs to.")]
        public string Team
        {
            get => Character.Team;
            set
            {
                if (Character.IsMine())
                    Character.SetTeam(value);
            }
        }

        [CLProperty("Character's current health.")]
        public float Health
        {
            get => Character.CurrentHealth;
            set
            {
                if (Character.IsMine())
                    Character.SetCurrentHealth((int)value);
            }
        }

        [CLProperty("Character's maximum health.")]
        public float MaxHealth
        {
            get => Character.MaxHealth;
            set
            {
                if (Character.IsMine())
                    Character.SetMaxHealth((int)value);
            }
        }

        [CLProperty("Is custom damage dealing enabled.")]
        public bool CustomDamageEnabled
        {
            get => Character.CustomDamageEnabled;
            set
            {
                if (Character.IsMine())
                    Character.CustomDamageEnabled = value;
            }
        }

        [CLProperty("Amount of custom damage to deal per attack.")]
        public int CustomDamage
        {
            get => Character.CustomDamage;
            set
            {
                if (Character.IsMine())
                    Character.CustomDamage = value;
            }
        }

        [CLProperty("Character's current playing animation.")]
        public string CurrentAnimation => Character.GetCurrentAnimation();

        [CLProperty("Character's grounded status.")]
        public bool Grounded => Character.Grounded;

        [CLProperty("Character's rigidbody component (if available).")]
        public CustomLogicRigidbodyBuiltin Rigidbody => new CustomLogicRigidbodyBuiltin(this, Character.Cache.Rigidbody);

        [CLMethod("Kills the character.")]
        public void GetKilled(
            [CLParam("Killer name.")]
            string killer)
            => Character.GetKilled(killer);

        [CLMethod("Damages the character.")]
        public void GetDamaged(
            [CLParam("Killer name.")]
            string killer,
            [CLParam("Damage amount.")]
            int damage)
            => Character.GetDamaged(killer, damage);

        [CLMethod("Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.")]
        public void Emote(
            [CLParam("Name of the emote to play.")]
            string emote)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.Emote(emote);
        }

        [CLMethod("Causes the character to play an animation.")]
        public void PlayAnimation(
            [CLParam("Name of the animation. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation. Note that shifters also have all titan animations.")]
            string animation,
            [CLParam("Fade time. If provided, will crossfade the animation by this timestep.")]
            float fade = 0.1f)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.CrossFadeIfNotPlaying(animation, fade);
        }

        [CLMethod("Causes the character to play an animation at a specific time.")]
        public void PlayAnimationAt(
            [CLParam("Name of the animation.")]
            string animation,
            [CLParam("Time in the animation to start playing.")]
            float t,
            [CLParam("Fade time.")]
            float fade = 0.1f,
            [CLParam("Whether to force the animation even if it's already playing.")]
            bool force = false)
        {
            if (Character.IsMine() && !Character.Dead)
            {
                if (force)
                    Character.CrossFade(animation, fade, t);
                else
                    Character.CrossFadeIfNotPlaying(animation, fade, t);
            }
        }

        [CLMethod("Gets the animation speed of a given animation.")]
        public void GetAnimationSpeed(
            [CLParam("Name of the animation.")]
            string animation)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.GetAnimationSpeed(animation);
        }

        [CLMethod("Sets the animation speed of a given animation.")]
        public void SetAnimationSpeed(
            [CLParam("Name of the animation.")]
            string animation,
            [CLParam("The animation speed multiplier.")]
            float speed,
            [CLParam("Whether to sync the speed across the network.")]
            bool synced = true)
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

        [CLMethod("Returns true if the animation is playing. Returns: True if the animation is playing, false otherwise.")]
        public bool IsPlayingAnimation(
            [CLParam("Name of the animation.")]
            string animation)
        {
            return Character.Animation.IsPlaying(animation);
        }

        [CLMethod("Gets the normalized time of the currently playing animation. Returns: The normalized time (0-1) of the animation.")]
        public float GetAnimationNormalizedTime(
            [CLParam("Name of the animation.")]
            string animation)
        {
            if (!Character.Animation.IsPlaying(animation))
                return 1f;
            return Character.Animation.GetCurrentNormalizedTime();
        }

        [CLMethod("Forces the character to play an animation, even if it's already playing.")]
        public void ForceAnimation(
            [CLParam("Name of the animation. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation. Note that shifters also have all titan animations.")]
            string animation,
            [CLParam("Fade time. If provided, will crossfade the animation by this timestep.")]
            float fade = 0.1f)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.ForceAnimation(animation, fade);
        }

        [CLMethod("Gets the length of animation. Returns: The length of the animation in seconds.")]
        public float GetAnimationLength(
            [CLParam("Name of the animation.")]
            string animation)
        {
            return Character.Animation.GetLength(animation);
        }

        [CLMethod("Returns true if the character is playing a sound. Returns: True if the sound is playing, false otherwise. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.")]
        public bool IsPlayingSound(
            [CLParam("Name of the sound.")]
            string sound)
        {
            return Character.IsPlayingSound(sound);
        }

        [CLMethod("Plays a sound for the character.")]
        public void PlaySound(
            [CLParam("Name of the sound to play. Available sound names can be found here: Human, Shifters, Titans. Note that shifters also have all titan sounds.")]
            string sound)
        {
            if (Character.IsMine() && !Character.Dead && !Character.IsPlayingSound(sound))
                Character.PlaySound(sound);
        }

        [CLMethod("Stops a sound for the character.")]
        public void StopSound(
            [CLParam("Name of the sound to stop.")]
            string sound)
        {
            if (Character.IsMine() && !Character.Dead && Character.IsPlayingSound(sound))
                Character.StopSound(sound);
        }

        [CLMethod("Fades the sound volume to a specific volume between 0.0 and 1.0 over [time] seconds. Does not play or stop the sound.")]
        public void FadeSound(
            [CLParam("Name of the sound.")]
            string sound,
            [CLParam("Target volume (0.0 to 1.0).")]
            float volume,
            [CLParam("Time in seconds to fade over.")]
            float time)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.FadeSound(sound, volume, time);
        }

        [CLMethod("Rotates the character such that it is looking towards a world position.")]
        public void LookAt(
            [CLParam("The world position to look at.")]
            CustomLogicVector3Builtin position)
        {
            if (Character.IsMine() && !Character.Dead)
                Character.Cache.Transform.LookAt(position.Value);
        }

        [CLMethod("Adds a force to the character with given force vector and optional mode.")]
        public void AddForce(
            [CLParam("Force vector.")]
            CustomLogicVector3Builtin force,
            [CLParam("Force mode. Valid modes are Force, Acceleration, Impulse, VelocityChange.")]
            string mode = "Acceleration")
        {
            if (!Character.IsMine()) return;
            Character.SetKinematic(false, 1f);
            var useForceMode = Enum.TryParse(mode, out ForceMode forceMode) ? forceMode : ForceMode.Acceleration;
            Character.Cache.Rigidbody.AddForce(force.Value, useForceMode);
        }

        [CLMethod("Reveal the titan for a set number of seconds.")]
        public void Reveal(
            [CLParam("Delay in seconds before revealing.")]
            float delay)
        {
            Character.Reveal(0, delay);
        }

        [CLMethod("Adds an outline effect with the given color and mode.")]
        public void AddOutline(
            [CLParam("Outline color.")]
            CustomLogicColorBuiltin color = null,
            [CLParam("Outline mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor.")]
            string mode = "OutlineAll")
        {
            Color outlineColor = Color.white;
            if (color != null)
                outlineColor = color.Value.ToColor();

            Outline.Mode outlineMode = (Outline.Mode)Enum.Parse(typeof(Outline.Mode), mode);

            Character.AddOutlineWithColor(outlineColor, outlineMode);
        }

        [CLMethod("Removes the outline effect from the character.")]
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

        [CLMethod("Checks if two characters are equal. Returns: True if the characters are equal, false otherwise.")]
        public bool __Eq__(object self, object other)
        {
            return self.Equals(other);
        }

        [CLMethod("Gets the hash code of the character. Returns: The hash code.")]
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
