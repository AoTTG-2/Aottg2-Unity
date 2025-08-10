using UnityEngine;
using System.Collections.Generic;
using GameManagers;
using ApplicationManagers;

// Optimized radius-based detection between pairs of characters. An alternative to Unity physics sweep-and-prune.

namespace Characters
{
    class BaseDetection
    {
        public bool Detect;
        public BaseCharacter ClosestEnemy = null;
        public float ClosestEnemyDistance = 0f;
        public BaseCharacter Owner;
        private const float CriticalRange = 200f;
        private const float MinimumSpeed = 100f;
        private const float MediumDelay = 0.2f;
        private const float SlowDelay = 2f;
        private bool _enemiesOnly = false;
        private bool _titansOnly = false;
        private float _currentMediumTime = 0f;
        private float _currentSlowTime = 0f;
        private HashSet<BaseCharacter> _recalculateFast = new HashSet<BaseCharacter>();
        private HashSet<BaseCharacter> _recalculateMedium = new HashSet<BaseCharacter>();
        private HashSet<BaseCharacter> _recalculateSlow = new HashSet<BaseCharacter>();
        private HashSet<BaseCharacter> _tempRecalculateFast = new HashSet<BaseCharacter>();
        private HashSet<BaseCharacter> _tempRecalculateMedium = new HashSet<BaseCharacter>();
        private HashSet<BaseCharacter> _tempRecalculateSlow = new HashSet<BaseCharacter>();
        private InGameManager _inGameManager;

        public BaseDetection (BaseCharacter owner, bool enemiesOnly, bool titansOnly)
        {
            Owner = owner;
            _enemiesOnly = enemiesOnly;
            _titansOnly = titansOnly;
            _inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            foreach (var character in _inGameManager.GetAllCharactersEnumerable())
                OnCharacterSpawned(character);
            _inGameManager.Detections.Add(this);
            // randomize update times so every detection isn't running on the same frame
            _currentMediumTime += Random.Range(0f, 0.2f);
            _currentSlowTime += Random.Range(0f, 2f);
        }

        public virtual bool IsNullOrDead()
        {
            return Owner == null || Owner.Dead;
        }

        public void OnTeamChanged()
        {
            foreach (var character in _inGameManager.GetAllCharactersEnumerable())
                OnCharacterSpawned(character);
            foreach (var detection in _inGameManager.Detections)
                detection.OnCharacterSpawned(Owner);
        }

        public void OnCharacterSpawned(BaseCharacter character)
        {
            if (character == Owner || character == null || character.Dead)
                return;
            if (_titansOnly && !(character is BaseTitan))
                return;
            if (!IsValidTeam(character))
                return;
            _recalculateFast.Add(character);
        }

        private bool IsValidTeam(BaseCharacter character)
        {
            return !_enemiesOnly || Owner == null || !TeamInfo.SameTeam(character, Owner);
        }

        public void OnFixedUpdate()
        {
            if (ClosestEnemy != null && ClosestEnemy.Dead)
                ClosestEnemy = null; // reset ClosestEnemy if enemy is dead. Otherwise, ai will be in a daze for a while
            _currentMediumTime += Time.deltaTime;
            _currentSlowTime += Time.deltaTime;
            float mySpeed = GetSpeed();
            Vector3 myPosition = GetPosition();
            Recalculate(_recalculateFast, myPosition, mySpeed);
            _recalculateFast.Clear();
            if (_currentMediumTime > MediumDelay)
            {
                _currentMediumTime = 0f;
                Recalculate(_recalculateMedium, myPosition, mySpeed);
                _recalculateMedium.Clear();
            }
            if (_currentSlowTime > SlowDelay)
            {
                _currentSlowTime = 0f;
                Recalculate(_recalculateSlow, myPosition, mySpeed);
                _recalculateSlow.Clear();
            }
            MergeTemp(_recalculateFast, _tempRecalculateFast);
            MergeTemp(_recalculateMedium, _tempRecalculateMedium);
            MergeTemp(_recalculateSlow, _tempRecalculateSlow);
        }

        private void MergeTemp(HashSet<BaseCharacter> main, HashSet<BaseCharacter> temp)
        {
            if (temp.Count > 0)
            {
                foreach (var character in temp)
                    main.Add(character);
                temp.Clear();
            }
        }

        private void Recalculate(HashSet<BaseCharacter> characters, Vector3 myPosition, float mySpeed)
        {
            if (ClosestEnemy != null && !IsValidTeam(ClosestEnemy))
                ClosestEnemy = null;
            foreach (var character in characters)
            {
                if (character == null || character.Dead || !IsValidTeam(character))
                    continue;
                float distance = Vector3.Distance(myPosition, character.GetCenterPosition());
                float totalSpeed = mySpeed + character.CurrentSpeed + MinimumSpeed;
                float delay = (distance - CriticalRange) / totalSpeed;
                if (delay > SlowDelay)
                    _tempRecalculateSlow.Add(character);
                else if (delay > MediumDelay)
                    _tempRecalculateMedium.Add(character);
                else
                    _tempRecalculateFast.Add(character);
                if (ClosestEnemy == null || ClosestEnemy == character || distance < ClosestEnemyDistance)
                {
                    ClosestEnemy = character;
                    ClosestEnemyDistance = distance;
                }
                OnRecalculate(character, distance);
            }
        }

        protected virtual float GetSpeed()
        {
            return Owner.CurrentSpeed;
        }

        protected virtual Vector3 GetPosition()
        {
            return Owner.GetCenterPosition();
        }

        protected virtual void OnRecalculate(BaseCharacter character, float distance)
        {

        }
    }
}
