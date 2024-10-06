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
        public KeybindSetting CoverNape = new KeybindSetting(new string[] { "WheelDown", "None" });
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
        public KeybindSetting AttackSwingL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackSwingR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabAirFarL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabAirFarR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabAirL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabAirR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabBackL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabBackR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabCoreL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabCoreR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabGroundBackL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabGroundBackR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabGroundFrontL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabGroundFrontR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabHeadBackL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabHeadBackR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabHeadFrontL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabHeadFrontR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabHighL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabHighR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabStomachL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabStomachR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackSlapHighL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackSlapHighR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackSlapLowL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackSlapLowR = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackBrushChestL = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackBrushChestR = new KeybindSetting(new string[] { "None", "None" });
    }
}
