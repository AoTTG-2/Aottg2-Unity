using ApplicationManagers;
using Characters;
using GameManagers;
using Settings;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

namespace Characters
{
    class CarrySpecial : BaseHoldAttackSpecial
    {
        protected bool _needActivate;
        protected override float ActiveTime => 0.64f;
        protected float CarryDistance => 10f;
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


            RaycastHit hit;
            Human target = owner.GetHumanAlongRay(owner.GetAimRayAfterHumanCheap(), CarryDistance);
            bool hasTarget = owner.IsValidCarryTarget(target, CarryDistance);
            if (hasTarget)
            {
                target.AddVisibleOutlineWithColor(Color.green);
            }
            else if (target != null && target != owner)
            {
                target.AddVisibleOutlineWithColor(Color.red);
            }

            float nearestDistance = float.PositiveInfinity;
            Human nearestHuman = null;
            InGameManager inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            foreach (Human carryTarget in inGameManager.Humans)
            {
                if (!owner.IsCarryable(carryTarget) || carryTarget == target)
                {
                    continue;
                }
                if (Vector3.Distance(owner.Cache.Transform.position, carryTarget.Cache.Transform.position) > CarryDistance)
                {
                    carryTarget.RemoveOutline();
                    continue;
                }

                if (TeamInfo.SameTeam(carryTarget, owner.Team))
                {
                    carryTarget.AddVisibleOutlineWithColor(Color.white);
                }
                else
                {
                    carryTarget.AddVisibleOutlineWithColor(Color.red);
                }

                float targetDistance = Vector3.Distance(owner.Cache.Transform.position, carryTarget.Cache.Transform.position);
                if (targetDistance < nearestDistance)
                {
                    nearestHuman = carryTarget;
                    nearestDistance = targetDistance;
                }
            }

            if (nearestHuman != null & !hasTarget)
            {
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
            var target = owner.GetCarryOption(CarryDistance);
            if (target != null)
                owner.StartCarrySpecial(target);

            InGameManager inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            foreach (Human human in inGameManager.Humans)
            {
                human.RemoveOutline();
            }
        }
    }
}