namespace Characters
{
    class AnnieAnimations: BaseTitanAnimations
    {
        public const string IdleValue = "Armature_FemT|ft_idle";
        public const string RunValue = "Armature_FemT|ft_run";
        public const string WalkValue = "Armature_FemT|ft_walk";
        public const string JumpValue = "Armature_FemT|ft_jump_start";
        public const string FallValue = "Armature_FemT|ft_jump_air";
        public const string LandValue = "Armature_FemT|ft_jump_land";
        public const string DieValue = "Armature_FemT|ft_die_shifter";
        public const string StunValue = "Armature_FemT|ft_hit_titan";
        public const string SitFallValue = "Armature_FemT|ft_legHurt";
        public const string SitIdleValue = "Armature_FemT|ft_legHurt_loop";
        public const string SitUpValue = "Armature_FemT|ft_legHurt_getup";
        public const string AttackComboValue = "Armature_FemT|ft_attack_combo_full";
        public const string AttackComboBlindValue = "Armature_FemT|ft_attack_combo_blind_full";
        public const string AttackSwingValue = "Armature_FemT|ft_attack_front";
        public const string AttackBrushBackValue = "Armature_FemT|ft_attack_sweep_back";
        public const string AttackBrushFrontLValue = "Armature_FemT|ft_attack_sweep_front_left";
        public const string AttackBrushFrontRValue = "Armature_FemT|ft_attack_sweep_front_right";
        public const string AttackBrushHeadLValue = "Armature_FemT|ft_attack_sweep_head_b_l";
        public const string AttackBrushHeadRValue = "Armature_FemT|ft_attack_sweep_head_b_r";
        public const string AttackGrabBottomLeftValue = "Armature_FemT|ft_attack_grab_bottom_left";
        public const string AttackGrabBottomRightValue = "Armature_FemT|ft_attack_grab_bottom_right";
        public const string AttackGrabMidLeftValue = "Armature_FemT|ft_attack_grab_mid_left";
        public const string AttackGrabMidRightValue = "Armature_FemT|ft_attack_grab_mid_right";
        public const string AttackGrabUpValue = "Armature_FemT|ft_attack_grab_up";
        public const string AttackGrabUpLeftValue = "Armature_FemT|ft_attack_grab_up_left";
        public const string AttackGrabUpRightValue = "Armature_FemT|ft_attack_grab_up_right";
        public const string AttackKickValue = "Armature_FemT|ft_attack_sweep";
        public const string AttackStompValue = "Armature_FemT|ft_attack_core";
        public const string AttackHeadValue = "Armature_FemT|ft_attack_head";
        public const string AttackBiteValue = "Armature_FemT|ft_attack_bite";
        public const string EmoteSaluteValue = "Armature_FemT|ft_emote_salute";
        public const string EmoteTauntValue = "Armature_FemT|ft_emote_taunt";
        public const string EmoteWaveValue = "Armature_FemT|ft_emote_wave";
        public const string EmoteRoarValue = "Armature_FemT|ft_mad1";
        
        public override string Idle => IdleValue;
        public override string Run => RunValue;
        public override string Walk => WalkValue;
        public override string Jump => JumpValue;
        public override string Fall => FallValue;
        public override string Land => LandValue;
        public override string Die => DieValue;
        public override string Stun => StunValue;
        public override string SitFall => SitFallValue;
        public override string SitIdle => SitIdleValue;
        public override string SitUp => SitUpValue;
        public string AttackCombo = AttackComboValue;
        public string AttackComboBlind = AttackComboBlindValue;
        public string AttackSwing = AttackSwingValue;
        public string AttackBrushBack = AttackBrushBackValue;
        public string AttackBrushFrontL = AttackBrushFrontLValue;
        public string AttackBrushFrontR = AttackBrushFrontRValue;
        public string AttackBrushHeadL = AttackBrushHeadLValue;
        public string AttackBrushHeadR = AttackBrushHeadRValue;
        public string AttackGrabBottomLeft = AttackGrabBottomLeftValue;
        public string AttackGrabBottomRight = AttackGrabBottomRightValue;
        public string AttackGrabMidLeft = AttackGrabMidLeftValue;
        public string AttackGrabMidRight = AttackGrabMidRightValue;
        public string AttackGrabUp = AttackGrabUpValue;
        public string AttackGrabUpLeft = AttackGrabUpLeftValue;
        public string AttackGrabUpRight = AttackGrabUpRightValue;
        public string AttackKick = AttackKickValue;
        public string AttackStomp = AttackStompValue;
        public string AttackHead = AttackHeadValue;
        public string AttackBite = AttackBiteValue;
        public string EmoteSalute = EmoteSaluteValue;
        public string EmoteTaunt = EmoteTauntValue;
        public string EmoteWave = EmoteWaveValue;
        public string EmoteRoar = EmoteRoarValue;
    }
}
