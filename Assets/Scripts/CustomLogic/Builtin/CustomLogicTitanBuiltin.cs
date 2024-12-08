using Characters;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using System.Reflection;

namespace CustomLogic
{
    [CLType(Static = false)]
    class CustomLogicTitanBuiltin : CustomLogicCharacterBuiltin
    {
        public BasicTitan Titan;

        public CustomLogicTitanBuiltin(BasicTitan titan) : base(titan, "Titan")
        {
            Titan = titan;
        }

        [CLProperty("Gets or sets the size of the titan.")]
        public float Size
        {
            get => Titan.Size;
            set => Titan.SetSize(value);
        }

        [CLProperty("Gets or sets the run speed base of the titan.")]
        public float RunSpeedBase
        {
            get => Titan.RunSpeedBase;
            set => Titan.RunSpeedBase = value;
        }

        [CLProperty("Gets or sets the walk speed base of the titan.")]
        public float WalkSpeedBase
        {
            get => Titan.WalkSpeedBase;
            set => Titan.WalkSpeedBase = value;
        }

        [CLProperty("Gets or sets the walk speed per level of the titan.")]
        public float WalkSpeedPerLevel
        {
            get => Titan.WalkSpeedPerLevel;
            set => Titan.WalkSpeedPerLevel = value;
        }

        [CLProperty("Gets or sets the run speed per level of the titan.")]
        public float RunSpeedPerLevel
        {
            get => Titan.RunSpeedPerLevel;
            set => Titan.RunSpeedPerLevel = value;
        }

        [CLProperty("Gets or sets the turn speed of the titan.")]
        public float TurnSpeed
        {
            get => Titan.TurnSpeed;
            set => Titan.TurnSpeed = value;
        }

        [CLProperty("Gets or sets the rotate speed of the titan.")]
        public float RotateSpeed
        {
            get => Titan.RotateSpeed;
            set => Titan.RotateSpeed = value;
        }

        [CLProperty("Gets or sets the jump force of the titan.")]
        public float JumpForce
        {
            get => Titan.JumpForce;
            set => Titan.JumpForce = value;
        }

        [CLProperty("Gets or sets the action pause duration of the titan.")]
        public float ActionPause
        {
            get => Titan.ActionPause;
            set => Titan.ActionPause = value;
        }

        [CLProperty("Gets or sets the attack pause duration of the titan.")]
        public float AttackPause
        {
            get => Titan.AttackPause;
            set => Titan.AttackPause = value;
        }

        [CLProperty("Gets or sets the turn pause duration of the titan.")]
        public float TurnPause
        {
            get => Titan.TurnPause;
            set => Titan.TurnPause = value;
        }

        [CLProperty("Gets the nape position of the titan.")]
        public CustomLogicVector3Builtin NapePosition => new CustomLogicVector3Builtin(Titan.BasicCache.NapeHurtbox.transform.position);

        [CLProperty("Gets a value indicating whether the titan is a crawler.")]
        public bool IsCrawler => Titan.IsCrawler;

        [CLProperty("Gets or sets the detect range of the titan.")]
        public float DetectRange
        {
            get => Titan.IsMine() && Titan.AI ? Titan.GetComponent<BaseTitanAIController>().DetectRange : 0;
            set { if (Titan.IsMine() && Titan.AI) Titan.GetComponent<BaseTitanAIController>().SetDetectRange(value); }
        }

        [CLProperty("Gets or sets the focus range of the titan.")]
        public float FocusRange
        {
            get => Titan.IsMine() && Titan.AI ? Titan.GetComponent<BaseTitanAIController>().FocusRange : 0;
            set { if (Titan.IsMine() && Titan.AI) Titan.GetComponent<BaseTitanAIController>().FocusRange = value; }
        }

        [CLProperty("Gets or sets the focus time of the titan.")]
        public float FocusTime
        {
            get => Titan.IsMine() && Titan.AI ? Titan.GetComponent<BaseTitanAIController>().FocusTime : 0;
            set { if (Titan.IsMine() && Titan.AI) Titan.GetComponent<BaseTitanAIController>().FocusTime = value; }
        }

        [CLProperty("Gets or sets the far attack cooldown of the titan.")]
        public float FarAttackCooldown
        {
            get => Titan.IsMine() && Titan.AI ? Titan.GetComponent<BaseTitanAIController>().FarAttackCooldown : 0;
        }

        [CLProperty("Gets or sets the attack wait time of the titan.")]
        public float AttackWait
        {
            get => Titan.IsMine() && Titan.AI ? Titan.GetComponent<BaseTitanAIController>().AttackWait : 0;
        }

        [CLProperty("Gets or sets a value indicating whether the titan can run.")]
        public bool CanRun
        {
            get => Titan.IsMine() && Titan.AI ? Titan.GetComponent<BaseTitanAIController>().IsRun : false;
            set { if (Titan.IsMine() && Titan.AI) Titan.GetComponent<BaseTitanAIController>().IsRun = value; }
        }

        [CLProperty("Gets or sets the attack speed multiplier of the titan.")]
        public float AttackSpeedMultiplier
        {
            get => Titan.AttackSpeedMultiplier;
            set => Titan.AttackSpeedMultiplier = value;
        }

        [CLProperty("Gets or sets a value indicating whether pathfinding is used by the titan.")]
        public bool UsePathfinding
        {
            get => Titan.IsMine() && Titan.AI ? Titan.GetComponent<BaseTitanAIController>()._usePathfinding : false;
            set { if (Titan.IsMine() && Titan.AI) Titan.GetComponent<BaseTitanAIController>()._usePathfinding = value; }
        }

        [CLProperty("Gets the head mount transform of the titan.")]
        public CustomLogicTransformBuiltin HeadMount => new CustomLogicTransformBuiltin(Titan.BasicCache.Head);

        [CLProperty("Gets the neck mount transform of the titan.")]
        public CustomLogicTransformBuiltin NeckMount => new CustomLogicTransformBuiltin(Titan.BasicCache.Neck);

        [CLMethod("Moves the titan to the specified position.")]
        public void MoveTo(CustomLogicVector3Builtin position, float range, bool ignoreEnemies)
        {
            if (Titan.IsMine() && !Titan.Dead && Titan.AI)
                Titan.GetComponent<BaseTitanAIController>().MoveTo(position.Value, range, ignoreEnemies);
        }

        [CLMethod("Targets the specified enemy.")]
        public void Target(object enemyObj, float focus)
        {
            if (Titan.IsMine() && !Titan.Dead && Titan.AI)
            {
                ITargetable enemy = enemyObj is CustomLogicMapTargetableBuiltin mapTargetable
                                    ? mapTargetable.Value
                                    : ((CustomLogicCharacterBuiltin)enemyObj).Character;
                Titan.GetComponent<BaseTitanAIController>().SetEnemy(enemy, focus);
            }
        }

        [CLMethod("Forces the titan to idle for a specified duration.")]
        public void Idle(float time)
        {
            if (Titan.IsMine() && !Titan.Dead && Titan.AI)
                Titan.GetComponent<BaseTitanAIController>().ForceIdle(time);
        }

        [CLMethod("Cancels the titan's current order, causing it to wander.")]
        public void Wander()
        {
            if (Titan.IsMine() && !Titan.Dead && Titan.AI)
                Titan.GetComponent<BaseTitanAIController>().CancelOrder();
        }

        [CLMethod("Blinds the titan.")]
        public void Blind()
        {
            if (Titan.IsMine() && !Titan.Dead)
                Titan.Blind();
        }

        [CLMethod("Cripples the titan for a specified duration.")]
        public void Cripple(float time)
        {
            if (Titan.IsMine() && !Titan.Dead)
                Titan.Cripple(time);
        }

        [CLMethod("Makes the titan perform an emote.")]
        public void Emote(string emote)
        {
            if (Titan.IsMine() && !Titan.Dead)
                Titan.Emote(emote);
        }
    }
}
