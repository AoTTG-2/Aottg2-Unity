using Characters;
using Controllers;
using Map;

namespace CustomLogic
{
    /// <summary>
    /// Represents a Titan character. Only character owner can modify fields and call functions unless otherwise specified.
    /// </summary>
    /// <code>
    /// function OnCharacterSpawn(character)
    /// {
    ///     if (character.IsMine &amp;&amp; character.Type == "Titan")
    ///     {
    ///         character.Size = 3;
    ///         character.DetectRange = 1000;
    ///         character.Blind();
    ///     }
    /// }
    /// </code>
    [CLType(Name = "Titan", Abstract = true)]
    partial class CustomLogicTitanBuiltin : CustomLogicCharacterBuiltin
    {
        public readonly BasicTitan Titan;
        public readonly BaseTitanAIController Controller;

        public CustomLogicTitanBuiltin(BasicTitan titan) : base(titan)
        {
            Titan = titan;
            Controller = Titan.GetComponent<BaseTitanAIController>();
        }

        /// <summary>
        /// Titan's size.
        /// </summary>
        [CLProperty]
        public float Size
        {
            get => Titan.Size;
            set { if (Titan.IsMine()) Titan.SetSize(value); }
        }

        /// <summary>
        /// Titan's base run speed. Final run speed is RunSpeedBase + Size * RunSpeedPerLevel.
        /// </summary>
        [CLProperty]
        public float RunSpeedBase
        {
            get => Titan.RunSpeedBase;
            set { if (Titan.IsMine()) Titan.RunSpeedBase = value; }
        }

        /// <summary>
        /// Titan's base walk speed. Final walk speed is WalkSpeedBase + Size * WalkSpeedPerLevel.
        /// </summary>
        [CLProperty]
        public float WalkSpeedBase
        {
            get => Titan.WalkSpeedBase;
            set { if (Titan.IsMine()) Titan.WalkSpeedBase = value; }
        }

        /// <summary>
        /// Titan's walk speed added per size.
        /// </summary>
        [CLProperty]
        public float WalkSpeedPerLevel
        {
            get => Titan.WalkSpeedPerLevel;
            set { if (Titan.IsMine()) Titan.WalkSpeedPerLevel = value; }
        }

        /// <summary>
        /// Titan's run speed added per size.
        /// </summary>
        [CLProperty]
        public float RunSpeedPerLevel
        {
            get => Titan.RunSpeedPerLevel;
            set { if (Titan.IsMine()) Titan.RunSpeedPerLevel = value; }
        }

        /// <summary>
        /// Titan's turn animation speed.
        /// </summary>
        [CLProperty]
        public float TurnSpeed
        {
            get => Titan.TurnSpeed;
            set { if (Titan.IsMine()) Titan.TurnSpeed = value; }
        }

        /// <summary>
        /// Titan's rotate speed.
        /// </summary>
        [CLProperty]
        public float RotateSpeed
        {
            get => Titan.RotateSpeed;
            set { if (Titan.IsMine()) Titan.RotateSpeed = value; }
        }

        /// <summary>
        /// Titan's jump force.
        /// </summary>
        [CLProperty]
        public float JumpForce
        {
            get => Titan.JumpForce;
            set { if (Titan.IsMine()) Titan.JumpForce = value; }
        }

        /// <summary>
        /// Titan's pause delay after performing an action.
        /// </summary>
        [CLProperty]
        public float ActionPause
        {
            get => Titan.ActionPause;
            set { if (Titan.IsMine()) Titan.ActionPause = value; }
        }

        /// <summary>
        /// Titan's pause delay after performing an attack.
        /// </summary>
        [CLProperty]
        public float AttackPause
        {
            get => Titan.AttackPause;
            set { if (Titan.IsMine()) Titan.AttackPause = value; }
        }

        /// <summary>
        /// Titan's pause delay after performing a turn.
        /// </summary>
        [CLProperty]
        public float TurnPause
        {
            get => Titan.TurnPause;
            set { if (Titan.IsMine()) Titan.TurnPause = value; }
        }

        /// <summary>
        /// PT stamina.
        /// </summary>
        [CLProperty]
        public float Stamina
        {
            get => Titan.CurrentSprintStamina;
            set { if (Titan.IsMine()) Titan.CurrentSprintStamina = value; }
        }

        /// <summary>
        /// PT max stamina.
        /// </summary>
        [CLProperty]
        public float MaxStamina
        {
            get => Titan.MaxSprintStamina;
            set { if (Titan.IsMine()) Titan.MaxSprintStamina = value; }
        }

        /// <summary>
        /// The titan's nape position.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin NapePosition => Titan.BasicCache.NapeHurtbox.transform.position;

        /// <summary>
        /// Is titan a crawler.
        /// </summary>
        [CLProperty]
        public bool IsCrawler => Titan.IsCrawler;

        /// <summary>
        /// (AI) titan's detect range.
        /// </summary>
        [CLProperty]
        public float DetectRange
        {
            get => Titan.IsMine() && Titan.AI ? Controller.DetectRange : 0;
            set { if (Titan.IsMine() && Titan.AI) Controller.SetDetectRange(value); }
        }

        /// <summary>
        /// (AI) titan's focus range.
        /// </summary>
        [CLProperty]
        public float FocusRange
        {
            get => Titan.IsMine() && Titan.AI ? Controller.FocusRange : 0;
            set { if (Titan.IsMine() && Titan.AI) Controller.FocusRange = value; }
        }

        /// <summary>
        /// (AI) titan's focus time.
        /// </summary>
        [CLProperty]
        public float FocusTime
        {
            get => Titan.IsMine() && Titan.AI ? Controller.FocusTime : 0;
            set { if (Titan.IsMine() && Titan.AI) Controller.FocusTime = value; }
        }

        /// <summary>
        /// (AI) Titan's cooldown after performing a ranged attack.
        /// </summary>
        [CLProperty]
        public float FarAttackCooldown => Titan.IsMine() && Titan.AI ? Controller.FarAttackCooldown : 0;

        /// <summary>
        /// (AI) Titan's wait time between being in range and performing an attack.
        /// </summary>
        [CLProperty]
        public float AttackWait
        {
            get => Titan.IsMine() && Titan.AI ? Controller.AttackWait : 0;
            set { if (Titan.IsMine() && Titan.AI) Controller.AttackWait = value; }
        }

        /// <summary>
        /// (AI) Titan can run or only walk.
        /// </summary>
        [CLProperty]
        public bool CanRun
        {
            get => Titan.IsMine() && Titan.AI && Controller.IsRun;
            set { if (Titan.IsMine() && Titan.AI) Controller.IsRun = value; }
        }

        /// <summary>
        /// Titan's attack animation speed.
        /// </summary>
        [CLProperty]
        public float AttackSpeedMultiplier
        {
            get => Titan.IsMine() && Titan.AI ? Titan.AttackSpeedMultiplier : 0;
            set { if (Titan.IsMine() && Titan.AI) Titan.AttackSpeedMultiplier = value; }
        }

        /// <summary>
        /// Determines whether the (AI) titan uses pathfinding. (Smart Movement in titan settings)
        /// </summary>
        [CLProperty]
        public bool UsePathfinding
        {
            get => Titan.IsMine() && Titan.AI && Controller._usePathfinding;
            set { if (Titan.IsMine() && Titan.AI) Controller._usePathfinding = value; }
        }

        /// <summary>
        /// Titan's head transform.
        /// </summary>
        [CLProperty]
        public CustomLogicTransformBuiltin HeadMount => Titan.BasicCache.Head;

        /// <summary>
        /// Titan's neck transform.
        /// </summary>
        [CLProperty]
        public CustomLogicTransformBuiltin NeckMount => Titan.BasicCache.Neck;

        /// <summary>
        /// Causes the (AI) titan to move towards a position and stopping when within specified range. If ignoreEnemies is true, will not engage enemies along the way.
        /// </summary>
        /// <param name="position">The target position to move to.</param>
        /// <param name="range">The stopping range from the target position.</param>
        /// <param name="ignoreEnemies">If true, will not engage enemies along the way.</param>
        [CLMethod]
        public void MoveTo(CustomLogicVector3Builtin position, float range, bool ignoreEnemies)
        {
            if (IsAlive() && Titan.AI)
                Controller.MoveTo(position.Value, range, ignoreEnemies);
        }

        /// <summary>
        /// Causes the (AI) titan to target an enemy character or MapTargetable for focusTime seconds. If focusTime is 0 it will use the default focus time.
        /// </summary>
        /// <param name="enemyObj">The enemy to target (can be Character or MapTargetable).</param>
        /// <param name="focus">The focus time in seconds (0 uses default focus time).</param>
        [CLMethod]
        public void Target(object enemyObj, float focus)
        {
            if (IsAlive() && Titan.AI == false)
                return;

            ITargetable enemy = enemyObj is CustomLogicMapTargetableBuiltin mapTargetable
                                    ? mapTargetable.Value
                                    : ((CustomLogicCharacterBuiltin)enemyObj).Character;
            Controller.SetEnemy(enemy, focus);
        }

        /// <summary>
        /// Gets the target currently focused by this character.
        /// </summary>
        /// <returns>Returns null if no target is set.</returns>
        [CLMethod]
        public object GetTarget()
        {
            if (IsAlive() && Titan.AI == false)
                return null;


            ITargetable enemy = Controller.GetEnemy();

            if (enemy == null)
                return null;

            if (enemy is CustomLogicMapTargetableBuiltin)
            {
                MapTargetable mapTargetable1 = (MapTargetable)enemy;
                return new CustomLogicMapTargetableBuiltin(mapTargetable1.GameObject, mapTargetable1);
            }
            else if (enemy is Human)
            {
                return new CustomLogicHumanBuiltin((Human)enemy);
            }
            else if (enemy is WallColossalShifter)
            {
                return new CustomLogicWallColossalBuiltin((WallColossalShifter)enemy);
            }
            else if (enemy is BaseShifter)
            {
                return new CustomLogicShifterBuiltin((BaseShifter)enemy);
            }
            else if (enemy is BasicTitan)
            {
                return new CustomLogicTitanBuiltin((BasicTitan)enemy);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Causes the (AI) titan to idle for time seconds before beginning to wander. During idle the titan will not react or move at all.
        /// </summary>
        /// <param name="time">The idle time in seconds.</param>
        [CLMethod]
        public void Idle(float time)
        {
            if (IsAlive() && Titan.AI)
                Controller.ForceIdle(time);
        }

        /// <summary>
        /// Causes the (AI) titan to cancel any move commands and begin wandering randomly.
        /// </summary>
        [CLMethod]
        public void Wander()
        {
            if (IsAlive() && Titan.AI)
                Controller.CancelOrder();
        }

        /// <summary>
        /// Causes the titan to enter the blind state.
        /// </summary>
        [CLMethod]
        public void Blind()
        {
            if (IsAlive())
                Titan.Blind();
        }

        /// <summary>
        /// Causes the titan to enter the cripple state for time seconds. Using 0 will use the default cripple time.
        /// </summary>
        /// <param name="time">The cripple duration in seconds (0 uses default time).</param>
        [CLMethod]
        public void Cripple(float time)
        {
            if (IsAlive())
                Titan.Cripple(time);
        }

        private new bool IsAlive() => Titan.IsMine() && Titan.Dead == false;
    }
}
