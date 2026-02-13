namespace Characters
{
    class WallColossalAnimations: BaseTitanAnimations
    {
        public const string IdleValue = "Armature_VER2|wall_colossal_idle";
        public const string AttackWallSlap1LValue = "Armature_VER2|wall_attack_slap1_L";
        public const string AttackWallSlap1RValue = "Armature_VER2|wall_attack_slap1_R";
        public const string AttackWallSlap2LValue = "Armature_VER2|wall_attack_slap2_L";
        public const string AttackWallSlap2RValue = "Armature_VER2|wall_attack_slap2_R";
        public const string AttackSteamValue = "Armature_VER2|wall_attack_steam";
        public const string AttackSweepValue = "Armature_VER2|wall_attack_sweep";
        public const string AttackKickValue = "Armature_VER2|wall_colossal_kick";
        public const string StunFallFace = "Armature_VER2|wall_colossal_stun";

        public override string Idle => IdleValue;
        public string AttackWallSlap1L = AttackWallSlap1LValue;
        public string AttackWallSlap1R = AttackWallSlap1RValue;
        public string AttackWallSlap2L = AttackWallSlap2LValue;
        public string AttackWallSlap2R = AttackWallSlap2RValue;
        public string AttackSteam = AttackSteamValue;
        public string AttackSweep = AttackSweepValue;
        public string AttackKick = AttackKickValue;
        public string StunFallFaceAnim = StunFallFace;
    }
}
