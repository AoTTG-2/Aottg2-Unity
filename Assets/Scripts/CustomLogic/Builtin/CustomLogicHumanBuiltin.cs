using ApplicationManagers;
using Characters;
using Controllers;
using GameManagers;
using Settings;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Map;

namespace CustomLogic
{
    /// <summary>
    /// Only character owner can modify fields and call functions unless otherwise specified.
    /// </summary>
    /// <code>
    /// function OnCharacterSpawn(character) {
    ///     if (character.IsMainCharacter && character.Type == "Human") {
    ///         character.SetWeapon("Blade");
    ///         character.SetSpecial("Potato");
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

        [CLProperty(description: "The human's name")]
        public string Name
        {
            get => Human.Name;
            set => Human.Name = value;
        }

        [CLProperty(description: "The human's guild")]
        public string Guild
        {
            get => Human.Guild;
            set => Human.Guild = value;
        }

        // Add CLProperties for the above setField/getField 
        [CLProperty(description: "The weapon the human is using")]
        public string Weapon
        {
            get => Human.Setup.Weapon.ToString();
            set => SetWeapon(value);
        }

        [CLProperty(description: "The current special the human is using")]
        public string CurrentSpecial
        {
            get => Human.CurrentSpecial;
            set => SetSpecial(value);
        }

        [CLProperty(description: "The normalized cooldown time of the special. Has a range of 0 to 1.")]
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

        [CLProperty(description: "The cooldown of the special")]
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

        [CLProperty(description: "The live time of the shifter special")]
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

        [CLProperty(description: "The ratio of the special cooldown")]
        public float SpecialCooldownRatio => Human.Special == null ? 0f : Human.Special.GetCooldownRatio();

        [CLProperty(description: "The current gas of the human")]
        public float CurrentGas
        {
            get => Human.Stats.CurrentGas;
            set => Human.Stats.CurrentGas = Mathf.Min(Human.Stats.MaxGas, value);
        }

        [CLProperty(description: "The max gas of the human")]
        public float MaxGas
        {
            get => Human.Stats.MaxGas;
            set => Human.Stats.MaxGas = value;
        }

        [CLProperty(description: "The acceleration of the human")]
        public int Acceleration
        {
            get => Human.Stats.Acceleration;
            set => Human.Stats.Acceleration = value;
        }

        [CLProperty(description: "The speed of the human")]
        public int Speed
        {
            get => Human.Stats.Speed;
            set => Human.Stats.Speed = value;
        }

        [CLProperty(description: "The speed of the horse")]
        public float HorseSpeed
        {
            get => Human.Stats.HorseSpeed;
            set => Human.Stats.HorseSpeed = value;
        }

        [CLProperty(description: "The current blade durability")]
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

        [CLProperty(description: "The max blade durability")]
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

        [CLProperty(description: "The current blade")]
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

        [CLProperty(description: "The max number of blades held")]
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

        [CLProperty(description: "The current ammo round")]
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

        [CLProperty(description: "The max ammo round")]
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

        [CLProperty(description: "The current ammo left")]
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

        [CLProperty(description: "The max total ammo")]
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

        [CLProperty(description: "Whether the left hook is enabled")]
        public bool LeftHookEnabled
        {
            get => Human.HookLeft.Enabled;
            set => Human.HookLeft.Enabled = value;
        }

        [CLProperty(description: "Whether the right hook is enabled")]
        public bool RightHookEnabled
        {
            get => Human.HookRight.Enabled;
            set => Human.HookRight.Enabled = value;
        }

        [CLProperty(description: "Whether the human is mounted")]
        public bool IsMounted => Human.MountState == HumanMountState.MapObject;

        [CLProperty(description: "The mount state of human. 0: None, 1: Horse, 2:MapObject")]
        public int MountState => (int)Human.MountState;

        [CLProperty(description: "The map object the human is mounted on")]
        public CustomLogicMapObjectBuiltin MountedMapObject
        {
            get
            {
                if (Human.MountedMapObject == null)
                    return null;
                return new CustomLogicMapObjectBuiltin(Human.MountedMapObject);
            }
        }

        [CLProperty(description: "The transform the human is mounted on")]
        public CustomLogicTransformBuiltin MountedTransform
        {
            get
            {
                if (Human.MountedTransform == null)
                    return null;
                return new CustomLogicTransformBuiltin(Human.MountedTransform);
            }
        }

        [CLProperty(description: "Whether the human auto refills gas")]
        public bool AutoRefillGas
        {
            get => Human != null && Human.IsMine() && SettingsManager.InputSettings.Human.AutoRefillGas.Value;
            set
            {
                if (Human != null && Human.IsMine())
                    SettingsManager.InputSettings.Human.AutoRefillGas.Value = value;
            }
        }

        [CLProperty(description: "The state of the human")]
        public string State => Human.State.ToString();

        [CLProperty(description: "Whether the human can dodge")]
        public bool CanDodge
        {
            get => Human.CanDodge;
            set => Human.CanDodge = value;
        }

        [CLProperty(description: "Whether the human is invincible")]
        public bool IsInvincible
        {
            get => Human.IsInvincible;
            set => Human.IsInvincible = value;
        }

        [CLProperty(description: "The time left for invincibility")]
        public float InvincibleTimeLeft
        {
            get => Human.InvincibleTimeLeft;
            set => Human.InvincibleTimeLeft = value;
        }

        [CLProperty(description: "If the human is carried.")]
        public bool IsCarried => Human.CarryState == HumanCarryState.Carry;

        [CLProperty(description: "If the human is on the ground.")]
        public bool Grounded => Human.Grounded;

        [CLProperty(description: "If the human can hold reel in/out.")]
        public bool Pivot => Human.Pivot;

        [CLProperty(description: "The position of the pivot when the human hold reel in/out.")]
        public CustomLogicVector3Builtin PivotPosition => Human.PivotPosition();

        [CLProperty(description: "If left hook is hooked.")]
        public bool IsHookedLeft => Human.HookLeft.IsHooked();

        [CLProperty(description: "If right hook is hooked.")]
        public bool IsHookedRight => Human.HookRight.IsHooked();

        [CLProperty(description: "If left hook is in the air.")]
        public bool IsHookingLeft => Human.HookLeft.IsHooking();

        [CLProperty(description: "If right hook is in the air.")]
        public bool IsHookingRight => Human.HookRight.IsHooking();

        [CLProperty(description: "If left hook is used.")]
        public bool HasHookLeft => Human.HookLeft.HasHook();

        [CLProperty(description: "If right hook is used.")]
        public bool HasHookRight => Human.HookRight.HasHook();

        [CLProperty(description: "If left hook is ready.")]
        public bool LeftHookReady => Human.HookLeft.IsReady();

        [CLProperty(description: "If right hook is ready.")]
        public bool RightHookReady => Human.HookRight.IsReady();


        [CLProperty("The target currently focused by this character. Returns null if no target is set.")]
        public object Target
        {
            get
            {
                if (!Human.IsMine() || !Human.AI)
                    return null;

                ITargetable enemy = Controller.Target;

                if (enemy == null)
                    return null;

                if (enemy is CustomLogicMapTargetableBuiltin)
                {
                    MapTargetable mapTargetable1 = (MapTargetable)enemy;
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

        [CLProperty("The target position of the (AI) human.")]
        public CustomLogicVector3Builtin TargetPosition
        {
            get => Controller.TargetPosition;
            set => Controller.TargetPosition = value.Value;
        }

        [CLProperty("The target velocity of the (AI) human.")]
        public CustomLogicVector3Builtin TargetVelocity => Controller.TargetVelocity;

        // Add CLMethods for the above setField/getField
        [CLMethod(description: "Refills the gas of the human")]
        public bool Refill()
        {
            if (Human.IsMine() && Human.NeedRefill(true))
                return Human.Refill();
            return false;
        }

        [CLMethod(description: "Refills the gas of the human immediately")]
        public void RefillImmediate()
        {
            if (Human.IsMine())
                Human.FinishRefill();
        }

        [CLMethod(description: "Clears all hooks")]
        public void ClearHooks()
        {
            if (Human.IsMine())
            {
                Human.HookLeft.DisableAnyHook();
                Human.HookRight.DisableAnyHook();
            }
        }

        [CLMethod(description: "Clears the left hook")]
        public void ClearLeftHook()
        {
            if (Human.IsMine())
                Human.HookLeft.DisableAnyHook();
        }

        [CLMethod(description: "Clears the right hook")]
        public void ClearRightHook()
        {
            if (Human.IsMine())
                Human.HookRight.DisableAnyHook();
        }

        [CLMethod(description: "Position of the left hook, Vector3.zero if there is no hook.")]
        public CustomLogicVector3Builtin LeftHookPosition() => Human.HookLeft.GetHookPosition();

        [CLMethod(description: "Position of the right hook, Vector3.zero if there is no hook.")]
        public CustomLogicVector3Builtin RightHookPosition() => Human.HookRight.GetHookPosition();

        [CLMethod(description: "Mounts the human on a map object")]
        public void MountMapObject(CustomLogicMapObjectBuiltin mapObject, CustomLogicVector3Builtin positionOffset, CustomLogicVector3Builtin rotationOffset, bool canMountedAttack = false)
        {
            if (Human.IsMine())
                Human.Mount(mapObject.Value, positionOffset.Value, rotationOffset.Value, canMountedAttack);
        }

        [CLMethod(description: "Mounts the human on a transform")]
        public void MountTransform(CustomLogicTransformBuiltin transform, CustomLogicVector3Builtin positionOffset, CustomLogicVector3Builtin rotationOffset, bool canMountedAttack = false)
        {
            if (Human.IsMine())
                Human.Mount(transform.Value, positionOffset.Value, rotationOffset.Value, canMountedAttack);
        }

        [CLMethod(description: "Unmounts the human")]
        public void Unmount(bool immediate = true)
        {
            if (Human.IsMine())
                Human.Unmount(immediate);
        }

        [CLMethod(description: "Sets the special of the human")]
        public void SetSpecial(string special)
        {
            if (Human.IsMine())
                Human.SetSpecial(special);
        }

        [CLMethod(description: "Activates the special of the human")]
        public void ActivateSpecial()
        {
            if (Human.IsMine() && Human.Special != null)
            {
                Human.Special.SetInput(true);
                Human.Special.SetInput(false);
            }
        }

        [CLMethod(description: "Sets the weapon of the human")]
        public void SetWeapon(string weapon)
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

        [CLMethod(description: "Disables all perks of the human")]
        public void DisablePerks()
        {
            if (Human.IsMine())
                Human.Stats.DisablePerks();
        }


        [CLMethod(description: "Causes the (AI) human to move towards a position and stopping when within specified range")]
        public void MoveTo(CustomLogicVector3Builtin position, float range)
        {
            if (Human.IsMine() && Human.AI)
                Controller.MoveTo(position.Value, range);
        }

        [CLMethod(description: "Causes the (AI) human to move towards a target and stopping when within specified range")]
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

        [CLMethod(description: "Causes the (AI) human to move towards a position and stopping when within specified range")]
        public void Idle()
        {
            if (Human.IsMine() && Human.AI)
                Controller.Idle();
        }

        [CLMethod(description: "Determine whether an AIState exists for the (AI) humans")]
        public bool HasAIState(string name)
        {
            if (Human.IsMine() && Human.AI)
            {
                return Controller.HasAIState(name);
            }
            return false;
        }

        [CLMethod(description: "Set the custom ai states for the (AI) humans")]
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

        [CLMethod(description: "Get the name of the ai state of the (AI) human.")]
        public string GetAIState()
        {
            if (Human.IsMine() && Human.AI)
            {
                return Controller.AIState?.Name;
            }
            return null;
        }

        [CLMethod(description: "Switch the ai state of the (AI) human.")]
        public void SwitchAIState(string name)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.SwitchAIState(Controller.AIStates[name]);
            }
        }

        [CLMethod(description: "Reset the callbacks for the (AI) humans")]
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

        [CLMethod(description: "Causes the (AI) humans to Move")]
        public void Move(CustomLogicVector3Builtin direction)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Move(direction?.Value);
            }
        }

        [CLMethod(description: "Causes the (AI) humans to aim at the specific position.")]
        public void AimAt(CustomLogicVector3Builtin position)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.AimAt(position?.Value);
            }
        }

        [CLMethod(description: "Causes the (AI) humans to Jump")]
        public void Jump()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Jump();
            }
        }

        [CLMethod(description: "Causes the (AI) humans to mount on their horse")]
        public void HorseMount(bool mount = true)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.HorseMount(mount);
            }
        }

        [CLMethod(description: "Causes the (AI) humans to Dodge")]
        public void Dodge()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Dodge();
            }
        }

        [CLMethod(description: "Causes the (AI) humans to Reload")]
        public void Reload()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Reload();
            }
        }


        [CLMethod(description: "Causes the (AI) humans to use gas")]
        public void UseGas(bool useGas)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.UseGas(useGas);
            }
        }

        [CLMethod(description: "Causes the (AI) humans mounted on the horse to walk")]
        public void HorseWalk(bool isWalk)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.HorseWalk(isWalk);
            }
        }

        [CLMethod(description: "Causes the (AI) humans mounted on the horse to walk")]
        public void Dash(CustomLogicVector3Builtin direction)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Dash(direction.Value);
            }
        }

        [CLMethod(description: "Causes the (AI) humans to reel, -1 is reel in, 1 is reel out, 0 is not reel")]
        public void Reel(int reelAxis)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Reel(reelAxis);
            }
        }

        [CLMethod(description: "Causes the (AI) humans to launch left hook.")]
        public void LaunchHookLeft(CustomLogicVector3Builtin aimPoint)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.LaunchHookLeft(aimPoint.Value);
            }
        }

        [CLMethod(description: "Causes the (AI) humans to launch right hook.")]
        public void LaunchHookRight(CustomLogicVector3Builtin aimPoint)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.LaunchHookRight(aimPoint.Value);
            }
        }

        [CLMethod(description: "Causes the (AI) humans to release left hook.")]
        public void ReleaseHookLeft()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.ReleaseHookLeft();
            }
        }

        [CLMethod(description: "Causes the (AI) humans to release right hook.")]
        public void ReleaseHookRight()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.ReleaseHookRight();
            }
        }

        [CLMethod(description: "Causes the (AI) humans to release all hooks.")]
        public void ReleaseHookAll()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.ReleaseHookAll();
            }
        }

        [CLMethod(description: "Causes the (AI) humans to attack.")]
        public void Attack(bool attackOn)
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Attack(attackOn);
            }
        }

        [CLMethod(description: "Causes the (AI) humans to find the nearest enemy")]
        public void FindNearestEnemy()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.Target = Controller.FindNearestEnemy();
            }
        }

        [CLMethod(description: "Correct the direction of (AI) humans for moving to the target.")]
        public void Navigation()
        {
            if (Human.IsMine() && Human.AI)
            {
                Controller.MoveToPosition();
            }
        }

    }
}
