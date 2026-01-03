using Characters;
using Controllers;

namespace CustomLogic
{
    /// <code>
    /// function OnCharacterSpawn(character) {
    ///     if (character.IsMine &amp;&amp; character.Type == "WallColossal") {
    ///         character.Size = 2;
    ///         if (Network.MyPlayer.Status == "Alive" &amp;&amp; Network.MyPlayer.Character.Type == "Human") {
    ///             character.Target(Network.MyPlayer, 10);
    ///         }
    ///     }
    /// }
    /// </code>
    [CLType(Name = "WallColossal", Abstract = true, Description = "Represents a WallColossal character. Only character owner can modify fields and call functions unless otherwise specified.")]
    partial class CustomLogicWallColossalBuiltin : CustomLogicShifterBuiltin
    {
        public readonly WallColossalShifter Shifter;
        public readonly WallColossalAIController Controller;

        public CustomLogicWallColossalBuiltin(WallColossalShifter shifter) : base(shifter)
        {
            Shifter = shifter;
            Controller = shifter.GetComponent<WallColossalAIController>();
        }

        [CLProperty("Colossal's current hand health (applies to both hands for backward compatibility).")]
        public int HandHealth
        {
            get => (Shifter.CurrentLeftHandHealth + Shifter.CurrentRightHandHealth) / 2;
            set
            {
                if (Shifter.IsMine())
                {
                    Shifter.SetCurrentLeftHandHealth(value);
                    Shifter.SetCurrentRightHandHealth(value);
                }
            }
        }

        [CLProperty("Colossal's maximum hand health (applies to both hands for backward compatibility).")]
        public int MaxHandHealth
        {
            get => (Shifter.MaxLeftHandHealth + Shifter.MaxRightHandHealth) / 2;
            set
            {
                if (Shifter.IsMine())
                {
                    Shifter.SetMaxLeftHandHealth(value);
                    Shifter.SetMaxRightHandHealth(value);
                }
            }
        }

        [CLProperty("Colossal's current left hand health.")]
        public int LeftHandHealth
        {
            get => Shifter.CurrentLeftHandHealth;
            set
            {
                if (Shifter.IsMine())
                    Shifter.SetCurrentLeftHandHealth(value);
            }
        }

        [CLProperty("Colossal's maximum left hand health.")]
        public int MaxLeftHandHealth
        {
            get => Shifter.MaxLeftHandHealth;
            set
            {
                if (Shifter.IsMine())
                    Shifter.SetMaxLeftHandHealth(value);
            }
        }

        [CLProperty("Colossal's current right hand health.")]
        public int RightHandHealth
        {
            get => Shifter.CurrentRightHandHealth;
            set
            {
                if (Shifter.IsMine())
                    Shifter.SetCurrentRightHandHealth(value);
            }
        }

        [CLProperty("Colossal's maximum right hand health.")]
        public int MaxRightHandHealth
        {
            get => Shifter.MaxRightHandHealth;
            set
            {
                if (Shifter.IsMine())
                    Shifter.SetMaxRightHandHealth(value);
            }
        }

        [CLProperty("Colossal's left hand state.", Enum = typeof(CustomLogicHandStateEnum))]
        public string LeftHandState => Shifter.LeftHandState.ToString();

        [CLProperty("Colossal's right hand state.", Enum = typeof(CustomLogicHandStateEnum))]
        public string RightHandState => Shifter.RightHandState.ToString();

        [CLProperty("Time in seconds for a hand to fully recover from broken state.")]
        public float HandRecoveryTime
        {
            get => Shifter.HandRecoveryTime;
            set
            {
                if (Shifter.IsMine())
                    Shifter.HandRecoveryTime = value;
            }
        }

        [CLProperty("Time remaining in seconds for left hand to recover (0 if not recovering).")]
        public float LeftHandRecoveryTimeLeft
        {
            get => Shifter.LeftHandRecoveryTimeLeft;
            set
            {
                if (Shifter.IsMine())
                    Shifter.LeftHandRecoveryTimeLeft = value;
            }
        }

        [CLProperty("Time remaining in seconds for right hand to recover (0 if not recovering).")]
        public float RightHandRecoveryTimeLeft
        {
            get => Shifter.RightHandRecoveryTimeLeft;
            set
            {
                if (Shifter.IsMine())
                    Shifter.RightHandRecoveryTimeLeft = value;
            }
        }

        [CLProperty("Colossal's (AI) wall attack cooldown per attack.")]
        public float WallAttackCooldown
        {
            get => Controller.WallAttackCooldown;
            set
            {
                Controller.WallAttackCooldown = value;
            }
        }

        [CLProperty("Colossal's (AI) wall attack cooldown remaining.")]
        public float WallAttackCooldownLeft
        {
            get => Controller.WallAttackCooldownLeft;
            set
            {
                Controller.WallAttackCooldownLeft = value;
            }
        }

        [CLProperty("Colossal's current steam state.", Enum = typeof(CustomLogicSteamStateEnum))]
        public string SteamState => Shifter.SteamState.ToString();


        [CLMethod("Causes the colossal to perform steam attack.")]
        public void AttackSteam()
        {
            if (Shifter.IsMine() && !Shifter.Dead)
                Shifter.SteamAttack();
        }

        [CLMethod("Causes the colossal to stop steam attack.")]
        public void StopSteam()
        {
            if (Shifter.IsMine() && !Shifter.Dead)
                Shifter.StopSteam();
        }

        [CLMethod("Causes the (AI) colossal to perform a random wall attack.")]
        public void WallAttack()
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Controller.WallAttack();
        }
    }
}
