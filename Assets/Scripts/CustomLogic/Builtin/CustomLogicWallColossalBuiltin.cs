using Characters;
using Controllers;

namespace CustomLogic
{
    /// <summary>
    /// Only character owner can modify fields and call functions unless otherwise specified.
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
        [CLProperty(Description = "Colossal's current hand health.")]
        public int HandHealth
        {
            get => Shifter.CurrentHandHealth;
            set
            {
                if (Shifter.IsMine())
                    Shifter.SetCurrentHandHealth(value);
            }
        }

        [CLProperty(Description = "Colossal's maximum hand health.")]
        public int MaxHandHealth
        {
            get => Shifter.MaxHandHealth;
            set
            {
                if (Shifter.IsMine())
                    Shifter.SetMaxHandHealth(value);
            }
        }

        [CLProperty(Description = "Colossal's (AI) wall attack cooldown per attack.")]
        public float WallAttackCooldown
        {
            get => Controller.WallAttackCooldown;
            set
            {
                Controller.WallAttackCooldown = value;
            }
        }

        [CLProperty(Description = "Colossal's (AI) wall attack cooldown remaining.")]
        public float WallAttackCooldownLeft
        {
            get => Controller.WallAttackCooldownLeft;
            set
            {
                Controller.WallAttackCooldownLeft = value;
            }
        }

        [CLProperty(Description = "Colossal's current steam state (Off, Warning, or Damage).")]
        public string SteamState => Shifter._steamState.ToString();


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
