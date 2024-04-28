namespace Characters
{
    class ErenAnimations: BaseTitanAnimations
    {
        public override string Idle => "ErenRig_VER2|et_idle";
        public override string Run => "ErenRig_VER2|et_run";
        public override string Walk => "ErenRig_VER2|et_walk";
        public override string Jump => "ErenRig_VER2|et_jump_start";
        public override string Fall => "ErenRig_VER2|et_jump_air";
        public override string Land => "ErenRig_VER2|et_jump_land";
        public override string Die => "ErenRig_VER2|et_die";
        public string AttackCombo = "ErenRig_VER2|et_attack_combo_full";
        public string AttackKick = "ErenRig_VER2|et_attack_kick";
        public override string Stun => "ErenRig_VER2|et_hit_titan";
        public string EmoteNod = "ErenRig_VER2|et_yes";
        public string EmoteRoar = "ErenRig_VER2|et_born";
    }
}
