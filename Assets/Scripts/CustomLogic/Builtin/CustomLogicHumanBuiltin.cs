using Characters;
using Settings;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicHumanBuiltin : CustomLogicCharacterBuiltin
    {
        public Human Human;

        public CustomLogicHumanBuiltin(Human human) : base(human, "Human")
        {
            Human = human;
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (Human != null && Human.IsMine())
            {
                if (methodName == "Refill")
                {
                    if (Human.NeedRefill())
                        return Human.Refill();
                    return false;
                }
                if (methodName == "RefillImmediate")
                {
                    Human.FinishRefill();
                    return null;
                }
                if (methodName == "ClearHooks")
                {
                    Human.HookLeft.DisableAnyHook();
                    Human.HookRight.DisableAnyHook();
                    return null;
                }
                if (methodName == "ClearLeftHook")
                {
                    Human.HookLeft.DisableAnyHook();
                    return null;
                }
                if (methodName == "ClearRightHook")
                {
                    Human.HookRight.DisableAnyHook();
                    return null;
                }
                if (methodName == "MountMapObject")
                {
                    Vector3 positionOffset = ((CustomLogicVector3Builtin)parameters[1]).Value;
                    Vector3 rotationOffset = ((CustomLogicVector3Builtin)parameters[2]).Value;
                    Human.Mount(((CustomLogicMapObjectBuiltin)parameters[0]).Value, positionOffset, rotationOffset);
                    return null;
                }
                if (methodName == "MountTransform")
                {
                    Vector3 positionOffset = ((CustomLogicVector3Builtin)parameters[1]).Value;
                    Vector3 rotationOffset = ((CustomLogicVector3Builtin)parameters[2]).Value;
                    Human.Mount(((CustomLogicTransformBuiltin)parameters[0]).Value, positionOffset, rotationOffset);
                    return null;
                }
                if (methodName == "Unmount")
                {
                    Human.Unmount(true);
                    return null;
                }
                if (methodName == "SetSpecial")
                {
                    Human.SetSpecial((string)parameters[0]);
                    return null;
                }
                return base.CallMethod(methodName, parameters);
            }
            return null;
        }

        public override object GetField(string name)
        {
            BladeWeapon bladeWeapon = null;
            AmmoWeapon ammoWeapon = null;
            if (Human.Weapon is BladeWeapon)
                bladeWeapon = (BladeWeapon)Human.Weapon;
            else if (Human.Weapon is AmmoWeapon)
                ammoWeapon = (AmmoWeapon)Human.Weapon;
            if (name == "Weapon")
                return Human.Setup.Weapon.ToString();
            if (name == "CurrentSpecial")
                return Human.CurrentSpecial;
            if (name == "CurrentGas")
                return Human.Stats.CurrentGas;
            if (name == "MaxGas")
                return Human.Stats.MaxGas;
            if (name == "Acceleration")
                return Human.Stats.Acceleration;
            if (name == "Speed")
                return Human.Stats.Speed;
            if (name == "HorseSpeed")
                return Human.Stats.HorseSpeed;
            if (name == "CurrentBladeDurability")
            {
                if (bladeWeapon != null)
                    return bladeWeapon.CurrentDurability;
                return 0f;
            }
            if (name == "MaxBladeDurability")
            {
                if (bladeWeapon != null)
                    return bladeWeapon.MaxDurability;
                return 0f;
            }
            if (name == "CurrentBlade")
            {
                if (bladeWeapon != null)
                    return bladeWeapon.BladesLeft;
                return 0;
            }
            if (name == "MaxBlade")
            {
                if (bladeWeapon != null)
                    return bladeWeapon.MaxBlades;
                return 0;
            }
            if (name == "CurrentAmmoRound")
            {
                if (ammoWeapon != null)
                    return ammoWeapon.RoundLeft;
                return 0;
            }
            if (name == "MaxAmmoRound")
            {
                if (ammoWeapon != null)
                    return ammoWeapon.MaxRound;
                return 0;
            }
            if (name == "CurrentAmmoLeft")
            {
                if (ammoWeapon != null)
                    return ammoWeapon.AmmoLeft;
                return 0;
            }
            if (name == "MaxAmmoTotal")
            {
                if (ammoWeapon != null)
                    return ammoWeapon.MaxAmmo;
                return 0;
            }
            if (name == "IsMounted")
                return Human.MountState == HumanMountState.MapObject;
            if (name == "MountedMapObject")
            {
                if (Human.MountedMapObject == null)
                    return null;
                return new CustomLogicMapObjectBuiltin(Human.MountedMapObject);
            }
            if (name == "MountedTransform")
            {
                if (Human.MountedTransform == null)
                    return null;
                return new CustomLogicTransformBuiltin(Human.MountedTransform);
            }
            if (name == "AutoRefillGas")
            {
                if (Human != null && Human.IsMine())
                    return SettingsManager.InputSettings.Human.AutoRefillGas.Value;
                return false;
            }
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (!Human.IsMine())
                return;
            BladeWeapon bladeWeapon = null;
            AmmoWeapon ammoWeapon = null;
            if (Human.Weapon is BladeWeapon)
                bladeWeapon = (BladeWeapon)Human.Weapon;
            else if (Human.Weapon is AmmoWeapon)
                ammoWeapon = (AmmoWeapon)Human.Weapon;
            if (name == "CurrentGas")
                Human.Stats.CurrentGas = Mathf.Min(Human.Stats.MaxGas, value.UnboxToFloat());
            else if (name == "MaxGas")
                Human.Stats.MaxGas = value.UnboxToFloat();
            else if (name == "Acceleration")
                Human.Stats.Acceleration = value.UnboxToInt();
            else if (name == "Speed")
                Human.Stats.Speed = value.UnboxToInt();
            else if (name == "HorseSpeed")
                Human.Stats.HorseSpeed = value.UnboxToFloat();
            else if (name == "CurrentBladeDurability")
            {
                if (bladeWeapon != null)
                    bladeWeapon.CurrentDurability = Mathf.Min(bladeWeapon.MaxDurability, value.UnboxToFloat());
            }
            else if (name == "MaxBladeDurability")
            {
                if (bladeWeapon != null)
                    bladeWeapon.MaxDurability = value.UnboxToFloat();
            }
            else if (name == "CurrentBlade")
            {
                if (bladeWeapon != null)
                    bladeWeapon.BladesLeft = Mathf.Min(value.UnboxToInt(), bladeWeapon.MaxBlades);
            }
            else if (name == "MaxBlade")
            {
                if (bladeWeapon != null)
                    bladeWeapon.MaxBlades = value.UnboxToInt();
            }
            else if (name == "CurrentAmmoRound")
            {
                if (ammoWeapon != null)
                    ammoWeapon.RoundLeft = Mathf.Min(ammoWeapon.MaxRound, value.UnboxToInt());
            }
            else if (name == "MaxAmmoRound")
            {
                if (ammoWeapon != null)
                    ammoWeapon.MaxRound = value.UnboxToInt();
            }
            else if (name == "CurrentAmmoLeft")
            {
                if (ammoWeapon != null)
                    ammoWeapon.AmmoLeft = Mathf.Min(ammoWeapon.MaxAmmo, value.UnboxToInt());
            }
            else if (name == "MaxAmmoTotal")
            {
                if (ammoWeapon != null)
                    ammoWeapon.MaxAmmo = value.UnboxToInt();
            }
            else
                base.SetField(name, value);
            Human.Stats.UpdateStats();
        }
    }
}
