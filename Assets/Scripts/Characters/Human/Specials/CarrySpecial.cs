using ApplicationManagers;
using Characters;
using GameManagers;
using Settings;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class CarrySpecial : BaseHoldAttackSpecial
    {
        public const float DefaultCarryDistance = 25f;
        public const float DefaultGroundedCarryDistance = 10f;
        protected bool _needActivate;
        protected override float ActiveTime => 0.64f;
        protected float CarryDistance => DefaultCarryDistance;
        protected float GroundedCarryDistance => DefaultGroundedCarryDistance;
        public CarrySpecial(BaseCharacter owner) : base(owner)
        {
            Cooldown = 2f;
        }

        protected override void Activate()
        {
            _needActivate = true;
            var owner = (Human)_owner;
            if (owner.BackHuman != null)
            {
                IsActive = false;
                Deactivate();
                return;
            }
        }

        protected override void ActiveFixedUpdate()
        {
            base.ActiveFixedUpdate();

            // While held, get the list of targets within the carry distance, if cursor is over a target, highlight them in green
            // otherwise highlight the closest target in green.
            // Highlight non-selectable targets in red.

            var owner = (Human)_owner;
            if (owner.BackHuman != null)
            {
                IsActive = false;
                return;
            }

            float distance = owner.Grounded ? GroundedCarryDistance : CarryDistance;

            RaycastHit hit;
            Human target = owner.GetHumanAlongRay(owner.GetAimRayAfterHumanCheap(), distance);
            bool hasTarget = owner.IsValidCarryTarget(target, distance);

            float nearestDistance = float.PositiveInfinity;
            Human nearestHuman = null;
            InGameManager inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            foreach (Human carryTarget in inGameManager.Humans)
            {
                if (!owner.IsCarryable(carryTarget))
                {
                    carryTarget.RemoveOutline();
                    continue;
                }
                if (Vector3.Distance(owner.Cache.Transform.position, carryTarget.Cache.Transform.position) > distance)
                {
                    carryTarget.RemoveOutline();
                    continue;
                }
                if (!TeamInfo.SameTeam(carryTarget, owner.Team))
                {
                    carryTarget.AddVisibleOutlineWithColor(Color.red);
                    continue;
                    
                }
                if (carryTarget == target)
                    continue;
                carryTarget.AddVisibleOutlineWithColor(Color.white);
                float targetDistance = Vector3.Distance(owner.Cache.Transform.position, carryTarget.Cache.Transform.position);
                if (targetDistance < nearestDistance)
                {
                    nearestHuman = carryTarget;
                    nearestDistance = targetDistance;
                }
            }

            if (hasTarget)
            {
                target.AddVisibleOutlineWithColor(Color.green);
            }
            else
            {
                if (target != null)
                    target.AddVisibleOutlineWithColor(Color.red);
                if (nearestHuman != null)
                    nearestHuman.AddVisibleOutlineWithColor(Color.green);
            }

        }

        protected override void Deactivate()
        {
            var owner = (Human)_owner;
            if (owner.BackHuman != null)
            {
                owner.StopCarrySpecial();
                return;
            }
            float distance = owner.Grounded ? GroundedCarryDistance : CarryDistance;
            var target = owner.GetCarryOption(distance);
            if (target != null)
                owner.StartCarrySpecial(target);

            // reset skill cd
            SetCooldownLeft(0f);

            InGameManager inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            foreach (Human human in inGameManager.Humans)
            {
                human.RemoveOutline();
            }
        }
    }
}