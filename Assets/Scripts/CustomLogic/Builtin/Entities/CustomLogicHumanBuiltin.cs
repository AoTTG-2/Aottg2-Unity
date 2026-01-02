using System.Collections.Generic;
using ApplicationManagers;
using Characters;
using GameManagers;
using Settings;
using UnityEngine;

namespace CustomLogic
{
    /// <code>
    /// function OnCharacterSpawn(character)
    /// {
    ///     if (character.IsMainCharacter &amp;&amp; character.Type == "Human")
    ///     {
    ///         character.SetWeapon("Blade");
    ///         character.SetSpecial("Potato");
    ///         character.CurrentGas = character.MaxGas / 2;
    ///     }
    /// }
    /// </code>
    [CLType(Name = "Human", Abstract = true, Description = "Represents a human character. Only character owner can modify fields and call functions unless otherwise specified.")]
    partial class CustomLogicHumanBuiltin : CustomLogicCharacterBuiltin
    {
        public Human Human;

        public CustomLogicHumanBuiltin(Human human) : base(human)
        {
            Human = human;
        }

        [CLProperty("The weapon the human is using.")]
        public string Weapon
        {
            get => Human.Setup.Weapon.ToString();
            set => SetWeapon(value);
        }

        [CLProperty("The current special the human is using.")]
        public string CurrentSpecial
        {
            get => Human.CurrentSpecial;
            set => SetSpecial(value);
        }

        [CLProperty("The normalized cooldown time of the special. Has a range of 0 to 1.")]
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

        [CLProperty("The cooldown of the special.")]
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

        [CLProperty("The live time of the shifter special.")]
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

        [CLProperty("The ratio of the special cooldown.")]
        public float SpecialCooldownRatio => Human.Special == null ? 0f : Human.Special.GetCooldownRatio();

        [CLProperty("The current gas of the human.")]
        public float CurrentGas
        {
            get => Human.Stats.CurrentGas;
            set => Human.Stats.CurrentGas = Mathf.Min(Human.Stats.MaxGas, value);
        }

        [CLProperty("The max gas of the human.")]
        public float MaxGas
        {
            get => Human.Stats.MaxGas;
            set => Human.Stats.MaxGas = value;
        }

        [CLProperty("The acceleration of the human.")]
        public int Acceleration
        {
            get => Human.Stats.Acceleration;
            set => Human.Stats.Acceleration = value;
        }

        [CLProperty("The speed of the human.")]
        public int Speed
        {
            get => Human.Stats.Speed;
            set => Human.Stats.Speed = value;
        }

        [CLProperty("The transform of the horse belonging to this human. Returns null if horses are disabled.")]
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

        [CLProperty("The speed of the horse.")]
        public float HorseSpeed
        {
            get => Human.Stats.HorseSpeed;
            set => Human.Stats.HorseSpeed = value;
        }

        [CLProperty("The current blade durability.")]
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

        [CLProperty("The max blade durability.")]
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

        [CLProperty("The current blade.")]
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

        [CLProperty("The max number of blades held.")]
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

        [CLProperty("The current ammo round.")]
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

        [CLProperty("The max ammo round.")]
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

        [CLProperty("The current ammo left.")]
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

        [CLProperty("The max total ammo.")]
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

        [CLProperty("Whether the left hook is enabled.")]
        public bool LeftHookEnabled
        {
            get => Human.HookLeft.Enabled;
            set => Human.HookLeft.Enabled = value;
        }

        [CLProperty("Whether the right hook is enabled.")]
        public bool RightHookEnabled
        {
            get => Human.HookRight.Enabled;
            set => Human.HookRight.Enabled = value;
        }

        [CLProperty("Whether the human is mounted.")]
        public bool IsMounted => Human.MountState == HumanMountState.MapObject;

        [CLProperty("The map object the human is mounted on.")]
        public CustomLogicMapObjectBuiltin MountedMapObject
        {
            get
            {
                if (Human.MountedMapObject == null)
                    return null;
                return new CustomLogicMapObjectBuiltin(Human.MountedMapObject);
            }
        }

        [CLProperty("The transform the human is mounted on.")]
        public CustomLogicTransformBuiltin MountedTransform
        {
            get
            {
                if (Human.MountedTransform == null)
                    return null;
                return new CustomLogicTransformBuiltin(Human.MountedTransform);
            }
        }

        [CLProperty("Whether the human auto refills gas.")]
        public bool AutoRefillGas
        {
            get => Human != null && Human.IsMine() && SettingsManager.InputSettings.Human.AutoRefillGas.Value;
            set
            {
                if (Human != null && Human.IsMine())
                    SettingsManager.InputSettings.Human.AutoRefillGas.Value = value;
            }
        }

        [CLProperty("The state of the human.")]
        public string State => Human.State.ToString();

        [CLProperty("Whether the human can dodge.")]
        public bool CanDodge
        {
            get => Human.CanDodge;
            set => Human.CanDodge = value;
        }

        [CLProperty("Whether the human is invincible.")]
        public bool IsInvincible
        {
            get => Human.IsInvincible;
            set => Human.IsInvincible = value;
        }

        [CLProperty("The time left for invincibility.")]
        public float InvincibleTimeLeft
        {
            get => Human.InvincibleTimeLeft;
            set => Human.InvincibleTimeLeft = value;
        }

        [CLProperty("If the human is carried.")]
        public bool IsCarried => Human.CarryState == HumanCarryState.Carry;

        [CLMethod("Refills the gas of the human.")]
        public bool Refill()
        {
            if (Human.IsMine() && Human.NeedRefill(true))
                return Human.Refill();
            return false;
        }

        [CLMethod("Refills the gas of the human immediately.")]
        public void RefillImmediate()
        {
            if (Human.IsMine())
                Human.FinishRefill();
        }

        [CLMethod("Clears all hooks.")]
        public void ClearHooks()
        {
            if (Human.IsMine())
            {
                Human.HookLeft.DisableAnyHook();
                Human.HookRight.DisableAnyHook();
            }
        }

        [CLMethod("Clears the left hook.")]
        public void ClearLeftHook()
        {
            if (Human.IsMine())
                Human.HookLeft.DisableAnyHook();
        }

        [CLMethod("Clears the right hook.")]
        public void ClearRightHook()
        {
            if (Human.IsMine())
                Human.HookRight.DisableAnyHook();
        }

        [CLMethod("Position of the left hook, null if there is no hook.")]
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

        [CLMethod("Position of the right hook, null if there is no hook.")]
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

        [CLMethod("Mounts the human on a map object.")]
        public void MountMapObject(CustomLogicMapObjectBuiltin mapObject, CustomLogicVector3Builtin positionOffset, CustomLogicVector3Builtin rotationOffset, bool canMountedAttack = false)
        {
            if (Human.IsMine())
                Human.Mount(mapObject.Value, positionOffset.Value, rotationOffset.Value, canMountedAttack);
        }

        [CLMethod("Mounts the human on a transform.")]
        public void MountTransform(CustomLogicTransformBuiltin transform, CustomLogicVector3Builtin positionOffset, CustomLogicVector3Builtin rotationOffset, bool canMountedAttack = false)
        {
            if (Human.IsMine())
                Human.Mount(transform.Value, positionOffset.Value, rotationOffset.Value, canMountedAttack);
        }

        [CLMethod("Unmounts the human.")]
        public void Unmount(bool immediate = true)
        {
            if (Human.IsMine())
                Human.Unmount(immediate);
        }

        [CLMethod("Sets the special of the human.")]
        public void SetSpecial(string special)
        {
            if (Human.IsMine())
                Human.SetSpecial(special);
        }

        [CLMethod("Activates the special of the human.")]
        public void ActivateSpecial()
        {
            if (Human.IsMine() && Human.Special != null)
            {
                Human.Special.SetInput(true);
                Human.Special.SetInput(false);
            }
        }

        [CLMethod("Sets the weapon for the human. Available weapons: \"Blade\", \"AHSS\", \"APG\", \"Thunderspear\".")]
        public void SetWeapon(
            [CLParam("Name of the weapon. Available weapons: \"Blade\", \"AHSS\", \"APG\", \"Thunderspear\"")]
            string weapon)
        {
            if (!Human.IsMine())
                return;
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (gameManager.CurrentCharacter != null && gameManager.CurrentCharacter is Human && Human.IsMine())
            {

                if (weapon == "Blades")
                    weapon = "Blade"; // Normalize to Blade for compatibility
                else if (weapon == "Thunderspears")
                    weapon = "Thunderspear"; // Normalize to Thunderspear for compatibility

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

        [CLMethod("Disables all perks of the human.")]
        public void DisablePerks()
        {
            if (Human.IsMine())
                Human.Stats.DisablePerks();
        }

        [CLMethod("Enables or disables a particle effect. Available effects: Buff1, Buff2, Fire1.")]
        public void SetParticleEffect(
            [CLParam("Name of the effect: \"Buff1\", \"Buff2\", or \"Fire1\"")]
            string effectName,
            [CLParam("True to enable, false to disable")]
            bool enabled)
        {
            if (!Human.IsMine())
                return;

            switch (effectName)
            {
                case "Buff1":
                    Human.ToggleBuff1(enabled);
                    break;
                case "Buff2":
                    Human.ToggleBuff2(enabled);
                    break;
                case "Fire1":
                    Human.ToggleFire1(enabled);
                    break;
                default:
                    // Invalid effect name, do nothing
                    break;
            }
        }

    }
}
