namespace Characters
{
    class AnnieAnimations: BaseTitanAnimations
    {
        public override string Idle => "Armature_FemT|ft_idle";
        public override string Run => "Armature_FemT|ft_run";
        public override string Walk => "Armature_FemT|ft_run";
        public override string Kick => "Armature_FemT|ft_attack_sweep";
        public override string Die => "Armature_FemT|ft_die_shifter";
        public override string Stun => "Armature_FemT|ft_hit_titan";
        public override string Attack => "Armature_FemT|ft_attack_combo_full";
        public override string SitFall => "Armature_FemT|ft_legHurt";
        public override string SitIdle => "Armature_FemT|ft_legHurt_loop";
        public override string SitUp => "Armature_FemT|ft_legHurt_getup";
        //public override string Turn90L => "Armature_FemT|ft_turnaround2";
        //public override string Turn90R => "Armature_FemT|ft_turnaround1";
        public string AttackSwing = "Armature_FemT|ft_attack_front";
        public string AttackBrushBack = "Armature_FemT|ft_attack_sweep_back";
        public string AttackBrushFrontL = "Armature_FemT|ft_attack_sweep_front_left";
        public string AttackBrushFrontR = "Armature_FemT|ft_attack_sweep_front_right";
        public string AttackBrushHeadL = "Armature_FemT|ft_attack_sweep_head_b_l";
        public string AttackBrushHeadR = "Armature_FemT|ft_attack_sweep_head_b_r";
        public string AttackBite = "Armature_FemT|ft_attack_stomp";
        public string EmoteSalute = "Armature_FemT|ft_emote_salute";
        public string EmoteTaunt = "Armature_FemT|ft_emote_taunt";
        public string EmoteWave = "Armature_FemT|ft_emote_wave";
        public string EmoteRoar = "Armature_FemT|ft_mad1";
    }
}
