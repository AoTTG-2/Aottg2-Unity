namespace Settings
{
    class TitanInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "TitanInput.json"; } }
        public KeybindSetting Kick = new KeybindSetting(new string[] { "LeftControl", "None" });
        public KeybindSetting Jump = new KeybindSetting(new string[] { "Space", "None" });
        public KeybindSetting Sit = new KeybindSetting(new string[] { "Z", "None" });
        public KeybindSetting Walk = new KeybindSetting(new string[] { "LeftAlt", "None" });
        public KeybindSetting Sprint = new KeybindSetting(new string[] { "LeftShift", "None" });
        public KeybindSetting CoverNape1 = new KeybindSetting(new string[] { "Mouse2", "None" });
        public KeybindSetting AttackPunch = new KeybindSetting(new string[] { "Mouse0", "None" });
        public KeybindSetting AttackBellyFlop = new KeybindSetting(new string[] { "Mouse1", "None" });
        public KeybindSetting AttackSlapL = new KeybindSetting(new string[] { "Q", "None" });
        public KeybindSetting AttackSlapR = new KeybindSetting(new string[] { "E", "None" });
        public KeybindSetting AttackRockThrow = new KeybindSetting(new string[] { "R", "None" });
        public KeybindSetting AttackBiteL = new KeybindSetting(new string[] { "Alpha1", "None" });
        public KeybindSetting AttackBiteF = new KeybindSetting(new string[] { "Alpha2", "None" });
        public KeybindSetting AttackBiteR = new KeybindSetting(new string[] { "Alpha3", "None" });
        public KeybindSetting AttackHitFace = new KeybindSetting(new string[] { "Alpha4", "None" });
        public KeybindSetting AttackHitBack = new KeybindSetting(new string[] { "Alpha5", "None" });
        public KeybindSetting AttackSlam = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackStomp = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackSwing = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabAirFar = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabAir = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabBody = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabCore = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabGround = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabHead = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabHigh = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackSlapHighL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackSlapHighR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackSlapLowL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackSlapLowR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackBrushChest = new KeybindSetting(new string[] { "None", "None" });
    }
}
