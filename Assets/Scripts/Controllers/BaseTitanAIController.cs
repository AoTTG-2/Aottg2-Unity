using Settings;
using Characters;
using UnityEngine;
using System.Collections.Generic;
using SimpleJSONFixed;
using Utility;
using GameManagers;
using UnityEngine.UIElements;

namespace Controllers
{
    class BaseTitanAIController : BaseAIController
    {
        protected BaseTitan _titan;
        public TitanAIState AIState = TitanAIState.Idle;
        public bool SmartAttack = false;
        public float DetectRange;
        public float CloseAttackRange;
        public float FarAttackRange;
        public float FarAttackCooldown;
        public float FocusRange;
        public float FocusTime;
        public float AttackWait;
        public float ChaseAngleTimeMin;
        public float ChaseAngleTimeMax;
        public float ChaseAngleMinRange;
        public bool IsRun;
        public bool IsTurn;
        public float TurnAngle;
        protected Vector3 _moveToPosition;
        protected float _moveAngle;
        protected bool _moveToActive;
        protected float _moveToRange;
        protected bool _moveToIgnoreEnemies;
        public Dictionary<string, float> AttackChances = new Dictionary<string, float>();
        public Dictionary<string, string> AttackGroups = new Dictionary<string, string>();
        public Dictionary<string, TitanAttackInfo> AttackInfos;
        protected float _stateTimeLeft;
        protected float _focusTimeLeft;
        protected float _rangedCooldownLeft;
        protected float _attackRange;
        protected BaseCharacter _enemy;
        protected AICharacterDetection _detection;
        protected string _attack;
        protected float _attackCooldownLeft;
        protected float _waitAttackTime;
        private CapsuleCollider _mainCollider;

        // pathing
        private bool _collidedLastFrame = false;
        private readonly int _sampleRayCount = 6;
        private readonly float _sampleRayRange = 120f;
        private readonly float _targetWeight = 1f;
        private readonly float _collisionWeight = 140f;
        private float _collisionAvoidDistance => this._attackRange * 2; // 100f;
        private float _collisionDetectionDistance => this._attackRange * 2;
        private readonly bool _useCollisionAvoidance = true;

        protected override void Awake()
        {
            base.Awake();
            _titan = GetComponent<BaseTitan>();
            _mainCollider = _titan.GetComponent<CapsuleCollider>();
        }

        protected override void Start()
        {
            Idle();
        }

        public void MoveTo(Vector3 position, float range, bool ignore)
        {
            _moveToPosition = position;
            _moveToActive = true;
            _moveToRange = range;
            _moveToIgnoreEnemies = ignore;
        }

        public void CancelOrder()
        {
            _moveToActive = false;
            if (AIState == TitanAIState.ForcedIdle)
                Idle();
            _enemy = null;
        }

        public void ForceIdle(float time)
        {
            Idle();
            AIState = TitanAIState.ForcedIdle;
            _stateTimeLeft = time;
        }

        public virtual void Init(JSONNode data)
        {
            DetectRange = data["DetectRange"].AsFloat;
            CloseAttackRange = data["CloseAttackRange"].AsFloat;
            FarAttackRange = data["FarAttackRange"].AsFloat;
            FarAttackCooldown = data["FarAttackCooldown"].AsFloat;
            FocusRange = data["FocusRange"].AsFloat;
            FocusTime = data["FocusTime"].AsFloat;
            AttackWait = data["AttackWait"].AsFloat;
            ChaseAngleTimeMin = data["ChaseAngleTimeMin"].AsFloat;
            ChaseAngleTimeMax = data["ChaseAngleTimeMax"].AsFloat;
            ChaseAngleMinRange = data["ChaseAngleMinRange"].AsFloat;
            IsRun = data["IsRun"].AsBool;
            IsTurn = data["IsTurn"].AsBool;
            TurnAngle = data["TurnAngle"].AsFloat;
            AttackInfos = CharacterData.TitanAttackInfos[data["Type"].Value];
            foreach (string attack in data["Attacks"].Keys)
            {
                float chance = data["Attacks"][attack];
                if (attack.EndsWith("*"))
                {
                    string prefix = attack.Substring(0, attack.Length - 1);
                    foreach (string anyAttack in AttackInfos.Keys)
                    {
                        if (anyAttack.StartsWith(prefix))
                        {
                            AttackChances.Add(anyAttack, chance);
                            AttackGroups.Add(anyAttack, prefix);
                        }
                    }
                }
                else
                    AttackChances.Add(attack, chance);
            }
            _detection = AICharacterDetection.Create(_titan, DetectRange);
        }

        public void SetDetectRange(float range)
        {
            _detection.SetRange(range);
            DetectRange = range;
        }

        public void SetEnemy(BaseCharacter enemy, float focusTime = 0f)
        {
            _enemy = enemy;
            if (focusTime == 0f)
                focusTime = FocusTime;
            _focusTimeLeft = focusTime;
        }

        protected override void FixedUpdate()
        {
            _focusTimeLeft -= Time.deltaTime;
            _stateTimeLeft -= Time.deltaTime;
            if (_titan.Dead)
                return;
            if (_titan.State != TitanState.Attack && _titan.State != TitanState.Eat)
            {
                _rangedCooldownLeft -= Time.deltaTime;
                _attackCooldownLeft -= Time.deltaTime;
            }
            if (AIState == TitanAIState.ForcedIdle)
            {
                if (_stateTimeLeft <= 0f)
                    Idle();
                else
                    return;
            }
            if (_enemy != null)
            {
                if (_enemy.Dead)
                    _enemy = null;
            }
            if (_focusTimeLeft <= 0f || _enemy == null)
            {
                var enemy = FindNearestEnemy();
                if (enemy != null)
                    _enemy = enemy;
                else if (_enemy != null)
                {
                    if (Vector3.Distance(_titan.Cache.Transform.position, _enemy.Cache.Transform.position) > FocusRange)
                        _enemy = null;
                }
                _focusTimeLeft = FocusTime;
            }
            _titan.TargetEnemy = _enemy;
            if (_moveToActive && _moveToIgnoreEnemies)
                _enemy = null;
            if (AIState == TitanAIState.Idle || AIState == TitanAIState.Wander || AIState == TitanAIState.SitIdle)
            {
                if (_enemy == null)
                {
                    if (_moveToActive)
                        MoveToPosition();
                    else if (_stateTimeLeft <= 0f)
                    {
                        if (AIState == TitanAIState.Idle)
                        {
                            if (!IsCrawler() && !IsShifter() && RandomGen.Roll(0.3f))
                                Sit();
                            else
                                Wander();
                        }
                        else
                            Idle();
                    }
                }
                else
                {
                    _attackRange = CloseAttackRange * _titan.Size;
                    MoveToEnemy(true);
                }
            }
            else if (AIState == TitanAIState.MoveToPosition)
            {
                float distance = Vector3.Distance(_character.Cache.Transform.position, _moveToPosition);
                if (_enemy != null)
                {
                    _attackRange = CloseAttackRange * _titan.Size;
                    MoveToEnemy(true);
                }
                else if (distance < _moveToRange || !_moveToActive)
                {
                    _moveToActive = false;
                    Idle();
                }
                else if (_stateTimeLeft <= 0)
                    MoveToPosition(true);                    
                else
                    _titan.TargetAngle = GetMoveToAngle(_moveToPosition, true);
            }
            else if (AIState == TitanAIState.MoveToEnemy)
            {
                if (_enemy == null)
                    Idle();
                else if (_stateTimeLeft <= 0f && Util.DistanceIgnoreY(_character.Cache.Transform.position, _enemy.Cache.Transform.position) > ChaseAngleMinRange)
                {
                    MoveToEnemy(true);
                }
                else
                {
                    bool inRange = Util.DistanceIgnoreY(_character.Cache.Transform.position, _enemy.Cache.Transform.position) <= _attackRange;
                    if (inRange)
                    {
                        if (AttackWait <= 0f)
                        {
                            var validAttacks = GetValidAttacks();
                            if (validAttacks.Count > 0)
                                Attack(validAttacks);
                            else if (GetEnemyAngle(_enemy) > TurnAngle && HasClearLineOfSight(_enemy.Cache.Transform.position))
                            {
                                _moveAngle = 0f;
                                _titan.TargetAngle = GetChaseAngle(_enemy.Cache.Transform.position);
                                _titan.Turn(_titan.GetTargetDirection());
                                //Debug.DrawRay(_titan.Cache.Transform.TransformPoint(_mainCollider.center), _titan.GetTargetDirection() * 100, Color.black);
                            }
                            else
                            {
                                MoveToEnemy(true);
                                //Debug.DrawRay(_titan.Cache.Transform.TransformPoint(_mainCollider.center), _titan.GetTargetDirection() * 100, Color.white);
                            }
                                
                        }
                        else
                        {
                            var validAttacks = GetValidAttacks();
                            if (validAttacks.Count > 0)
                                WaitAttack();
                            else if (_stateTimeLeft <= 0f)
                                MoveToEnemy(false);
                        }
                    }
                    else
                    {
                        inRange = Util.DistanceIgnoreY(_character.Cache.Transform.position, _enemy.Cache.Transform.position) <= FarAttackRange;
                        var validAttacks = GetValidAttacks(true);
                        if (inRange && validAttacks.Count > 0)
                            Attack(validAttacks);
                        else if (HasClearLineOfSight(_enemy.Cache.Transform.position) == false)
                        {
                            _titan.TargetAngle = GetMoveToAngle(_enemy.Cache.Transform.position, true);
                            // Debug.DrawRay(_titan.Cache.Transform.TransformPoint(_mainCollider.center), _titan.GetTargetDirection() * 100, Color.magenta);
                        }
                        else
                        {
                            _titan.TargetAngle = GetChaseAngle(_enemy.Cache.Transform.position);
                            //Debug.DrawRay(_titan.Cache.Transform.TransformPoint(_mainCollider.center), _titan.GetTargetDirection() * 100, Color.yellow);
                        }
                    }
                }
            }
            else if (AIState == TitanAIState.WaitAttack)
            {
                if (_enemy == null)
                {
                    Idle();
                    return;
                }
                if (_stateTimeLeft <= 0f)
                {
                    _waitAttackTime += Time.deltaTime;
                    bool inRange = Util.DistanceIgnoreY(_character.Cache.Transform.position, _enemy.Cache.Transform.position) <= _attackRange;
                    _titan.HasDirection = false;
                    if (!inRange)
                        MoveToEnemy();
                    else
                    {
                        var validAttacks = GetValidAttacks();
                        if (validAttacks.Count > 0)
                            Attack(validAttacks);
                        else if (GetEnemyAngle(_enemy) > TurnAngle)
                        {
                            _moveAngle = 0f;
                            _titan.TargetAngle = GetChaseAngle(_enemy.Cache.Transform.position);
                            _titan.Turn(_titan.GetTargetDirection());
                            //Debug.DrawRay(_titan.Cache.Transform.TransformPoint(_mainCollider.center), _titan.GetTargetDirection() * 100, Color.red);
                        }
                        else
                        {
                            MoveToEnemy();
                            /*
                            _titan.HasDirection = true;
                            _titan.IsWalk = !IsRun;
                            _moveAngle = 0f;
                            _titan.TargetAngle = GetChaseAngle(_enemy.Cache.Transform.position);
                            //Debug.DrawRay(_titan.Cache.Transform.TransformPoint(_mainCollider.center), _titan.GetTargetDirection() * 100, Color.cyan);
                            */
                        }
                    }
                }
            }
            else if (AIState == TitanAIState.Action)
            {
                if (_titan.State == TitanState.Idle)
                    Idle();
            }
            SmartAttack = false;
        }

        protected float GetEnemyAngle(BaseCharacter enemy)
        {
            Vector3 direction = (enemy.Cache.Transform.position - _character.Cache.Transform.position);
            direction.y = 0f;
            return Mathf.Abs(Vector3.Angle(_character.Cache.Transform.forward, direction.normalized));
        }

        protected float GetChaseAngle(Vector3 position)
        {
            return GetChaseAngleGivenDirection((position - _character.Cache.Transform.position).normalized);
        }

        protected float GetChaseAngleGivenDirection(Vector3 direction)
        {
            float angle = GetTargetAngle(direction);
            angle += _moveAngle;
            // angle = Mathf.LerpAngle(_titan.TargetAngle, angle, Time.fixedDeltaTime);
            if (angle > 360f)
                angle -= 360f;
            if (angle < 0f)
                angle += 360f;
            return angle;
        }

        protected float GetMoveToAngle(Vector3 target, bool avoidCollisions = false)
        {
            var goalDirection = target - _titan.Cache.Transform.position;
            var resultDirection = (target - _titan.Cache.Transform.position).normalized * _targetWeight;
            bool col = false;
            if (avoidCollisions && _useCollisionAvoidance)
            {
                if (IsHeadingForCollision())
                {
                    col = true;
                    _collidedLastFrame = true;
                    _moveAngle = 0; // Random.Range(-10f, 10f);
                    resultDirection += GetFreeDirection(goalDirection).normalized * _collisionWeight;
                }
                else
                    _collidedLastFrame = false;
            }
            resultDirection = new Vector3(resultDirection.x, 0, resultDirection.z);
            resultDirection = resultDirection.normalized;
            var result =  GetChaseAngleGivenDirection(resultDirection);

            result = Mathf.LerpAngle(_titan.TargetAngle, result, Time.fixedDeltaTime);

            /*if (col)
                Debug.DrawRay(_titan.Cache.Transform.TransformPoint(_mainCollider.center), resultDirection * 100, Color.magenta);
            else
                Debug.DrawRay(_titan.Cache.Transform.TransformPoint(_mainCollider.center), resultDirection * 100, Color.white);*/

            return result;
        }

        protected bool IsHeadingForCollision()
        {
            RaycastHit hit;
            float colliderRadius = _mainCollider.radius * _titan.Cache.Transform.localScale.x;
            var start = _titan.Cache.Transform.TransformPoint(_mainCollider.center) + _titan.Cache.Transform.forward * -1 * colliderRadius;

            LayerMask mask = PhysicsLayer.GetMask(PhysicsLayer.MapObjectEntities);
            if (Physics.CheckSphere(start, colliderRadius, mask))
            {
                //Debug.DrawRay(start, _titan.Cache.Transform.forward * colliderRadius, Color.red, 1);
                return true;
            }
            else if (Physics.SphereCast(start, colliderRadius, _titan.Cache.Transform.forward, out hit, _collisionDetectionDistance, mask))
            {
                // display the ray
                //Debug.DrawRay(start, _titan.Cache.Transform.forward * hit.distance, Color.red, 1);
                return true;
            }
            //Debug.DrawRay(start, _titan.Cache.Transform.forward * _collisionDetectionDistance, Color.green, 1);
            return false;
        }

        protected bool HasClearLineOfSight(Vector3 target)
        {
            float colliderRadius = _mainCollider.radius * _titan.Cache.Transform.localScale.x * 0.5f;
            var start = _titan.Cache.Transform.TransformPoint(_mainCollider.center) + _titan.Cache.Transform.forward * -1 * colliderRadius;
            var left = _titan.Cache.Transform.TransformPoint(_mainCollider.center) + _titan.Cache.Transform.forward * -1 * colliderRadius + _titan.Cache.Transform.right * -1 * colliderRadius;
            var right = _titan.Cache.Transform.TransformPoint(_mainCollider.center) + _titan.Cache.Transform.forward * -1 * colliderRadius + _titan.Cache.Transform.right * colliderRadius;
            return HasLineOfSight(left, target) && HasLineOfSight(right, target);
        }

        protected bool HasLineOfSight(Vector3 start, Vector3 target)
        {
            RaycastHit hit;
            var direction = target - start;

            if (direction.magnitude > 100)
                return false;

            direction = direction.normalized;

            LayerMask mask = PhysicsLayer.GetMask(PhysicsLayer.MapObjectEntities, PhysicsLayer.Human);
            if (Physics.Raycast(start, direction, out hit, 100, mask))
            {
                if (hit.collider.gameObject.layer == PhysicsLayer.Human)
                {
                    Debug.DrawRay(start, direction * hit.distance, Color.cyan);
                    return true;
                }
                else
                {
                    Debug.DrawRay(start, direction * hit.distance, Color.gray);
                    return false;
                }
                    
            }
            return true;
        }

        private bool Approximately(float a, float b, float precision)
        {
            return (a - b) < precision;
        }

        protected Vector3 GetFreeDirection(Vector3 goalDirection)
        {
            float rayDelta = _sampleRayRange / _sampleRayCount;
            Vector3 bestDirection = _titan.Cache.Transform.forward;
            float bestDirScore = 0;
            float bestDirAlignment = 0;
            float bestPreviousAlignment = 0;

            float colliderRadius = _mainCollider.radius * _titan.Cache.Transform.localScale.x * 1f;
            Vector3 colliderCenter = _titan.Cache.Transform.TransformPoint(_mainCollider.center);
            var start = colliderCenter + _titan.Cache.Transform.forward * -1 * colliderRadius;

            for (int i = 0; i < _sampleRayCount; i++)
            {
                Vector3 rayDirection = Quaternion.AngleAxis(i * rayDelta - _sampleRayRange / 2, _titan.Cache.Transform.up) * _titan.Cache.Transform.forward;
                RaycastHit hit;
                if (Physics.SphereCast(start, colliderRadius, rayDirection, out hit, _collisionAvoidDistance, PhysicsLayer.GetMask(PhysicsLayer.MapObjectEntities)))
                {
                    Vector3 rayActualDirection = (start + rayDirection.normalized * hit.distance) - colliderCenter;
                    float alignment = Vector3.Angle(rayActualDirection.normalized, goalDirection.normalized);
                    float prevAlignment = Vector3.Angle(_titan.GetTargetDirection(), rayActualDirection.normalized);

                    if (Approximately(hit.distance, bestDirScore, 0.001f))
                    {
                        if (Approximately(alignment, bestDirAlignment, 0.001f))
                        {
                            if (Approximately(prevAlignment, bestPreviousAlignment, 0.001f) && prevAlignment < bestPreviousAlignment)
                            {
                                bestDirection = rayActualDirection;
                                bestDirScore = hit.distance;
                                bestDirAlignment = alignment;
                                bestPreviousAlignment = prevAlignment;
                            }
                        }
                        else if (alignment < bestPreviousAlignment)
                        {
                            bestDirection = rayActualDirection;
                            bestDirScore = hit.distance;
                            bestDirAlignment = alignment;
                            bestPreviousAlignment = prevAlignment;
                        }
                    }
                    else if (hit.distance > bestDirScore)
                    {
                        bestDirection = rayActualDirection;
                        bestDirScore = hit.distance;
                        bestDirAlignment = alignment;
                        bestPreviousAlignment = prevAlignment;
                    }

                    // Draw the ray to the hit point
                    //Debug.DrawRay(start, rayDirection * hit.distance, Color.red);

                }
                else
                {
                    Vector3 rayActualDirection = (start + rayDirection.normalized * _collisionAvoidDistance) - colliderCenter;
                    //Debug.DrawRay(start, rayDirection * _collisionAvoidDistance, Color.blue);
                    return rayActualDirection;
                }
            }
            return bestDirection;
        }

        protected bool IsCrawler()
        {
            return _titan is BasicTitan && ((BasicTitan)_titan).IsCrawler;
        }

        protected bool IsShifter()
        {
            return _titan is BaseShifter;
        }

        protected void Idle()
        {
            AIState = TitanAIState.Idle;
            _titan.HasDirection = false;
            _titan.IsSit = false;
            _stateTimeLeft = Random.Range(2f, 6f);
        }

        protected void Wander()
        {
            AIState = TitanAIState.Wander;
            _titan.HasDirection = true;
            _titan.TargetAngle = Random.Range(0f, 360f);
            _titan.IsWalk = true;
            _titan.IsSit = false;
            if (IsCrawler())
                _titan.IsWalk = false;
            float angle = Vector3.Angle(_titan.Cache.Transform.forward, _titan.GetTargetDirection());
            if (Mathf.Abs(angle) > 60f)
                _titan.Turn(_titan.GetTargetDirection());
            _stateTimeLeft = Random.Range(2f, 8f);
        }

        protected void Sit()
        {
            AIState = TitanAIState.SitIdle;
            _titan.IsSit = true;
            _stateTimeLeft = Random.Range(6f, 12f);
        }

        protected void MoveToEnemy(bool avoidCollisions = false)
        {
            AIState = TitanAIState.MoveToEnemy;
            _titan.HasDirection = true;
            _titan.IsSit = false;
            _titan.IsWalk = !IsRun;
            bool dodgeRange = Util.DistanceIgnoreY(_character.Cache.Transform.position, _enemy.Cache.Transform.position) > ChaseAngleMinRange;
            if (dodgeRange)
                _moveAngle = Random.Range(-45f, 45f);
            else
                _moveAngle = 0f;
            _titan.TargetAngle = GetMoveToAngle(_enemy.Cache.Transform.position, avoidCollisions);
            _stateTimeLeft = Random.Range(ChaseAngleTimeMin, ChaseAngleTimeMax);
        }

        protected void MoveToPosition(bool avoidCollisions = false)
        {
            AIState = TitanAIState.MoveToPosition;
            _titan.HasDirection = true;
            _titan.IsSit = false;
            _titan.IsWalk = !IsRun;
            _moveAngle = Random.Range(-45f, 45f);
            _titan.TargetAngle = GetMoveToAngle(_moveToPosition, avoidCollisions);
            _stateTimeLeft = Random.Range(ChaseAngleTimeMin, ChaseAngleTimeMax);
        }

        protected void Attack(List<string> validAttacks)
        {
            _titan.HasDirection = false;
            string attack = GetRandomAttack(validAttacks);
            if (attack == "" || !_titan.CanAttack())
                Idle();
            else
            {
                _attack = attack;
                AIState = TitanAIState.Action;
                _titan.Attack(_attack);
                if (AttackInfos[attack].FarOnly)
                    _rangedCooldownLeft = FarAttackCooldown;
            }
        }

        protected void WaitAttack()
        {
            AIState = TitanAIState.WaitAttack;
            _titan.HasDirection = false;
            _stateTimeLeft = AttackWait;
            _waitAttackTime = 0f;
        }

        protected BaseCharacter FindNearestEnemy()
        {
            if (_detection.Enemies.Count == 0)
                return null;
            Vector3 position = _titan.Cache.Transform.position;
            float nearestDistance = float.PositiveInfinity;
            BaseCharacter nearestCharacter = null;
            foreach (BaseCharacter character in _detection.Enemies)
            {
                if (character == null || character.Dead)
                    continue;
                float distance = Vector3.Distance(character.Cache.Transform.position, position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestCharacter = character;
                }
            }
            return nearestCharacter;
        }

        private string GetRandomAttack(List<string> validAttacks)
        {
            float total = 0f;
            Dictionary<string, int> groupCount = new Dictionary<string, int>();
            foreach (var attack in validAttacks)
            {
                var info = AttackInfos[attack];
                if (AttackGroups.ContainsKey(attack))
                {
                    string group = AttackGroups[attack];
                    if (!groupCount.ContainsKey(group))
                    {
                        groupCount.Add(group, 0);
                        total += AttackChances[attack];
                    }
                    groupCount[group]++;
                }
                else
                    total += AttackChances[attack];
            }
            if (total == 0f)
                return string.Empty;
            float r = Random.Range(0f, total);
            float start = 0f;
            foreach (var attack in validAttacks)
            {
                float chance = AttackChances[attack];
                if (AttackGroups.ContainsKey(attack))
                {
                    string group = AttackGroups[attack];
                    int count = groupCount[group];
                    chance = (chance / (float)count);
                }
                if (r >= start && r < start + chance)
                    return attack;
                start += chance;
            }
            return validAttacks[0];
        }

        protected virtual List<string> GetValidAttacks(bool farOnly = false)
        {
            var validAttacks = new List<string>();
            if (_enemy == null || !_titan.CanAttack())
                return validAttacks;
            Vector3 worldPosition = _enemy.Cache.Transform.position;
            Vector3 velocity = Vector3.zero;
            Vector3 relativePosition;
            bool isHuman = _enemy is Human;
            if (isHuman)
            {
                velocity = ((Human)_enemy).GetVelocity();
                relativePosition = _character.Cache.Transform.InverseTransformPoint(_enemy.Cache.Transform.position);
            }
            else
            {
                var titan = (BaseTitan)_enemy;
                relativePosition = _character.Cache.Transform.InverseTransformPoint(titan.BaseTitanCache.Hip.position);
            }
            foreach (string attackName in AttackChances.Keys)
            {
                var attackInfo = AttackInfos[attackName];
                if (attackInfo.HumanOnly && !isHuman)
                    continue;
                if (farOnly && !attackInfo.FarOnly)
                    continue;
                if (attackInfo.FarOnly && _rangedCooldownLeft > 0f)
                    continue;
                if (!SmartAttack || attackInfo.FarOnly || !isHuman)
                {
                    if (attackInfo.CheckSimpleAttack(relativePosition))
                        validAttacks.Add(attackName);
                }
                else
                {
                    if (attackInfo.CheckSmartAttack(_titan.Cache.Transform, worldPosition, velocity, _titan.GetAttackSpeed(attackName), _titan.Size))
                        validAttacks.Add(attackName);
                }
            }
            return validAttacks;
        }
    }

    public enum TitanAIState
    {
        Idle,
        Wander,
        SitIdle,
        MoveToEnemy,
        MoveToPosition,
        Action,
        WaitAttack,
        ForcedIdle
    }
}
