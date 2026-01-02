using Characters;
using Controllers;

namespace CustomLogic
{
    /// <code>
    /// function OnCharacterSpawn(character)
    /// {
    ///     if (character.IsMine &amp;&amp; character.Type == "Shifter")
    ///     {
    ///         character.Size = 2;
    ///         if (Network.MyPlayer.Status == "Alive" &amp;&amp; Network.MyPlayer.Character.Type == "Human")
    ///         {
    ///             character.Target(Network.MyPlayer, 10);
    ///         }
    ///     }
    /// }
    /// </code>
    [CLType(Name = "Shifter", Abstract = true, Description = "Represents a Shifter character. Only character owner can modify fields and call functions unless otherwise specified.")]
    partial class CustomLogicShifterBuiltin : CustomLogicCharacterBuiltin
    {
        public readonly BaseShifter Shifter;
        public readonly BaseTitanAIController Controller;

        public CustomLogicShifterBuiltin(BaseShifter shifter) : base(shifter)
        {
            Shifter = shifter;
            Controller = shifter.GetComponent<BaseTitanAIController>();
        }

        [CLProperty("Shifter's size.")]
        public float Size
        {
            get => Shifter.Size;
            set { if (Shifter.IsMine()) Shifter.SetSize(value); }
        }

        [CLProperty("Shifter's base run speed. Final run speed is RunSpeedBase + Size * RunSpeedPerLevel.")]
        public float RunSpeedBase
        {
            get => Shifter.RunSpeedBase;
            set { if (Shifter.IsMine()) Shifter.RunSpeedBase = value; }
        }

        [CLProperty("Shifter's base walk speed. Final walk speed is WalkSpeedBase + Size * WalkSpeedPerLevel.")]
        public float WalkSpeedBase
        {
            get => Shifter.WalkSpeedBase;
            set { if (Shifter.IsMine()) Shifter.WalkSpeedBase = value; }
        }

        [CLProperty("Shifter's walk speed added per level.")]
        public float WalkSpeedPerLevel
        {
            get => Shifter.WalkSpeedPerLevel;
            set { if (Shifter.IsMine()) Shifter.WalkSpeedPerLevel = value; }
        }

        [CLProperty("Shifter's run speed added per level.")]
        public float RunSpeedPerLevel
        {
            get => Shifter.RunSpeedPerLevel;
            set { if (Shifter.IsMine()) Shifter.RunSpeedPerLevel = value; }
        }

        [CLProperty("Shifter's turn speed when running turn animation.")]
        public float TurnSpeed
        {
            get => Shifter.TurnSpeed;
            set { if (Shifter.IsMine()) Shifter.TurnSpeed = value; }
        }

        [CLProperty("Shifter's rotate speed when rotating body.")]
        public float RotateSpeed
        {
            get => Shifter.RotateSpeed;
            set { if (Shifter.IsMine()) Shifter.RotateSpeed = value; }
        }

        [CLProperty("Shifter's jump force when jumping.")]
        public float JumpForce
        {
            get => Shifter.JumpForce;
            set { if (Shifter.IsMine()) Shifter.JumpForce = value; }
        }

        [CLProperty("Shifter's pause delay after performing an action.")]
        public float ActionPause
        {
            get => Shifter.ActionPause;
            set { if (Shifter.IsMine()) Shifter.ActionPause = value; }
        }

        [CLProperty("Shifter's pause delay after performing an attack.")]
        public float AttackPause
        {
            get => Shifter.AttackPause;
            set { if (Shifter.IsMine()) Shifter.AttackPause = value; }
        }

        [CLProperty("Shifter's pause delay after performing a turn.")]
        public float TurnPause
        {
            get => Shifter.TurnPause;
            set { if (Shifter.IsMine()) Shifter.TurnPause = value; }
        }

        [CLProperty("(AI) shifter's detect range.")]
        public float DetectRange
        {
            get => Shifter.IsMine() && Shifter.AI ? Controller.DetectRange : 0;
            set { if (Shifter.IsMine() && Shifter.AI) Controller.SetDetectRange(value); }
        }

        [CLProperty("(AI) shifter's focus range.")]
        public float FocusRange
        {
            get => Shifter.IsMine() && Shifter.AI ? Controller.FocusRange : 0;
            set { if (Shifter.IsMine() && Shifter.AI) Controller.FocusRange = value; }
        }

        [CLProperty("(AI) shifter's focus time before switching targets.")]
        public float FocusTime
        {
            get => Shifter.IsMine() && Shifter.AI ? Controller.FocusTime : 0;
            set { if (Shifter.IsMine() && Shifter.AI) Controller.FocusTime = value; }
        }

        [CLProperty("(AI) Shifter's cooldown after performing a ranged attack.")]
        public float FarAttackCooldown => Shifter.IsMine() && Shifter.AI ? Controller.FarAttackCooldown : 0;

        [CLProperty("(AI) Shifter's wait time between being in range to attack and performing the attack.")]
        public float AttackWait => Shifter.IsMine() && Shifter.AI ? Controller.AttackWait : 0;

        [CLProperty("Shifter's attack animation speed.")]
        public float AttackSpeedMultiplier
        {
            get => Shifter.AttackSpeedMultiplier;
            set { if (Shifter.IsMine()) Shifter.AttackSpeedMultiplier = value; }
        }

        [CLProperty("(AI) Shifter uses pathfinding.")]
        public bool UsePathfinding
        {
            get => Shifter.IsMine() && Shifter.AI && Controller._usePathfinding;
            set { if (Shifter.IsMine() && Shifter.AI) Controller._usePathfinding = value; }
        }

        [CLProperty("Enable/Disable AI Behavior (Shifter will not attack/target but pathfinding/move methods will still work).")]
        public bool AIEnabled
        {
            get => Shifter.IsMine() && Shifter.AI && Controller.AIEnabled;
            set { if (Shifter.IsMine() && Shifter.AI) Controller.AIEnabled = value; }
        }

        [CLProperty("The shifter's nape position.")]
        public CustomLogicVector3Builtin NapePosition => new CustomLogicVector3Builtin(Shifter.BaseTitanCache.NapeHurtbox.transform.position);

        [CLProperty("The length of the death animation.")]
        public float DeathAnimLength
        {
            get => Shifter.DeathAnimationLength;
            set { if (Shifter.IsMine()) Shifter.DeathAnimationLength = value; }
        }

        [CLMethod("Causes the (AI) shifter to move towards a position. If ignoreEnemies is true, will not engage enemies along the way.")]
        public void MoveTo(
            [CLParam("The target position to move to.")]
            CustomLogicVector3Builtin position,
            [CLParam("The stopping range from the target position.")]
            float range,
            [CLParam("If true, will not engage enemies along the way.")]
            bool ignoreEnemies)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Controller.MoveTo(position.Value, range, ignoreEnemies);
        }

        [CLMethod("Causes the (AI) shifter to move towards a position. If ignoreEnemies is true, will not engage enemies along the way.")]
        public void MoveToExact(
            [CLParam("The exact target position to move to.")]
            CustomLogicVector3Builtin position,
            [CLParam("The timeout padding in seconds (default: 1).")]
            float timeoutPadding = 1)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Controller.MoveToExact(position.Value, timeoutPadding);
        }

        [CLMethod("Causes the (AI) shifter to move towards a position with a callback. The callback method will be called during movement and will receive this shifter as a parameter.")]
        public void MoveToExactCallback(
            [CLParam("The callback method to call during movement (receives this shifter as parameter).")]
            UserMethod method,
            [CLParam("The exact target position to move to.")]
            CustomLogicVector3Builtin position,
            [CLParam("The stopping range from the target position (default: 10).")]
            float range = 10,
            [CLParam("The timeout padding in seconds (default: 1).")]
            float timeoutPadding = 1)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
            {
                Controller.MoveToExactCallback(
                    () => CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { this }),
                    position.Value,
                    range,
                    timeoutPadding);
            }
        }

        [CLMethod("Causes the (AI) shifter to target an enemy character or MapTargetable for focusTime seconds. If focusTime is 0 it will use the default focus time.")]
        public void Target(
            [CLParam("The enemy to target (can be Character or MapTargetable).", Type = "Character|MapTargetable")]
            object enemyObj,
            [CLParam("The focus time in seconds (0 uses default focus time).")]
            float focus)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
            {
                ITargetable enemy = enemyObj is CustomLogicMapTargetableBuiltin mapTargetable
                                    ? mapTargetable.Value
                                    : ((CustomLogicCharacterBuiltin)enemyObj).Character;
                Controller.SetEnemy(enemy, focus);
            }
        }

        [CLMethod("Causes the (AI) shifter to idle for time seconds before beginning to wander. During idle the titan will not react or move at all.")]
        public void Idle(
            [CLParam("The idle time in seconds.")]
            float time)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Controller.ForceIdle(time);
        }

        [CLMethod("Causes the (AI) shifter to cancel any move commands and begin wandering randomly.")]
        public void Wander()
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Controller.CancelOrder();
        }

        [CLMethod("Causes the shifter to enter the blind state.")]
        public void Blind()
        {
            if (Shifter.IsMine() && !Shifter.Dead)
                Shifter.Blind();
        }

        [CLMethod("Causes the shifter to enter the cripple state.")]
        public void Cripple(
            [CLParam("The cripple duration in seconds.")]
            float time)
        {
            if (Shifter.IsMine() && !Shifter.Dead)
                Shifter.Cripple(time);
        }

        [CLMethod("Causes the shifter to perform the given attack, if able.")]
        public void Attack(
            [CLParam("The name of the attack to perform.")]
            string attack)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.CanAttack())
                Shifter.Attack(attack);
        }
    }
}
