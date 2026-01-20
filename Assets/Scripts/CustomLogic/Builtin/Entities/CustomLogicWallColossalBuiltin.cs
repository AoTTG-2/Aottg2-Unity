using System;
using Characters;
using Controllers;

namespace CustomLogic
{
    /// <summary>
    /// Represents a WallColossal character. Only character owner can modify fields and call functions unless otherwise specified.
    /// </summary>
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
    [CLType(Name = "WallColossal", Abstract = true)]
    partial class CustomLogicWallColossalBuiltin : CustomLogicShifterBuiltin
    {
        public readonly WallColossalShifter Shifter;
        public readonly WallColossalAIController Controller;

        public CustomLogicWallColossalBuiltin(WallColossalShifter shifter) : base(shifter)
        {
            Shifter = shifter;
            Controller = shifter.GetComponent<WallColossalAIController>();
        }

        /// <summary>
        /// Colossal's current hand health (applies to both hands for backward compatibility).
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Colossal's maximum hand health (applies to both hands for backward compatibility).
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Colossal's current left hand health.
        /// </summary>
        [CLProperty]
        public int LeftHandHealth
        {
            get => Shifter.CurrentLeftHandHealth;
            set
            {
                if (Shifter.IsMine())
                    Shifter.SetCurrentLeftHandHealth(value);
            }
        }

        /// <summary>
        /// Colossal's maximum left hand health.
        /// </summary>
        [CLProperty]
        public int MaxLeftHandHealth
        {
            get => Shifter.MaxLeftHandHealth;
            set
            {
                if (Shifter.IsMine())
                    Shifter.SetMaxLeftHandHealth(value);
            }
        }

        /// <summary>
        /// Colossal's current right hand health.
        /// </summary>
        [CLProperty]
        public int RightHandHealth
        {
            get => Shifter.CurrentRightHandHealth;
            set
            {
                if (Shifter.IsMine())
                    Shifter.SetCurrentRightHandHealth(value);
            }
        }

        /// <summary>
        /// Colossal's maximum right hand health.
        /// </summary>
        [CLProperty]
        public int MaxRightHandHealth
        {
            get => Shifter.MaxRightHandHealth;
            set
            {
                if (Shifter.IsMine())
                    Shifter.SetMaxRightHandHealth(value);
            }
        }

        /// <summary>
        /// Colossal's left hand state.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicHandStateEnum) })]
        public int LeftHandState => (int)Shifter.LeftHandState;

        /// <summary>
        /// Colossal's right hand state.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicHandStateEnum) })]
        public int RightHandState => (int)Shifter.RightHandState;

        /// <summary>
        /// Time in seconds for a hand to fully recover from broken state.
        /// </summary>
        [CLProperty]
        public float HandRecoveryTime
        {
            get => Shifter.HandRecoveryTime;
            set
            {
                if (Shifter.IsMine())
                    Shifter.HandRecoveryTime = value;
            }
        }

        /// <summary>
        /// Time remaining in seconds for left hand to recover (0 if not recovering).
        /// </summary>
        [CLProperty]
        public float LeftHandRecoveryTimeLeft
        {
            get => Shifter.LeftHandRecoveryTimeLeft;
            set
            {
                if (Shifter.IsMine())
                    Shifter.LeftHandRecoveryTimeLeft = value;
            }
        }

        /// <summary>
        /// Time remaining in seconds for right hand to recover (0 if not recovering).
        /// </summary>
        [CLProperty]
        public float RightHandRecoveryTimeLeft
        {
            get => Shifter.RightHandRecoveryTimeLeft;
            set
            {
                if (Shifter.IsMine())
                    Shifter.RightHandRecoveryTimeLeft = value;
            }
        }

        /// <summary>
        /// Colossal's (AI) wall attack cooldown per attack.
        /// </summary>
        [CLProperty]
        public float WallAttackCooldown
        {
            get => Controller.WallAttackCooldown;
            set
            {
                Controller.WallAttackCooldown = value;
            }
        }

        /// <summary>
        /// Colossal's (AI) wall attack cooldown remaining.
        /// </summary>
        [CLProperty]
        public float WallAttackCooldownLeft
        {
            get => Controller.WallAttackCooldownLeft;
            set
            {
                Controller.WallAttackCooldownLeft = value;
            }
        }

        /// <summary>
        /// Colossal's current steam state.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicSteamStateEnum) })]
        public int SteamState => (int)Shifter.SteamState;


        /// <summary>
        /// Causes the colossal to perform steam attack.
        /// </summary>
        [CLMethod]
        public void AttackSteam()
        {
            if (Shifter.IsMine() && !Shifter.Dead)
                Shifter.SteamAttack();
        }

        /// <summary>
        /// Causes the colossal to stop steam attack.
        /// </summary>
        [CLMethod]
        public void StopSteam()
        {
            if (Shifter.IsMine() && !Shifter.Dead)
                Shifter.StopSteam();
        }

        /// <summary>
        /// Causes the (AI) colossal to perform a random wall attack.
        /// </summary>
        [CLMethod]
        public void WallAttack()
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Controller.WallAttack();
        }
    }
}
