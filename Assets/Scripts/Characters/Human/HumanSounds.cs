namespace Characters
{
    class HumanSounds
    {
        public static string BladeBreak = "BladeBreak";
        public static string BladeHit = "BladeHit";
        public static string OldBladeHit = "OldBladeHit";
        public static string NapeHit = "NapeHit";
        public static string LimbHit = "LimbHit";
        public static string OldNapeHit = "OldNapeHit";
        public static string BladeReloadAir = "BladeReloadAir";
        public static string BladeReloadGround = "BladeReloadGround";
        public static string GunReload = "GunReload";
        public static string BladeSwing1 = "BladeSwing1";
        public static string BladeSwing2 = "BladeSwing2";
        public static string BladeSwing3 = "BladeSwing3";
        public static string BladeSwing4 = "BladeSwing4";
        public static string OldBladeSwing = "OldBladeSwing";
        public static string Dodge = "Dodge";
        public static string FlareLaunch = "FlareLaunch";
        public static string ThunderspearLaunch = "ThunderspearLaunch";
        public static string GasBurst = "GasBurst";
        public static string HookLaunch = "HookLaunch";
        public static string OldHookLaunch = "OldHookLaunch";
        public static string HookRetractLeft = "HookRetractLeft";
        public static string HookRetractRight = "HookRetractRight";
        public static string HookImpact = "HookImpact";
        public static string HookImpactLoud = "HookImpactLoud";
        public static string GasStart = "GasStart";
        public static string GasLoop = "GasLoop";
        public static string GasEnd = "GasEnd";
        public static string ReelIn = "ReelIn";
        public static string ReelOut = "ReelOut";
        public static string CrashLand = "CrashLand";
        public static string Jump = "Jump";
        public static string Land = "Land";
        public static string NoGas = "NoGas";
        public static string Refill = "Refill";
        public static string Slide = "Slide";
        public static string Footstep1 = "Footstep1";
        public static string Footstep2 = "Footstep2";
        public static string Death1 = "Death1";
        public static string Death2 = "Death2";
        public static string Death3 = "Death3";
        public static string Death4 = "Death4";
        public static string Death5 = "Death5";
        public static string Checkpoint = "Checkpoint";
        public static string GunExplode = "GunExplode";
        public static string GunExplodeLoud = "GunExplodeLoud";
        public static string WaterSplash = "WaterSplash";
        public static string Switchback = "Switchback";
        public static string APGShot1 = "APGShot1";
        public static string APGShot2 = "APGShot2";
        public static string APGShot3 = "APGShot3";
        public static string APGShot4 = "APGShot4";
        public static string BladeNape1 = "BladeNape1";
        public static string BladeNape2 = "BladeNape2";
        public static string BladeNape3 = "BladeNape2";
        public static string BladeNapeCrit1 = "BladeNapeCrit1";
        public static string BladeNapeCrit2 = "BladeNapeCrit2";
        public static string BladeNapeCrit3 = "BladeNapeCrit3";

        // Get random sound effect from list
        public static string GetRandom(params string[] sounds)
        {
            return sounds[UnityEngine.Random.Range(0, sounds.Length)];
        }

        // Get random apg shot
        public static string GetRandomAPGShot()
        {
            return GetRandom(APGShot1, APGShot2, APGShot3, APGShot4);
        }

        public static string GetRandomBladeNape()
        {
            return GetRandom(BladeNape1, BladeNape2, BladeNape3);
        }

        public static string GetRandomBladeNapeCrit()
        {
            return GetRandom(BladeNapeCrit1, BladeNapeCrit2, BladeNapeCrit3);
        }
    }
}
