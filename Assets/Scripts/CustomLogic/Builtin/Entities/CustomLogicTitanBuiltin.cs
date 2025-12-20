using Characters;
using Controllers;
using Map;

namespace CustomLogic
{
    /// <summary>
    /// Represents a Titan character.
    /// Only character owner can modify fields and call functions unless otherwise specified.
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

        [CLProperty("Titan's size.")]
        public float Size
        {
            get => Titan.Size;
            set { if (Titan.IsMine()) Titan.SetSize(value); }
        }

        [CLProperty("Titan's base run speed. Final run speed is RunSpeedBase + Size * RunSpeedPerLevel.")]
        public float RunSpeedBase
        {
            get => Titan.RunSpeedBase;
            set { if (Titan.IsMine()) Titan.RunSpeedBase = value; }
        }

        [CLProperty("Titan's base walk speed. Final walk speed is WalkSpeedBase + Size * WalkSpeedPerLevel.")]
        public float WalkSpeedBase
        {
            get => Titan.WalkSpeedBase;
            set { if (Titan.IsMine()) Titan.WalkSpeedBase = value; }
        }

        [CLProperty("Titan's walk speed added per size.")]
        public float WalkSpeedPerLevel
        {
            get => Titan.WalkSpeedPerLevel;
            set { if (Titan.IsMine()) Titan.WalkSpeedPerLevel = value; }
        }

        [CLProperty("Titan's run speed added per size.")]
        public float RunSpeedPerLevel
        {
            get => Titan.RunSpeedPerLevel;
            set { if (Titan.IsMine()) Titan.RunSpeedPerLevel = value; }
        }

        [CLProperty("Titan's turn animation speed.")]
        public float TurnSpeed
        {
            get => Titan.TurnSpeed;
            set { if (Titan.IsMine()) Titan.TurnSpeed = value; }
        }

        [CLProperty("Titan's rotate speed.")]
        public float RotateSpeed
        {
            get => Titan.RotateSpeed;
            set { if (Titan.IsMine()) Titan.RotateSpeed = value; }
        }

        [CLProperty("Titan's jump force.")]
        public float JumpForce
        {
            get => Titan.JumpForce;
            set { if (Titan.IsMine()) Titan.JumpForce = value; }
        }

        [CLProperty("Titan's pause delay after performing an action.")]
        public float ActionPause
        {
            get => Titan.ActionPause;
            set { if (Titan.IsMine()) Titan.ActionPause = value; }
        }

        [CLProperty("Titan's pause delay after performing an attack.")]
        public float AttackPause
        {
            get => Titan.AttackPause;
            set { if (Titan.IsMine()) Titan.AttackPause = value; }
        }

        [CLProperty("Titan's pause delay after performing a turn.")]
        public float TurnPause
        {
            get => Titan.TurnPause;
            set { if (Titan.IsMine()) Titan.TurnPause = value; }
        }

        [CLProperty("PT stamina.")]
        public float Stamina
        {
            get => Titan.CurrentSprintStamina;
            set { if (Titan.IsMine()) Titan.CurrentSprintStamina = value; }
        }

        [CLProperty("PT max stamina.")]
        public float MaxStamina
        {
            get => Titan.MaxSprintStamina;
            set { if (Titan.IsMine()) Titan.MaxSprintStamina = value; }
        }

        [CLProperty("The titan's nape position.")]
        public CustomLogicVector3Builtin NapePosition => Titan.BasicCache.NapeHurtbox.transform.position;

        [CLProperty("Is titan a crawler.")]
        public bool IsCrawler => Titan.IsCrawler;

        [CLProperty("(AI) titan's detect range.")]
        public float DetectRange
        {
            get => Titan.IsMine() && Titan.AI ? Controller.DetectRange : 0;
            set { if (Titan.IsMine() && Titan.AI) Controller.SetDetectRange(value); }
        }

        [CLProperty("(AI) titan's focus range.")]
        public float FocusRange
        {
            get => Titan.IsMine() && Titan.AI ? Controller.FocusRange : 0;
            set { if (Titan.IsMine() && Titan.AI) Controller.FocusRange = value; }
        }

        [CLProperty("(AI) titan's focus time.")]
        public float FocusTime
        {
            get => Titan.IsMine() && Titan.AI ? Controller.FocusTime : 0;
            set { if (Titan.IsMine() && Titan.AI) Controller.FocusTime = value; }
        }

        [CLProperty("(AI) Titan's cooldown after performing a ranged attack.")]
        public float FarAttackCooldown => Titan.IsMine() && Titan.AI ? Controller.FarAttackCooldown : 0;

        [CLProperty("(AI) Titan's wait time between being in range and performing an attack.")]
        public float AttackWait
        {
            get => Titan.IsMine() && Titan.AI ? Controller.AttackWait : 0;
            set { if (Titan.IsMine() && Titan.AI) Controller.AttackWait = value; }
        }

        [CLProperty("(AI) Titan can run or only walk.")]
        public bool CanRun
        {
            get => Titan.IsMine() && Titan.AI && Controller.IsRun;
            set { if (Titan.IsMine() && Titan.AI) Controller.IsRun = value; }
        }

        [CLProperty("Titan's attack animation speed.")]
        public float AttackSpeedMultiplier
        {
            get => Titan.IsMine() && Titan.AI ? Titan.AttackSpeedMultiplier : 0;
            set { if (Titan.IsMine() && Titan.AI) Titan.AttackSpeedMultiplier = value; }
        }

        [CLProperty("Determines whether the (AI) titan uses pathfinding. (Smart Movement in titan settings)")]
        public bool UsePathfinding
        {
            get => Titan.IsMine() && Titan.AI && Controller._usePathfinding;
            set { if (Titan.IsMine() && Titan.AI) Controller._usePathfinding = value; }
        }

        [CLProperty("Titan's head transform.")]
        public CustomLogicTransformBuiltin HeadMount => Titan.BasicCache.Head;

        [CLProperty("Titan's neck transform.")]
        public CustomLogicTransformBuiltin NeckMount => Titan.BasicCache.Neck;

        [CLMethod("Causes the (AI) titan to move towards a position and stopping when within specified range. If ignoreEnemies is true, will not engage enemies along the way.")]
        public void MoveTo(CustomLogicVector3Builtin position, float range, bool ignoreEnemies)
        {
            if (IsAlive() && Titan.AI)
                Controller.MoveTo(position.Value, range, ignoreEnemies);
        }

        [CLMethod("Causes the (AI) titan to target an enemy character or MapTargetable for focusTime seconds. If focusTime is 0 it will use the default focus time")]
        public void Target(object enemyObj, float focus)
        {
            if (IsAlive() && Titan.AI == false)
                return;

            ITargetable enemy = enemyObj is CustomLogicMapTargetableBuiltin mapTargetable
                                    ? mapTargetable.Value
                                    : ((CustomLogicCharacterBuiltin)enemyObj).Character;
            Controller.SetEnemy(enemy, focus);
        }

        [CLMethod("Gets the target currently focused by this character. Returns null if no target is set.")]
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

        [CLMethod("Causes the (AI) titan to idle for time seconds before beginning to wander. During idle the titan will not react or move at all.")]
        public void Idle(float time)
        {
            if (IsAlive() && Titan.AI)
                Controller.ForceIdle(time);
        }

        [CLMethod("Causes the (AI) titan to cancel any move commands and begin wandering randomly.")]
        public void Wander()
        {
            if (IsAlive() && Titan.AI)
                Controller.CancelOrder();
        }

        [CLMethod("Causes the titan to enter the blind state.")]
        public void Blind()
        {
            if (IsAlive())
                Titan.Blind();
        }

        [CLMethod("Causes the titan to enter the cripple state for time seconds. Using 0 will use the default cripple time.")]
        public void Cripple(float time)
        {
            if (IsAlive())
                Titan.Cripple(time);
        }

        private bool IsAlive() => Titan.IsMine() && Titan.Dead == false;
    }
}
