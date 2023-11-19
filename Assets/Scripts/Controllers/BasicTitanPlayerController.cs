using Settings;
using Characters;
using UnityEngine;
using GameManagers;
using UI;
using CustomLogic;

namespace Controllers
{
    class BasicTitanPlayerController: BasePlayerController
    {
        protected BasicTitan _titan;
        protected TitanInputSettings _titanInput;
        protected float _enemyTimeLeft;

        protected override void Awake()
        {
            base.Awake();
            _titan = GetComponent<BasicTitan>();
            _titanInput = SettingsManager.InputSettings.Titan;
            _titan.RotateSpeed = 5f;
            _titan.RunSpeed = 24f;
            _titan.BellyFlopTime = 3f;
        }

        protected override void UpdateActionInput(bool inMenu)
        {
            base.UpdateActionInput(inMenu);
            if (inMenu)
                return;
            _titan.IsWalk = _titanInput.Walk.GetKey();
            _titan.IsSit = _titanInput.Sit.GetKey();
            _enemyTimeLeft -= Time.deltaTime;
            if (_enemyTimeLeft <= 0f)
            {
                _titan.TargetEnemy = GetClosestEnemy();
                _enemyTimeLeft = 1f;
            }
            if (_titan.CanAction())
            {
                if (_titanInput.Jump.GetKeyDown())
                {
                    if (_titan.HasDirection)
                        _titan.Jump(Vector3.up + _titan.Cache.Transform.forward);
                    else
                        _titan.Jump(Vector3.up);
                }
                else if (_titanInput.AttackPunch.GetKeyDown())
                    _titan.Attack(BasicTitanAttacks.AttackPunch);
                else if (_titanInput.AttackGrab.GetKeyDown())
                    _titan.Attack(BasicTitanAttacks.AttackGrab);
                else if (_titanInput.AttackSlap.GetKeyDown())
                    _titan.Attack(BasicTitanAttacks.AttackSlap);
                else if (_titanInput.AttackBody.GetKeyDown())
                    _titan.Attack(BasicTitanAttacks.AttackBellyFlop);
                else if (_titanInput.Kick.GetKeyDown())
                    _titan.Attack(BasicTitanAttacks.AttackKick);
            }
        }

        BaseCharacter GetClosestEnemy()
        {
            BaseCharacter closestChar = null;
            float closestDist = 200f;
            foreach (var character in _gameManager.GetAllCharacters())
            {
                if (!TeamInfo.SameTeam(_titan, character))
                {
                    float distance = Vector3.Distance(_titan.Cache.Transform.position, character.Cache.Transform.position);
                    if (distance < closestDist)
                    {
                        closestChar = character;
                        closestDist = distance;
                    }
                }
            }
            return closestChar;
        }
    }
}
