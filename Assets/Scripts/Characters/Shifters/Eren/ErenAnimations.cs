namespace Characters
{
    class ErenAnimations : BaseTitanAnimations
    {
        public const string IdleValue = "ErenRig_VER2|et_idle";
        public const string RunValue = "ErenRig_VER2|et_run";
        public const string WalkValue = "ErenRig_VER2|et_walk";
        public const string JumpValue = "ErenRig_VER2|et_jump_start";
        public const string FallValue = "ErenRig_VER2|et_jump_air";
        public const string LandValue = "ErenRig_VER2|et_jump_land";
        public const string DieValue = "ErenRig_VER2|et_die";
        public const string AttackComboValue = "ErenRig_VER2|et_attack_combo_full";
        public const string AttackKickValue = "ErenRig_VER2|et_attack_kick";
        public const string StunValue = "ErenRig_VER2|et_hit_titan";
        public const string EmoteNodValue = "ErenRig_VER2|et_yes";
        public const string EmoteRoarValue = "ErenRig_VER2|et_born";
        public const string RockLiftValue = "ErenRig_VER2|et_rock_lift";
        public const string RockLift001Value = "ErenRig_VER2|et_rock_lift.001";
        public const string RockWalkValue = "ErenRig_VER2|et_rock_walk";
        public const string RockFixHoleValue = "ErenRig_VER2|et_rock_fix_hole";
        
        public override string Idle => IdleValue;
        public override string Run => RunValue;
        public override string Walk => WalkValue;
        public override string Jump => JumpValue;
        public override string Fall => FallValue;
        public override string Land => LandValue;
        public override string Die => DieValue;
        public string AttackCombo = AttackComboValue;
        public string AttackKick = AttackKickValue;
        public override string Stun => StunValue;
        public string EmoteNod = EmoteNodValue;
        public string EmoteRoar = EmoteRoarValue;
        public string RockLift = RockLiftValue;
        public string RockLift001 = RockLift001Value;
        public string RockWalk = RockWalkValue;
        public string RockFixHole = RockFixHoleValue;
    }
}
