using Characters;
using Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Static = false)]
    class CustomLogicShifterBuiltin : CustomLogicCharacterBuiltin
    {
        public BaseShifter Shifter;

        public CustomLogicShifterBuiltin(BaseShifter shifter) : base(shifter, "Shifter")
        {
            Shifter = shifter;
        }

        [CLProperty("Gets or sets the size of the shifter.")]
        public float Size
        {
            get => Shifter.Size;
            set => Shifter.SetSize(value);
        }

        [CLProperty("Gets or sets the run speed base of the shifter.")]
        public float RunSpeedBase
        {
            get => Shifter.RunSpeedBase;
            set => Shifter.RunSpeedBase = value;
        }

        [CLProperty("Gets or sets the walk speed base of the shifter.")]
        public float WalkSpeedBase
        {
            get => Shifter.WalkSpeedBase;
            set => Shifter.WalkSpeedBase = value;
        }

        [CLProperty("Gets or sets the walk speed per level of the shifter.")]
        public float WalkSpeedPerLevel
        {
            get => Shifter.WalkSpeedPerLevel;
            set => Shifter.WalkSpeedPerLevel = value;
        }

        [CLProperty("Gets or sets the run speed per level of the shifter.")]
        public float RunSpeedPerLevel
        {
            get => Shifter.RunSpeedPerLevel;
            set => Shifter.RunSpeedPerLevel = value;
        }

        [CLProperty("Gets or sets the turn speed of the shifter.")]
        public float TurnSpeed
        {
            get => Shifter.TurnSpeed;
            set => Shifter.TurnSpeed = value;
        }

        [CLProperty("Gets or sets the rotate speed of the shifter.")]
        public float RotateSpeed
        {
            get => Shifter.RotateSpeed;
            set => Shifter.RotateSpeed = value;
        }

        [CLProperty("Gets or sets the jump force of the shifter.")]
        public float JumpForce
        {
            get => Shifter.JumpForce;
            set => Shifter.JumpForce = value;
        }

        [CLProperty("Gets or sets the action pause duration of the shifter.")]
        public float ActionPause
        {
            get => Shifter.ActionPause;
            set => Shifter.ActionPause = value;
        }

        [CLProperty("Gets or sets the attack pause duration of the shifter.")]
        public float AttackPause
        {
            get => Shifter.AttackPause;
            set => Shifter.AttackPause = value;
        }

        [CLProperty("Gets or sets the turn pause duration of the shifter.")]
        public float TurnPause
        {
            get => Shifter.TurnPause;
            set => Shifter.TurnPause = value;
        }

        [CLProperty("Gets the detect range of the shifter.")]
        public float DetectRange
        {
            get => Shifter.IsMine() && Shifter.AI ? Shifter.GetComponent<BaseTitanAIController>().DetectRange : 0;
            set { if (Shifter.IsMine() && Shifter.AI) Shifter.GetComponent<BaseTitanAIController>().SetDetectRange(value); }
        }

        [CLProperty("Gets the focus range of the shifter.")]
        public float FocusRange
        {
            get => Shifter.IsMine() && Shifter.AI ? Shifter.GetComponent<BaseTitanAIController>().FocusRange : 0;
            set { if (Shifter.IsMine() && Shifter.AI) Shifter.GetComponent<BaseTitanAIController>().FocusRange = value; }
        }

        [CLProperty("Gets the focus time of the shifter.")]
        public float FocusTime
        {
            get => Shifter.IsMine() && Shifter.AI ? Shifter.GetComponent<BaseTitanAIController>().FocusTime : 0;
            set { if (Shifter.IsMine() && Shifter.AI) Shifter.GetComponent<BaseTitanAIController>().FocusTime = value; }
        }

        [CLProperty("Gets the far attack cooldown of the shifter.")]
        public float FarAttackCooldown
        {
            get => Shifter.IsMine() && Shifter.AI ? Shifter.GetComponent<BaseTitanAIController>().FarAttackCooldown : 0;
        }

        [CLProperty("Gets the attack wait time of the shifter.")]
        public float AttackWait
        {
            get => Shifter.IsMine() && Shifter.AI ? Shifter.GetComponent<BaseTitanAIController>().AttackWait : 0;
        }

        [CLProperty("Gets or sets the attack speed multiplier of the shifter.")]
        public float AttackSpeedMultiplier
        {
            get => Shifter.AttackSpeedMultiplier;
            set => Shifter.AttackSpeedMultiplier = value;
        }

        [CLProperty("Gets or sets a value indicating whether pathfinding is used by the shifter.")]
        public bool UsePathfinding
        {
            get => Shifter.IsMine() && Shifter.AI ? Shifter.GetComponent<BaseTitanAIController>()._usePathfinding : false;
            set { if (Shifter.IsMine() && Shifter.AI) Shifter.GetComponent<BaseTitanAIController>()._usePathfinding = value; }
        }

        [CLProperty("Gets the nape position of the shifter.")]
        public CustomLogicVector3Builtin NapePosition => new CustomLogicVector3Builtin(Shifter.BaseTitanCache.NapeHurtbox.transform.position);

        [CLProperty("Gets or sets the death animation length of the shifter.")]
        public float DeathAnimLength
        {
            get => Shifter.DeathAnimationLength;
            set => Shifter.DeathAnimationLength = value;
        }

        [CLMethod("Moves the shifter to the specified position.")]
        public void MoveTo(CustomLogicVector3Builtin position, float range, bool ignoreEnemies)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Shifter.GetComponent<BaseTitanAIController>().MoveTo(position.Value, range, ignoreEnemies);
        }

        [CLMethod("Targets the specified enemy.")]
        public void Target(object enemyObj, float focus)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
            {
                ITargetable enemy = enemyObj is CustomLogicMapTargetableBuiltin mapTargetable
                                    ? mapTargetable.Value
                                    : ((CustomLogicCharacterBuiltin)enemyObj).Character;
                Shifter.GetComponent<BaseTitanAIController>().SetEnemy(enemy, focus);
            }
        }

        [CLMethod("Forces the shifter to idle for a specified duration.")]
        public void Idle(float time)
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Shifter.GetComponent<BaseTitanAIController>().ForceIdle(time);
        }

        [CLMethod("Cancels the shifter's current order, causing it to wander.")]
        public void Wander()
        {
            if (Shifter.IsMine() && !Shifter.Dead && Shifter.AI)
                Shifter.GetComponent<BaseTitanAIController>().CancelOrder();
        }

        [CLMethod("Blinds the shifter.")]
        public void Blind()
        {
            if (Shifter.IsMine() && !Shifter.Dead)
                Shifter.Blind();
        }

        [CLMethod("Cripples the shifter for a specified duration.")]
        public void Cripple(float time)
        {
            if (Shifter.IsMine() && !Shifter.Dead)
                Shifter.Cripple(time);
        }

        [CLMethod("Makes the shifter perform an emote.")]
        public void Emote(string emote)
        {
            if (Shifter.IsMine() && !Shifter.Dead)
                Shifter.Emote(emote);
        }
    }
}
