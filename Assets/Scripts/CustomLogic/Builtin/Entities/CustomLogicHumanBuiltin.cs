using System;
using System.Collections.Generic;
using ApplicationManagers;
using Characters;
using Controllers;
using GameManagers;
using Settings;
using System;
using UnityEngine;
using Map;

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

        public HumanAIController Controller;

        public CustomLogicHumanBuiltin(Human human) : base(human)
        {
            Human = human;
            if (Human.AI)
            {
                Controller = (HumanAIController)Human.Controller;
            }
        }

        /// <summary>
        /// Position of the character.
        /// </summary>
        [CLProperty]
        public override CustomLogicVector3Builtin Position
        {
            get => base.Position;
            set
            {
                if (Human.IsMine())
                {
                    Human.IsChangingPosition();
                    base.Position = value;
                }
            }
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

        /// <summary>
        /// Whether horse follow is enabled.
        /// </summary>
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
                    float NewCurrentDurability = Mathf.Clamp(value, 0f, bladeWeapon.MaxDurability);

                    bladeWeapon.CurrentDurability = NewCurrentDurability;

                    if (bladeWasEnabled && NewCurrentDurability <= 0f)
                    {
                        // Only break blade if CurrentDurability > 0 and then be set to 0
                        Human.ToggleBlades(false);
                        Human.PlaySound(HumanSounds.BladeBreak);
                    }
                    else if(!bladeWasEnabled && NewCurrentDurability > 0f)
                    {
                        // Only enable blade if CurrentDurability <= 0 and then be set to greater 0
                        Human.ToggleBlades(true);
                        Human.PlaySound(HumanSounds.BladeReloadGround);
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
        /// The mount state of human. 0: None, 1: Horse, 2: MapObject.
        /// </summary>
        [CLProperty]
        public int MountState => (int)Human.MountState;

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
        /// If the human is on the ground.
        /// </summary>
        [CLProperty]
        public bool Grounded => Human.Grounded;

        /// <summary>
        /// If the human can hold reel in/out.
        /// </summary>
        [CLProperty]
        public bool Pivot => Human.Pivot;

        /// <summary>
        /// The position of the pivot when the human holds reel in/out.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin PivotPosition => Human.PivotPosition();

        /// <summary>
        /// If the left hook is hooked.
        /// </summary>
        [CLProperty]
        public bool IsHookedLeft => Human.HookLeft.IsHooked();

        /// <summary>
        /// If the right hook is hooked.
        /// </summary>
        [CLProperty]
        public bool IsHookedRight => Human.HookRight.IsHooked();

        /// <summary>
        /// If the left hook is in the air.
        /// </summary>
        [CLProperty]
        public bool IsHookingLeft => Human.HookLeft.IsHooking();

        /// <summary>
        /// If the right hook is in the air.
        /// </summary>
        [CLProperty]
        public bool IsHookingRight => Human.HookRight.IsHooking();

        /// <summary>
        /// If the left hook is used.
        /// </summary>
        [CLProperty]
        public bool HasHookLeft => Human.HookLeft.HasHook();

        /// <summary>
        /// If the right hook is used.
        /// </summary>
        [CLProperty]
        public bool HasHookRight => Human.HookRight.HasHook();

        /// <summary>
        /// If the left hook is ready.
        /// </summary>
        [CLProperty]
        public bool LeftHookReady => Human.HookLeft.IsReady();

        /// <summary>
        /// If the right hook is ready.
        /// </summary>
        [CLProperty]
        public bool RightHookReady => Human.HookRight.IsReady();

        /// <summary>
        /// Position of the left hook. Returns null if there is no hook.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin LeftHookPosition => Human.HookLeft.GetCLHookPosition();

        /// <summary>
        /// Position of the right hook. Returns null if there is no hook.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin RightHookPosition => Human.HookRight.GetCLHookPosition();


        /// <summary>
        /// The target currently focused by this character. Returns null if no target is set.
        /// </summary>
        [CLProperty]
        public object Target
        {
            get
            {
                if (!Human.IsMine() || !Human.AI)
                    return null;

                ITargetable enemy = Controller.Target;

                if (enemy == null)
                    return null;

                if (enemy is MapTargetable mapTargetable1)
                {
                    return new CustomLogicMapTargetableBuiltin(mapTargetable1.GameObject, mapTargetable1);
                }
                else if (enemy is Human human)
                {
                    return new CustomLogicHumanBuiltin(human);
                }
                else if (enemy is BaseShifter shifter)
                {
                    return new CustomLogicShifterBuiltin(shifter);
                }
                else if (enemy is BasicTitan titan)
                {
                    return new CustomLogicTitanBuiltin(titan);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (!Human.IsMine() || !Human.AI)
                    return;
                ITargetable itarget = value is CustomLogicMapTargetableBuiltin mapTargetable
                                    ? mapTargetable.Value
                                    : ((CustomLogicCharacterBuiltin)value).Character;
                Controller.Target = itarget;
            }
        }

        /// <summary>
        /// The target position of the (AI) human.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin TargetPosition
        {
            get => Controller.TargetPosition;
            set => Controller.TargetPosition = value.Value;
        }

        /// <summary>
        /// The target velocity of the (AI) human.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin TargetVelocity => Controller.TargetVelocity;

        /// <summary>
        /// Allow the (AI) human to have horse.
        /// </summary>
        [CLProperty]
        public bool AllowHorse
        {
            get => Human.AllowHorse;
            set => Human.AllowHorse = value;
        }

        /// <summary>
        /// Allow the (AI) human to use the skin. (preset skin only)
        /// </summary>
        [CLProperty]
        public bool AllowSkin
        {
            get => Human.AllowSkin;
            set => Human.AllowSkin = value;
        }

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
            if (Human.AI)
            {
                var settings = Human.Settings;
                settings.Loadout.Value = weapon;
                Human.ReloadHuman(settings);
                return;
            }
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
        /// Causes the (AI) human to move towards a position and stopping when within specified range.
        /// </summary>
        /// <param name="position">The target position to move towards.</param>
        /// <param name="range">The stopping range from the target position.</param>
        [CLMethod]
        public void MoveTo(CustomLogicVector3Builtin position, float range)
        {
            if (Human.IsMine() && Human.AI)
                Controller.MoveTo(position.Value, range);
        }

        /// <summary>
        /// Causes the (AI) human to move towards a target and stopping when within specified range.
        /// </summary>
        /// <param name="target">The target to move towards.</param>
        /// <param name="range">The stopping range from the target.</param>
        [CLMethod]
        public void MoveToTarget(object target, float range)
        {
            if (Human.IsMine() && Human.AI)
            {
                ITargetable itarget = target is CustomLogicMapTargetableBuiltin mapTargetable
                                    ? mapTargetable.Value
                                    : ((CustomLogicCharacterBuiltin)target).Character;
                Controller.MoveToTarget(itarget, range);
            }
        }

        /// <summary>
        /// Causes the (AI) human to idle.
        /// </summary>
        [CLMethod]
        public void Idle()
        {
            if (Human.IsMine() && Human.AI)
                Controller.Idle();
        }

        /// <summary>
        /// Determine whether an AIState exists for the (AI) human.
        /// </summary>
        /// <param name="name">The name of the AI state.</param>
        [CLMethod]
        public bool HasAIState(string name)
        {
            if (Human.IsMine() && Human.AI)
            {
                return Controller.HasAIState(name);
            }
            return false;
        }

        /// <summary>
        /// Set the custom AI states for the (AI) human.
        /// </summary>
        /// <param name="name">The name of the AI state.</param>
        /// <param name="classInstance">The class instance for the AI state.</param>
        [CLMethod]
        public void SetAIState(string name, UserClassInstance classInstance = null)
        {
            if (Human.IsMine() && Human.AI)
            {
                if (classInstance is not null)
                {
                    Controllers.HumanAIStates.Custom state = new();
                    state.Init(name, classInstance);
                    Controller.SetAIState(name, state);
                }
                else
                {
                    Controller.AIStates.Remove(name);
                }
            }
        }

        /// <summary>
        /// Get the name of the AI state of the (AI) human.
        /// </summary>
        [CLMethod]
        public string GetAIState()
        {
            if (Human.IsMine() && Human.AI)
            {
                return Controller.AIState?.Name;
            }
            return null;
        }

        /// <summary>
        /// Switch the AI state of the (AI) human.
        /// </summary>
        /// <param name="name">The name of the AI state to switch to.</param>
        [CLMethod]
        public void SwitchAIState(string name)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.SwitchAIState(Controller.AIStates[name]);
            }
        }

        /// <summary>
        /// Reset the callbacks for the (AI) human. Optional: OnIdle, PreAction, PostAction, MoveToCallback.
        /// </summary>
        /// <param name="callback">The callback name to reset.</param>
        /// <param name="method">The method to set as the callback.</param>
        [CLMethod]
        public void ResetCallback(string callback, UserMethod method = null)
        {
            if (Human.IsMine() && Human.AI)
            {
                if (method is null)
                {
                    Controller.Callbacks.GetType().GetField(callback).SetValue(Controller.Callbacks, null);
                }
                else
                {
                    Controller.Callbacks.GetType().GetField(callback).SetValue(Controller.Callbacks, new Action(() => CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { })));
                }
            }
        }

        /// <summary>
        /// Causes the (AI) human to move.
        /// </summary>
        /// <param name="direction">The direction to move in.</param>
        [CLMethod]
        public void Move(CustomLogicVector3Builtin direction)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Move(direction?.Value);
            }
        }

        /// <summary>
        /// Causes the (AI) human to aim at the specific position.
        /// </summary>
        /// <param name="position">The position to aim at.</param>
        [CLMethod]
        public void AimAt(CustomLogicVector3Builtin position)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.AimAt(position?.Value);
            }
        }

        /// <summary>
        /// Causes the (AI) human to jump.
        /// </summary>
        [CLMethod]
        public void Jump()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Jump();
            }
        }

        /// <summary>
        /// Causes the (AI) human to mount on their horse.
        /// </summary>
        /// <param name="mount">True to mount, false to dismount.</param>
        [CLMethod]
        public void HorseMount(bool mount = true)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.HorseMount(mount);
            }
        }

        /// <summary>
        /// Causes the (AI) human to dodge.
        /// </summary>
        [CLMethod]
        public void Dodge()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Dodge();
            }
        }

        /// <summary>
        /// Causes the (AI) human to reload.
        /// </summary>
        [CLMethod]
        public void Reload()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Reload();
            }
        }


        /// <summary>
        /// Causes the (AI) human to use gas.
        /// </summary>
        /// <param name="useGas">True to use gas, false to stop.</param>
        [CLMethod]
        public void UseGas(bool useGas)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.UseGas(useGas);
            }
        }

        /// <summary>
        /// Causes the (AI) human mounted on the horse to walk.
        /// </summary>
        /// <param name="isWalk">True to walk, false to stop walking.</param>
        [CLMethod]
        public void HorseWalk(bool isWalk)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.HorseWalk(isWalk);
            }
        }

        /// <summary>
        /// Causes the (AI) human to dash.
        /// </summary>
        /// <param name="direction">The direction to dash in.</param>
        [CLMethod]
        public void Dash(CustomLogicVector3Builtin direction)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Dash(direction.Value);
            }
        }

        /// <summary>
        /// Causes the (AI) human to reel. -1 is reel in, 1 is reel out, 0 is not reel.
        /// </summary>
        /// <param name="reelAxis">The reel axis value.</param>
        [CLMethod]
        public void Reel(int reelAxis)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Reel(reelAxis);
            }
        }

        /// <summary>
        /// Causes the (AI) human to launch the left hook.
        /// </summary>
        /// <param name="aimPoint">The aim point for the hook.</param>
        [CLMethod]
        public void LaunchHookLeft(CustomLogicVector3Builtin aimPoint)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.LaunchHookLeft(aimPoint.Value);
            }
        }

        /// <summary>
        /// Causes the (AI) human to launch the right hook.
        /// </summary>
        /// <param name="aimPoint">The aim point for the hook.</param>
        [CLMethod]
        public void LaunchHookRight(CustomLogicVector3Builtin aimPoint)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.LaunchHookRight(aimPoint.Value);
            }
        }

        /// <summary>
        /// Causes the (AI) human to release the left hook.
        /// </summary>
        [CLMethod]
        public void ReleaseHookLeft()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.ReleaseHookLeft();
            }
        }

        /// <summary>
        /// Causes the (AI) human to release the right hook.
        /// </summary>
        [CLMethod]
        public void ReleaseHookRight()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.ReleaseHookRight();
            }
        }

        /// <summary>
        /// Causes the (AI) human to release all hooks.
        /// </summary>
        [CLMethod]
        public void ReleaseHookAll()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.ReleaseHookAll();
            }
        }

        /// <summary>
        /// Causes the (AI) human to attack.
        /// </summary>
        /// <param name="attackOn">True to start attacking, false to stop.</param>
        [CLMethod]
        public void Attack(bool attackOn)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Attack(attackOn);
            }
        }

        /// <summary>
        /// Causes the (AI) human to find the nearest enemy.
        /// </summary>
        [CLMethod]
        public void FindNearestEnemy()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Target = Controller.FindNearestEnemy();
            }
        }

        /// <summary>
        /// Correct the direction of the (AI) human for moving to the target.
        /// </summary>
        [CLMethod]
        public void Navigation()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.MoveToPosition();
            }
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
