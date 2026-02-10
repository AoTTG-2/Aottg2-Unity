using System;
using System.Collections.Generic;
using ApplicationManagers;
using Characters;
using GameManagers;
using Settings;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Represents a human character. Only character owner can modify fields and call functions unless otherwise specified.
    /// </summary>
    /// <code>
    /// function OnCharacterSpawn(character)
    /// {
    ///     if (character.IsMainCharacter &amp;&amp; character.Type == "Human")
    ///     {
    ///         character.SetWeapon(WeaponEnum.Blade);
    ///         character.SetSpecial(SpecialEnum.Potato);
    ///         character.CurrentGas = character.MaxGas / 2;
    ///     }
    /// }
    /// </code>
    [CLType(Name = "Human", Abstract = true)]
    partial class CustomLogicHumanBuiltin : CustomLogicCharacterBuiltin
    {
        public Human Human;

        public CustomLogicHumanBuiltin(Human human) : base(human)
        {
            Human = human;
        }

        /// <summary>
        /// The weapon the human is using.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicWeaponEnum) })]
        public string Weapon
        {
            get => Human.Setup.Weapon.ToString();
            set => SetWeapon(value);
        }

        /// <summary>
        /// The current special the human is using.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicSpecialEnum) })]
        public string CurrentSpecial
        {
            get => Human.CurrentSpecial;
            set => SetSpecial(value);
        }

        /// <summary>
        /// The normalized cooldown time of the special. Has a range of 0 to 1.
        /// </summary>
        [CLProperty]
        public float SpecialCooldownTime
        {
            get => Human.Special == null ? 0f : Human.Special.GetCooldownRatio();
            set
            {
                if (Human.Special == null) return;
                var v = Mathf.Max(0f, value);
                Human.Special.SetCooldownRatio(v);
            }
        }

        /// <summary>
        /// The cooldown of the special.
        /// </summary>
        [CLProperty]
        public float SpecialCooldown
        {
            get => Human.Special == null ? 0f : Human.Special.Cooldown;
            set
            {
                if (Human.Special == null) return;
                var v = Mathf.Max(0f, value);
                Human.Special.Cooldown = v;
            }
        }

        /// <summary>
        /// The live time of the shifter special.
        /// </summary>
        [CLProperty]
        public float ShifterLiveTime
        {
            get
            {
                if (Human.Special != null && Human.Special is ShifterTransformSpecial special)
                    return special.LiveTime;
                return 0f;
            }
            set
            {
                if (Human.Special != null && Human.Special is ShifterTransformSpecial special)
                    special.LiveTime = value;
            }
        }

        /// <summary>
        /// The ratio of the special cooldown.
        /// </summary>
        [CLProperty]
        public float SpecialCooldownRatio => Human.Special == null ? 0f : Human.Special.GetCooldownRatio();

        /// <summary>
        /// The current gas of the human.
        /// </summary>
        [CLProperty]
        public float CurrentGas
        {
            get => Human.Stats.CurrentGas;
            set => Human.Stats.CurrentGas = Mathf.Min(Human.Stats.MaxGas, value);
        }

        /// <summary>
        /// The max gas of the human.
        /// </summary>
        [CLProperty]
        public float MaxGas
        {
            get => Human.Stats.MaxGas;
            set
            {
                Human.Stats.MaxGas = value;
                Human.Stats.UpdateStats();
            }
        }

        /// <summary>
        /// The acceleration of the human.
        /// </summary>
        [CLProperty]
        public int Acceleration
        {
            get => Human.Stats.Acceleration;
            set
            {
                Human.Stats.Acceleration = value;
                Human.Stats.UpdateStats();
            }
        }

        /// <summary>
        /// The speed of the human.
        /// </summary>
        [CLProperty]
        public int Speed
        {
            get => Human.Stats.Speed;
            set
            {
                Human.Stats.Speed = value;
                Human.Stats.UpdateStats();
            }
        }

        [CLProperty]
        public bool HorseFollowEnabled
        {
            get
            {
                Horse horse = Human.Horse;
                if (horse != null)
                    return horse.FollowingEnabled;
                return false;
            }
            set
            {
                Horse horse = Human.Horse;
                if (horse != null)
                    horse.FollowingEnabled = value;
            }
        }

        /// <summary>
        /// The transform of the horse belonging to this human. Returns null if horses are disabled.
        /// </summary>
        [CLProperty]
        public CustomLogicTransformBuiltin HorseTransform
        {
            get
            {
                Horse horse = Human.Horse;
                if (horse != null)
                    return new(Human.Horse.transform);
                return null;
            }
        }

        /// <summary>
        /// The speed of the horse.
        /// </summary>
        [CLProperty]
        public float HorseSpeed
        {
            get => Human.Stats.HorseSpeed;
            set => Human.Stats.HorseSpeed = value;
        }

        /// <summary>
        /// The current blade durability.
        /// </summary>
        [CLProperty]
        public float CurrentBladeDurability
        {
            get
            {
                if (Human.Weapon is BladeWeapon bladeWeapon)
                    return bladeWeapon.CurrentDurability;
                return 0f;
            }
            set
            {
                if (Human.Weapon is BladeWeapon bladeWeapon)
                {
                    bool bladeWasEnabled = bladeWeapon.CurrentDurability > 0f;
                    bladeWeapon.CurrentDurability = Mathf.Max(Mathf.Min(bladeWeapon.MaxDurability, value.UnboxToFloat()), 0);
                    if (bladeWeapon.CurrentDurability >= 0f)
                    {
                        Human.ToggleBlades(false);
                        if (bladeWasEnabled)
                            Human.PlaySound(HumanSounds.BladeBreak);
                    }
                }
            }
        }

        /// <summary>
        /// The max blade durability.
        /// </summary>
        [CLProperty]
        public float MaxBladeDurability
        {
            get
            {
                if (Human.Weapon is BladeWeapon bladeWeapon)
                    return bladeWeapon.MaxDurability;
                return 0f;
            }
            set
            {
                if (Human.Weapon is BladeWeapon bladeWeapon)
                    bladeWeapon.MaxDurability = value;
            }
        }

        /// <summary>
        /// The current blade.
        /// </summary>
        [CLProperty]
        public int CurrentBlade
        {
            get
            {
                if (Human.Weapon is BladeWeapon bladeWeapon)
                    return bladeWeapon.BladesLeft;
                return 0;
            }
            set
            {
                if (Human.Weapon is BladeWeapon bladeWeapon)
                    bladeWeapon.BladesLeft = Mathf.Min(value, bladeWeapon.MaxBlades);
            }
        }

        /// <summary>
        /// The max number of blades held.
        /// </summary>
        [CLProperty]
        public int MaxBlade
        {
            get
            {
                if (Human.Weapon is BladeWeapon bladeWeapon)
                    return bladeWeapon.MaxBlades;
                return 0;
            }
            set
            {
                if (Human.Weapon is BladeWeapon bladeWeapon)
                    bladeWeapon.MaxBlades = value;
            }
        }

        /// <summary>
        /// The current ammo round.
        /// </summary>
        [CLProperty]
        public int CurrentAmmoRound
        {
            get
            {
                if (Human.Weapon is AmmoWeapon ammoWeapon)
                    return ammoWeapon.RoundLeft;
                return 0;
            }
            set
            {
                if (Human.Weapon is AmmoWeapon ammoWeapon)
                    ammoWeapon.RoundLeft = Mathf.Min(ammoWeapon.MaxRound, value);
            }
        }

        /// <summary>
        /// The max ammo round.
        /// </summary>
        [CLProperty]
        public int MaxAmmoRound
        {
            get
            {
                if (Human.Weapon is AmmoWeapon ammoWeapon)
                    return ammoWeapon.MaxRound;
                return 0;
            }
            set
            {
                if (Human.Weapon is AmmoWeapon ammoWeapon)
                    ammoWeapon.MaxRound = value;
            }
        }

        /// <summary>
        /// The current ammo left.
        /// </summary>
        [CLProperty]
        public int CurrentAmmoLeft
        {
            get
            {
                if (Human.Weapon is AmmoWeapon ammoWeapon)
                    return ammoWeapon.AmmoLeft;
                return 0;
            }
            set
            {
                if (Human.Weapon is AmmoWeapon ammoWeapon)
                    ammoWeapon.AmmoLeft = Mathf.Min(ammoWeapon.MaxAmmo, value);
            }
        }

        /// <summary>
        /// The max total ammo.
        /// </summary>
        [CLProperty]
        public int MaxAmmoTotal
        {
            get
            {
                if (Human.Weapon is AmmoWeapon ammoWeapon)
                    return ammoWeapon.MaxAmmo;
                return 0;
            }
            set
            {
                if (Human.Weapon is AmmoWeapon ammoWeapon)
                    ammoWeapon.MaxAmmo = value;
            }
        }

        /// <summary>
        /// Whether the left hook is enabled.
        /// </summary>
        [CLProperty]
        public bool LeftHookEnabled
        {
            get => Human.HookLeft.Enabled;
            set => Human.HookLeft.Enabled = value;
        }

        /// <summary>
        /// Whether the right hook is enabled.
        /// </summary>
        [CLProperty]
        public bool RightHookEnabled
        {
            get => Human.HookRight.Enabled;
            set => Human.HookRight.Enabled = value;
        }

        /// <summary>
        /// Whether the human is mounted.
        /// </summary>
        [CLProperty]
        public bool IsMounted => Human.MountState == HumanMountState.MapObject;

        /// <summary>
        /// The map object the human is mounted on.
        /// </summary>
        [CLProperty]
        public CustomLogicMapObjectBuiltin MountedMapObject
        {
            get
            {
                if (Human.MountedMapObject == null)
                    return null;
                return CustomLogicManager.Evaluator.GetOrCreateMapObjectBuiltin(Human.MountedMapObject);
            }
        }

        /// <summary>
        /// The transform the human is mounted on.
        /// </summary>
        [CLProperty]
        public CustomLogicTransformBuiltin MountedTransform
        {
            get
            {
                if (Human.MountedTransform == null)
                    return null;
                return new CustomLogicTransformBuiltin(Human.MountedTransform);
            }
        }

        /// <summary>
        /// Whether the human auto refills gas.
        /// </summary>
        [CLProperty]
        public bool AutoRefillGas
        {
            get => Human != null && Human.IsMine() && SettingsManager.InputSettings.Human.AutoRefillGas.Value;
            set
            {
                if (Human != null && Human.IsMine())
                    SettingsManager.InputSettings.Human.AutoRefillGas.Value = value;
            }
        }

        /// <summary>
        /// The state of the human.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicHumanStateEnum) })]
        public string State => Human.State.ToString();

        /// <summary>
        /// Whether the human can dodge.
        /// </summary>
        [CLProperty]
        public bool CanDodge
        {
            get => Human.CanDodge;
            set => Human.CanDodge = value;
        }

        /// <summary>
        /// Whether the human is invincible.
        /// </summary>
        [CLProperty]
        public bool IsInvincible
        {
            get => Human.IsInvincible;
            set => Human.IsInvincible = value;
        }

        /// <summary>
        /// The time left for invincibility.
        /// </summary>
        [CLProperty]
        public float InvincibleTimeLeft
        {
            get => Human.InvincibleTimeLeft;
            set => Human.InvincibleTimeLeft = value;
        }

        /// <summary>
        /// If the human is carried.
        /// </summary>
        [CLProperty]
        public bool IsCarried => Human.CarryState == HumanCarryState.Carry;

        /// <summary>
        /// Refills the gas of the human.
        /// </summary>
        /// <returns>True if refill was successful, false otherwise.</returns>
        [CLMethod]
        public bool Refill()
        {
            if (Human.IsMine() && Human.NeedRefill(true))
                return Human.Refill();
            return false;
        }

        /// <summary>
        /// Refills the gas of the human immediately.
        /// </summary>
        [CLMethod]
        public void RefillImmediate()
        {
            if (Human.IsMine())
                Human.FinishRefill();
        }

        /// <summary>
        /// Clears all hooks.
        /// </summary>
        [CLMethod]
        public void ClearHooks()
        {
            if (Human.IsMine())
            {
                Human.HookLeft.DisableAnyHook();
                Human.HookRight.DisableAnyHook();
            }
        }

        /// <summary>
        /// Clears the left hook.
        /// </summary>
        [CLMethod]
        public void ClearLeftHook()
        {
            if (Human.IsMine())
                Human.HookLeft.DisableAnyHook();
        }

        /// <summary>
        /// Clears the right hook.
        /// </summary>
        [CLMethod]
        public void ClearRightHook()
        {
            if (Human.IsMine())
                Human.HookRight.DisableAnyHook();
        }

        /// <summary>
        /// Position of the left hook, null if there is no hook.
        /// </summary>
        /// <returns>The position of the left hook, or null if there is no hook.</returns>
        [CLMethod]
        public CustomLogicVector3Builtin LeftHookPosition()
        {
            if (Human.IsMine())
            {
                Vector3 hook = Human.HookLeft.GetHookPosition();
                if (hook != null)
                {
                    return new CustomLogicVector3Builtin(hook);
                }
            }
            return null;
        }

        /// <summary>
        /// Position of the right hook, null if there is no hook.
        /// </summary>
        /// <returns>The position of the right hook, or null if there is no hook.</returns>
        [CLMethod]
        public CustomLogicVector3Builtin RightHookPosition()
        {
            if (Human.IsMine())
            {
                Vector3 hook = Human.HookRight.GetHookPosition();
                if (hook != null)
                {
                    return new CustomLogicVector3Builtin(hook);
                }
            }
            return null;
        }

        /// <summary>
        /// Mounts the human on a map object.
        /// </summary>
        /// <param name="mapObject">The map object to mount on.</param>
        /// <param name="positionOffset">The position offset from the mount point.</param>
        /// <param name="rotationOffset">The rotation offset from the mount point.</param>
        /// <param name="canMountedAttack">If true, allows the human to attack while mounted (default: false).</param>
        [CLMethod]
        public void MountMapObject(
            CustomLogicMapObjectBuiltin mapObject,
            CustomLogicVector3Builtin positionOffset,
            CustomLogicVector3Builtin rotationOffset,
            bool canMountedAttack = false)
        {
            if (Human.IsMine())
                Human.Mount(mapObject.Value, positionOffset.Value, rotationOffset.Value, canMountedAttack);
        }

        /// <summary>
        /// Mounts the human on a transform.
        /// </summary>
        /// <param name="transform">The transform to mount on.</param>
        /// <param name="positionOffset">The position offset from the mount point.</param>
        /// <param name="rotationOffset">The rotation offset from the mount point.</param>
        /// <param name="canMountedAttack">If true, allows the human to attack while mounted (default: false).</param>
        [CLMethod]
        public void MountTransform(
            CustomLogicTransformBuiltin transform,
            CustomLogicVector3Builtin positionOffset,
            CustomLogicVector3Builtin rotationOffset,
            bool canMountedAttack = false)
        {
            if (Human.IsMine())
                Human.Mount(transform.Value, positionOffset.Value, rotationOffset.Value, canMountedAttack);
        }

        /// <summary>
        /// Unmounts the human.
        /// </summary>
        /// <param name="immediate">If true, unmounts immediately without animation (default: true).</param>
        [CLMethod]
        public void Unmount(bool immediate = true)
        {
            if (Human.IsMine())
                Human.Unmount(immediate);
        }

        /// <summary>
        /// Sets the special of the human.
        /// </summary>
        /// <param name="special">The name of the special to set.</param>
        [CLMethod]
        public void SetSpecial([CLParam(Enum = new Type[] { typeof(CustomLogicSpecialEnum) })] string special)
        {
            if (Human.IsMine())
                Human.SetSpecial(special);
        }

        /// <summary>
        /// Activates the special of the human.
        /// </summary>
        [CLMethod]
        public void ActivateSpecial()
        {
            if (Human.IsMine() && Human.Special != null)
            {
                Human.Special.SetInput(true);
                Human.Special.SetInput(false);
            }
        }

        /// <summary>
        /// Sets the weapon for the human.
        /// </summary>
        /// <param name="weapon">Name of the weapon.</param>
        [CLMethod]
        public void SetWeapon([CLParam(Enum = new Type[] { typeof(CustomLogicWeaponEnum) })] string weapon)
        {
            if (!Human.IsMine())
                return;
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (gameManager.CurrentCharacter != null && gameManager.CurrentCharacter is Human && Human.IsMine())
            {
                // TODO: Remove on the next update when CL developers will migrate to the new enum.
                if (weapon == "Blades")
                    weapon = CustomLogicWeaponEnum.Blade; // Normalize to Blade for compatibility
                else if (weapon == "Thunderspears")
                    weapon = CustomLogicWeaponEnum.Thunderspear; // Normalize to Thunderspear for compatibility

                var miscSettings = SettingsManager.InGameCurrent.Misc;
                if (!Human.Dead)
                {
                    List<string> loadouts = new List<string>();
                    if (miscSettings.AllowBlades.Value)
                        loadouts.Add(HumanLoadout.Blade);
                    if (miscSettings.AllowAHSS.Value)
                        loadouts.Add(HumanLoadout.AHSS);
                    if (miscSettings.AllowAPG.Value)
                        loadouts.Add(HumanLoadout.APG);
                    if (miscSettings.AllowThunderspears.Value)
                        loadouts.Add(HumanLoadout.Thunderspear);
                    if (loadouts.Count == 0)
                        loadouts.Add(HumanLoadout.Blade);
                    if (loadouts.Contains(weapon) && weapon != SettingsManager.InGameCharacterSettings.Loadout.Value)
                    {
                        SettingsManager.InGameCharacterSettings.Loadout.Value = weapon;
                        var manager = (InGameManager)SceneLoader.CurrentGameManager;
                        Human = (Human)gameManager.CurrentCharacter;
                        Human.ReloadHuman(manager.GetSetHumanSettings());
                    }
                }
            }
        }

        /// <summary>
        /// Disables all perks of the human.
        /// </summary>
        [CLMethod]
        public void DisablePerks()
        {
            if (Human.IsMine())
                Human.Stats.DisablePerks();
        }

        /// <summary>
        /// Enables or disables a particle effect.
        /// </summary>
        /// <param name="effectName">Name of the effect.</param>
        /// <param name="enabled">True to enable, false to disable.</param>
        [CLMethod]
        public void SetParticleEffect(
            [CLParam(Enum = new Type[] { typeof(CustomLogicHumanParticleEffectEnum) })] string effectName,
            bool enabled)
        {
            if (!Human.IsMine())
                return;

            switch (effectName)
            {
                case CustomLogicHumanParticleEffectEnum.Buff1Value:
                    Human.ToggleBuff1(enabled);
                    break;
                case CustomLogicHumanParticleEffectEnum.Buff2Value:
                    Human.ToggleBuff2(enabled);
                    break;
                case CustomLogicHumanParticleEffectEnum.Fire1Value:
                    Human.ToggleFire1(enabled);
                    break;
                default:
                    // Invalid effect name, do nothing
                    break;
            }
        }

    }
}
