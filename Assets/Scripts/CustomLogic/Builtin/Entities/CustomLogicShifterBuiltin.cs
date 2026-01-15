using Characters;
using Controllers;

namespace CustomLogic
{
    /// <summary>
    /// Represents a Shifter character. Only character owner can modify fields and call functions unless otherwise specified.
    /// </summary>
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
    [CLType(Name = "Shifter", Abstract = true)]
    partial class CustomLogicShifterBuiltin : CustomLogicCharacterBuiltin
    {
        public readonly BaseShifter Shifter;
        public readonly BaseTitanAIController Controller;

        public CustomLogicShifterBuiltin(BaseShifter shifter) : base(shifter)
        {
            Shifter = shifter;
            Controller = shifter.GetComponent<BaseTitanAIController>();
        }

        /// <summary>
        /// Shifter's size.
        /// </summary>
        [CLProperty]
        public float Size
        {
            get => Shifter.Size;
            set { if (Shifter.IsMine()) Shifter.SetSize(value); }
        }

        /// <summary>
        /// Shifter's base run speed. Final run speed is RunSpeedBase + Size * RunSpeedPerLevel.
        /// </summary>
        [CLProperty]
        public float RunSpeedBase
        {
            get => Shifter.RunSpeedBase;
            set { if (Shifter.IsMine()) Shifter.RunSpeedBase = value; }
        }

        /// <summary>
        /// Shifter's base walk speed. Final walk speed is WalkSpeedBase + Size * WalkSpeedPerLevel.
        /// </summary>
        [CLProperty]
        public float WalkSpeedBase
        {
            get => Shifter.WalkSpeedBase;
            set { if (Shifter.IsMine()) Shifter.WalkSpeedBase = value; }
        }

        /// <summary>
        /// Shifter's walk speed added per level.
        /// </summary>
        [CLProperty]
        public float WalkSpeedPerLevel
        {
            get => Shifter.WalkSpeedPerLevel;
            set { if (Shifter.IsMine()) Shifter.WalkSpeedPerLevel = value; }
        }

        /// <summary>
        /// Shifter's run speed added per level.
        /// </summary>
        [CLProperty]
        public float RunSpeedPerLevel
        {
            get => Shifter.RunSpeedPerLevel;
            set { if (Shifter.IsMine()) Shifter.RunSpeedPerLevel = value; }
        }

        /// <summary>
        /// Shifter's turn speed when running turn animation.
        /// </summary>
        [CLProperty]
        public float TurnSpeed
        {
            get => Shifter.TurnSpeed;
            set { if (Shifter.IsMine()) Shifter.TurnSpeed = value; }
        }

        /// <summary>
        /// Shifter's rotate speed when rotating body.
        /// </summary>
        [CLProperty]
        public float RotateSpeed
        {
            get => Shifter.RotateSpeed;
            set { if (Shifter.IsMine()) Shifter.RotateSpeed = value; }
        }

        /// <summary>
        /// Shifter's jump force when jumping.
        /// </summary>
        [CLProperty]
        public float JumpForce
        {
            get => Shifter.JumpForce;
            set { if (Shifter.IsMine()) Shifter.JumpForce = value; }
        }

        /// <summary>
        /// Shifter's pause delay after performing an action.
        /// </summary>
        [CLProperty]
        public float ActionPause
        {
            get => Shifter.ActionPause;
            set { if (Shifter.IsMine()) Shifter.ActionPause = value; }
        }

        /// <summary>
        /// Shifter's pause delay after performing an attack.
        /// </summary>
        [CLProperty]
        public float AttackPause
        {
            get => Shifter.AttackPause;
            set { if (Shifter.IsMine()) Shifter.AttackPause = value; }
        }

        /// <summary>
        /// Shifter's pause delay after performing a turn.
        /// </summary>
        [CLProperty]
        public float TurnPause
        {
            get => Shifter.TurnPause;
            set { if (Shifter.IsMine()) Shifter.TurnPause = value; }
        }

        /// <summary>
        /// (AI) shifter's detect range.
        /// </summary>
        [CLProperty]
        public float DetectRange
        {
            get => Shifter.IsMine() && Shifter.AI ? Controller.DetectRange : 0;
            set { if (Shifter.IsMine() && Shifter.AI) Controller.SetDetectRange(value); }
        }

        /// <summary>
        /// (AI) shifter's focus range.
        /// </summary>
        [CLProperty]
        public float FocusRange
        {
            get => Shifter.IsMine() && Shifter.AI ? Controller.FocusRange : 0;
            set { if (Shifter.IsMine() && Shifter.AI) Controller.FocusRange = value; }
        }

        /// <summary>
        /// (AI) shifter's focus time before switching targets.
        /// </summary>
        [CLProperty]
        public float FocusTime
        {
            get => Shifter.IsMine() && Shifter.AI ? Controller.FocusTime : 0;
            set { if (Shifter.IsMine() && Shifter.AI) Controller.FocusTime = value; }
        }

        /// <summary>
        /// (AI) Shifter's cooldown after performing a ranged attack.
        /// </summary>
        [CLProperty]
        public float FarAttackCooldown => Shifter.IsMine() && Shifter.AI ? Controller.FarAttackCooldown : 0;

        /// <summary>
        /// (AI) Shifter's wait time between being in range to attack and performing the attack.
        /// </summary>
        [CLProperty]
        public float AttackWait => Shifter.IsMine() && Shifter.AI ? Controller.AttackWait : 0;

        /// <summary>
        /// Shifter's attack animation speed.
        /// </summary>
        [CLProperty]
        public float AttackSpeedMultiplier
        {
            get => Shifter.AttackSpeedMultiplier;
            set { if (Shifter.IsMine()) Shifter.AttackSpeedMultiplier = value; }
        }

        /// <summary>
        /// (AI) Shifter uses pathfinding.
        /// </summary>
        [CLProperty]
        public bool UsePathfinding
        {
            get => Shifter.IsMine() && Shifter.AI && Controller._usePathfinding;
            set { if (Shifter.IsMine() && Shifter.AI) Controller._usePathfinding = value; }
        }

        /// <summary>
        /// Enable/Disable AI Behavior (Shifter will not attack/target but pathfinding/move methods will still work).
        /// </summary>
        [CLProperty]
        public bool AIEnabled
        {
            get => Shifter.IsMine() && Shifter.AI && Controller.AIEnabled;
            set { if (Shifter.IsMine() && Shifter.AI) Controller.AIEnabled = value; }
        }

        /// <summary>
        /// The shifter's nape position.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin NapePosition => new CustomLogicVector3Builtin(Shifter.BaseTitanCache.NapeHurtbox.transform.position);

        /// <summary>
        /// The length of the death animation.
        /// </summary>
        [CLProperty]
        public float DeathAnimLength
        {
            get => Shifter.DeathAnimationLength;
            set { if (Shifter.IsMine()) Shifter.DeathAnimationLength = value; }
        }

        /// <summary>
        /// Causes the (AI) shifter to move towards a position. If ignoreEnemies is true, will not engage enemies along the way.
        /// </summary>
        /// <param name="position">The target position to move to.</param>
        /// <param name="range">The stopping range from the target position.</param>
        /// <param name="ignoreEnemies">If true, will not engage enemies along the way.</param>
        [CLMethod]
        public void MoveTo(CustomLogicVector3Builtin position, float range, bool ignoreEnemies)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Controller.MoveTo(position.Value, range, ignoreEnemies);
        }

        /// <summary>
        /// Causes the (AI) shifter to move towards a position. If ignoreEnemies is true, will not engage enemies along the way.
        /// </summary>
        /// <param name="position">The exact target position to move to.</param>
        /// <param name="timeoutPadding">The timeout padding in seconds (default: 1).</param>
        [CLMethod]
        public void MoveToExact(CustomLogicVector3Builtin position, float timeoutPadding = 1)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Controller.MoveToExact(position.Value, timeoutPadding);
        }

        /// <summary>
        /// Causes the (AI) shifter to move towards a position with a callback. The callback method will be called during movement and will receive this shifter as a parameter.
        /// </summary>
        /// <param name="method">The callback method to call during movement (receives this shifter as parameter).</param>
        /// <param name="position">The exact target position to move to.</param>
        /// <param name="range">The stopping range from the target position (default: 10).</param>
        /// <param name="timeoutPadding">The timeout padding in seconds (default: 1).</param>
        [CLMethod]
        public void MoveToExactCallback(UserMethod method, CustomLogicVector3Builtin position, float range = 10, float timeoutPadding = 1)
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

        /// <summary>
        /// Causes the (AI) shifter to target an enemy character or MapTargetable for focusTime seconds. If focusTime is 0 it will use the default focus time.
        /// </summary>
        /// <param name="enemyObj">The enemy to target (can be Character or MapTargetable).</param>
        /// <param name="focus">The focus time in seconds (0 uses default focus time).</param>
        [CLMethod]
        public void Target(object enemyObj, float focus)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
            {
                ITargetable enemy = enemyObj is CustomLogicMapTargetableBuiltin mapTargetable
                                    ? mapTargetable.Value
                                    : ((CustomLogicCharacterBuiltin)enemyObj).Character;
                Controller.SetEnemy(enemy, focus);
            }
        }

        /// <summary>
        /// Causes the (AI) shifter to idle for time seconds before beginning to wander. During idle the titan will not react or move at all.
        /// </summary>
        /// <param name="time">The idle time in seconds.</param>
        [CLMethod]
        public void Idle(float time)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Controller.ForceIdle(time);
        }

        /// <summary>
        /// Causes the (AI) shifter to cancel any move commands and begin wandering randomly.
        /// </summary>
        [CLMethod]
        public void Wander()
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Controller.CancelOrder();
        }

        /// <summary>
        /// Causes the shifter to enter the blind state.
        /// </summary>
        [CLMethod]
        public void Blind()
        {
            if (Shifter.IsMine() && !Shifter.Dead)
                Shifter.Blind();
        }

        /// <summary>
        /// Causes the shifter to enter the cripple state.
        /// </summary>
        /// <param name="time">The cripple duration in seconds.</param>
        [CLMethod]
        public void Cripple(float time)
        {
            if (Shifter.IsMine() && !Shifter.Dead)
                Shifter.Cripple(time);
        }

        /// <summary>
        /// Causes the shifter to perform the given attack, if able.
        /// </summary>
        /// <param name="attack">The name of the attack to perform.</param>
        [CLMethod]
        public void Attack(string attack)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.CanAttack())
                Shifter.Attack(attack);
        }
    }
}
