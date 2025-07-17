using Characters;
using GameManagers;
using Map;
using Settings;
using SimpleJSONFixed;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Controllers
{
    class BaseTitanAIController : BaseAIController
    {
        protected BaseTitan _titan;
        public TitanAIState AIState = TitanAIState.Idle;
        public bool SmartAttack = false;
        public float DetectRange;
        public float CloseAttackRange;
        public float FarAttackMinRange;
        public float FarAttackMaxRange;
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
        protected float _moveToTimeout;
        protected bool _moveToExact = false;
        protected Action _moveToCallback = null;
        public Dictionary<string, float> AttackChances = new Dictionary<string, float>();
        public Dictionary<string, string> AttackGroups = new Dictionary<string, string>();
        public Dictionary<string, TitanAttackInfo> AttackInfos;
        protected float _stateTimeLeft;
        protected float _focusTimeLeft;
        protected float _rangedCooldownLeft;
        protected float _attackRange;
        protected ITargetable _enemy;
        protected string _attack;
        protected float _attackCooldownLeft;
        protected float _waitAttackTimeLeft;
        protected float _enemyDistance;
        protected bool _isAIEnabled = true;

        // A way to circumvent normal AI control so that we can force certain scripted behavior.
        public bool AIEnabled
        {
            get => _isAIEnabled;
            set
            {
                _isAIEnabled = value;
                _titan.EnableAI = value;
                if (!value)
                {
                    Idle();
                }
            }
        }

        // pathing
        public bool _usePathfinding = true;
        private NavMeshAgent _agent;
        private CapsuleCollider _mainCollider;
        private bool _setTargetThisFrame = false;

        // Layers
        private LayerMask _losLayer = PhysicsLayer.GetMask(PhysicsLayer.MapObjectEntities, PhysicsLayer.MapObjectCharacters, PhysicsLayer.MapObjectTitans,
            PhysicsLayer.MapObjectAll);

        protected override void Awake()
        {
            base.Awake();
            _titan = GetComponent<BaseTitan>();
            _mainCollider = _titan.GetComponent<CapsuleCollider>();
            _usePathfinding = SettingsManager.InGameUI.Titan.TitanSmartMovement.Value;

            if (_usePathfinding)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(_titan.Cache.Transform.position, out hit, 100f, NavMesh.AllAreas))
                {
                    _titan.Cache.Transform.position = hit.position;
                }
            }
        }

        protected override void Start()
        {
            Idle();

            // set agent size
            if (_usePathfinding)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(_titan.Cache.Transform.position, out hit, 100f, NavMesh.AllAreas))
                {
                    _titan.Cache.Transform.position = hit.position;
                }

                // Add the navmesh agent
                NavMeshBuildSettings agentSettings = Util.GetAgentSettingsCorrected(_titan.Size);
                _agent = gameObject.AddComponent<NavMeshAgent>();
                _agent.agentTypeID = agentSettings.agentTypeID;
                _agent.speed = _titan.GetCurrentSpeed();
                _agent.angularSpeed = 10;
                _agent.acceleration = 10;
                _agent.autoRepath = true;
                _agent.stoppingDistance = 1.1f;
                _agent.autoBraking = false;
                _agent.radius = agentSettings.agentRadius;
                _agent.height = agentSettings.agentHeight;
                _agent.updatePosition = false;
                _agent.updateRotation = false;
                _agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
                _agent.avoidancePriority = 0;
            }

        }

        public void MoveTo(Vector3 position, float range, bool ignore)
        {
            _moveToPosition = position;
            _moveToActive = true;
            _moveToRange = range;
            _moveToIgnoreEnemies = ignore;
            _moveToExact = false;
        }

        public void MoveToCallback(Action action, Vector3 position, float range, bool ignore)
        {
            _moveToPosition = position;
            _moveToActive = true;
            _moveToRange = range;
            _moveToIgnoreEnemies = ignore;
            _moveToExact = false;
            _moveToCallback = action;
        }

        public void MoveToExact(Vector3 position, float range = 10, float timeoutPadding = 1f)
        {
            _moveToPosition = position;
            _moveToActive = true;
            _moveToRange = range;
            _moveToIgnoreEnemies = true;
            _moveToExact = true;

            // Calculate expected walk time.
            _moveToTimeout = _titan.WalkSpeedBase * Vector3.Distance(_titan.Cache.Transform.position, position) + timeoutPadding;
        }

        public void MoveToExactCallback(Action action, Vector3 position, float range = 10, float timeoutPadding = 1f)
        {
            _moveToPosition = position;
            _moveToActive = true;
            _moveToRange = range;
            _moveToIgnoreEnemies = true;
            _moveToExact = true;

            // Calculate expected walk time.
            _moveToTimeout = _titan.WalkSpeedBase * Vector3.Distance(_titan.Cache.Transform.position, position) + timeoutPadding;

            _moveToCallback = action;
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
            FarAttackMinRange = data["FarAttackMinRange"].AsFloat;
            FarAttackMaxRange = data["FarAttackMaxRange"].AsFloat;
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
            _waitAttackTimeLeft = AttackWait;
        }

        public void SetDetectRange(float range)
        {
            DetectRange = range;
        }

        public void SetEnemy(ITargetable enemy, float focusTime = 0f)
        {
            _enemy = enemy;
            if (focusTime == 0f)
                focusTime = FocusTime;
            _focusTimeLeft = focusTime;
        }

        /// <summary>
        /// SetDestination is a marshalled call and is being called more often than needed.
        /// </summary>
        /// <param name="position"></param>
        public void SetAgentDestination(Vector3 position)
        {
            if (!_setTargetThisFrame)
            {
                _agent.SetDestination(position);
                _setTargetThisFrame = true;
            }
        }

        protected override void FixedUpdate()
        {
            if (_usePathfinding)
            {
                _setTargetThisFrame = false;
                _agent.speed = _titan.GetCurrentSpeed();
            }
            _focusTimeLeft -= Time.deltaTime;
            _stateTimeLeft -= Time.deltaTime;
            _moveToTimeout -= Time.deltaTime;
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

            if (AIEnabled == false)
            {
                _enemy = null;
                if (AIState == TitanAIState.Idle || AIState == TitanAIState.Wander || AIState == TitanAIState.SitIdle)
                {
                    if (_moveToActive)
                        MoveToPosition();
                }
                if (AIState == TitanAIState.MoveToPosition)
                {
                    float distance = Vector3.Distance(_character.Cache.Transform.position, _moveToPosition);

                    if (distance <= _moveToRange || !_moveToActive || (_moveToExact && _moveToTimeout <= 0))
                    {
                        if (_moveToExact)
                        {
                            _titan.Cache.Transform.position = _moveToPosition;
                        }

                        if (_moveToCallback != null)
                        {
                            _moveToCallback.Invoke();
                            _moveToCallback = null;
                        }

                        _moveToActive = false;
                        Idle();
                    }
                    else if (_stateTimeLeft <= 0 || _usePathfinding)
                        MoveToPosition();
                    else if (_usePathfinding == false)
                        _titan.TargetAngle = GetChaseAngle(_moveToPosition, true);
                }

                RefreshAgent();
                return;
            }

            if (_enemy != null)
            {
                if (!_enemy.ValidTarget())
                    _enemy = null;
            }
            if (_focusTimeLeft <= 0f || _enemy == null)
            {
                var enemy = FindNearestEnemy();
                if (enemy != null)
                    _enemy = enemy;
                else if (_enemy != null)
                {
                    if (TeamInfo.SameTeam(_titan.Team, _enemy.GetTeam()) || Vector3.Distance(_titan.Cache.Transform.position, _enemy.GetPosition()) > FocusRange)
                        _enemy = null;
                }
                if (_enemy != null && _enemy.ValidTarget() && _usePathfinding && _agent.isOnNavMesh && _agent.pathPending == false && !(_moveToActive && _moveToIgnoreEnemies))
                    SetAgentDestination(_enemy.GetPosition());
                _focusTimeLeft = FocusTime;
            }
            _titan.TargetEnemy = _enemy;
            if (_moveToActive && _moveToIgnoreEnemies)
                _enemy = null;
            if (_enemy != null && _enemy.ValidTarget())
                _enemyDistance = Util.DistanceIgnoreY(_character.Cache.Transform.position, _enemy.GetPosition());
            if (AIState == TitanAIState.Idle || AIState == TitanAIState.Wander || AIState == TitanAIState.SitIdle)
            {
                if (_enemy == null || _enemy.ValidTarget() == false)
                {
                    if (_moveToActive)
                        MoveToPosition();
                    else if (_stateTimeLeft <= 0f)
                    {
                        if (AIState == TitanAIState.Idle)
                        {
                            if (!IsCrawler() && !IsShifter() && RandomGen.Roll(0.33f))
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
                    MoveToEnemy();
                }
            }
            else if (AIState == TitanAIState.MoveToPosition)
            {
                float distance = Vector3.Distance(_character.Cache.Transform.position, _moveToPosition);
                if (_enemy != null)
                {
                    _attackRange = CloseAttackRange * _titan.Size;
                    MoveToEnemy();
                }
                if (distance < _moveToRange || !_moveToActive || (_moveToExact && _moveToTimeout <= 0))
                {
                    if (_moveToExact)
                    {
                        _titan.Cache.Transform.position = _moveToPosition;
                    }
                    _moveToActive = false;
                    Idle();
                }
                else if (_stateTimeLeft <= 0 || _usePathfinding)
                    MoveToPosition();
                else if (_usePathfinding == false)
                    _titan.TargetAngle = GetChaseAngle(_moveToPosition, true);
            }
            else if (AIState == TitanAIState.MoveToEnemy)
            {
                if (_enemy == null || _enemy.ValidTarget() == false)
                    Idle();
                else if (_stateTimeLeft <= 0f && _enemyDistance > ChaseAngleMinRange)
                    MoveToEnemy();
                else
                {
                    if (_enemyDistance <= _attackRange)
                    {
                        var validAttacks = GetValidAttacks();
                        if (validAttacks.Count > 0)
                        {
                            if (AttackWait <= 0f)
                                Attack(validAttacks);
                            else
                                WaitAttack();
                        }
                        else if (GetEnemyAngle(_enemy) > TurnAngle && AttackWait > 0f)
                            WaitAttack();
                        else if (HasClearLineOfSight(_enemy.GetPosition()))
                            TargetEnemy();
                        else if (_stateTimeLeft <= 0f)
                            MoveToEnemy();
                    }
                    else
                    {
                        var validAttacks = GetValidAttacks(true);
                        if (_enemyDistance <= FarAttackMaxRange && _enemyDistance >= FarAttackMinRange && validAttacks.Count > 0)
                            Attack(validAttacks);
                        else if (HasClearLineOfSight(_enemy.GetPosition()))
                            TargetEnemy();
                        else if (_stateTimeLeft <= 0f)
                            MoveToEnemy();
                    }
                }
            }
            else if (AIState == TitanAIState.WaitAttack)
            {
                if (_enemy == null || _enemy.ValidTarget() == false)
                {
                    Idle();
                    return;
                }
                _waitAttackTimeLeft -= Time.deltaTime;
                if (_waitAttackTimeLeft <= 0f)
                {
                    _titan.HasDirection = false;
                    if (_enemyDistance > _attackRange)
                        MoveToEnemy();
                    else
                    {
                        var validAttacks = GetValidAttacks();
                        if (validAttacks.Count > 0)
                            Attack(validAttacks);
                        else if (GetEnemyAngle(_enemy) > TurnAngle)
                        {
                            _moveAngle = 0f;
                            _titan.TargetAngle = GetChaseAngle(_enemy.GetPosition(), false);
                            _titan.Turn(_titan.GetTargetDirection());
                        }
                        else if (HasClearLineOfSight(_enemy.GetPosition()))
                        {
                            _titan.HasDirection = true;
                            _titan.IsWalk = !IsRun;
                            _moveAngle = 0f;
                            TargetEnemy();
                        }
                        else
                            MoveToEnemy();
                    }
                    _waitAttackTimeLeft = AttackWait;
                }
            }
            else if (AIState == TitanAIState.Action)
            {
                if (_titan.State == TitanState.Idle)
                    Idle();
            }

            RefreshAgent();
        }

        protected void RefreshAgent()
        {
            SmartAttack = false;
            if (_usePathfinding)
            {
                // Good ol' power cycle fix
                // if agent gets desynced (position is not equal to titan position), disable the component and re-enable it
                Vector3 agentPositionXY = new Vector3(_agent.transform.position.x, 0, _agent.transform.position.z);
                Vector3 titanPositionXY = new Vector3(_titan.Cache.Transform.position.x, 0, _titan.Cache.Transform.position.z);
                if (_usePathfinding && Vector3.Distance(agentPositionXY, titanPositionXY) > 0.1f)
                {
                    // debug log
                    _agent.enabled = false;
                    _agent.enabled = true;
                }

                _agent.nextPosition = _titan.Cache.Transform.position;
            }
        }

        protected float GetEnemyAngle(ITargetable enemy)
        {
            Vector3 direction;
            if (enemy == null || enemy.ValidTarget() == false)
                direction = _character.Cache.Transform.forward;
            else
                direction = (enemy.GetPosition() - _character.Cache.Transform.position);
            direction.y = 0f;
            return Mathf.Abs(Vector3.Angle(_character.Cache.Transform.forward, direction.normalized));
        }

        protected float GetChaseAngle(Vector3 position, bool useMoveAngle)
        {
            return GetChaseAngleGivenDirection((position - _character.Cache.Transform.position).normalized, useMoveAngle);
        }

        protected float GetChaseAngleGivenDirection(Vector3 direction, bool useMoveAngle)
        {
            float angle;
            if (direction == Vector3.zero)
                angle = _titan.TargetAngle;
            else
                angle = GetTargetAngle(direction);
            if (useMoveAngle)
                angle += _moveAngle;
            if (angle > 360f)
                angle -= 360f;
            if (angle < 0f)
                angle += 360f;
            return angle;
        }

        protected Vector3 GetDirectionTowardsNavMesh()
        {
            // Find a point on the navmesh closest to the titan
            NavMeshHit hit;
            if (NavMesh.SamplePosition(_titan.Cache.Transform.position, out hit, 100f, NavMesh.AllAreas))
            {
                return (hit.position - _titan.Cache.Transform.position).normalized;
            }

            // Return a random direction if the navmesh is not found
            Vector3 randDir = UnityEngine.Random.onUnitSphere;
            randDir.y = 0;
            return randDir.normalized;
        }

        /*public void Update()
        {
            // draw path
            if (_usePathfinding && _agent.hasPath)
            {
                StartCoroutine(DrawPath(_agent.path));
            }
        }

        public LineRenderer lineRenderer;
        IEnumerator DrawPath(NavMeshPath path)
        {
            // draw the agents path using line renderers
            if (path.corners.Length < 2)
                yield break;

            if (lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
                lineRenderer.positionCount = path.corners.Length;
            }

            lineRenderer.positionCount = path.corners.Length;

            for (int i = 0; i < path.corners.Length; i++)
            {
                lineRenderer.SetPosition(i, path.corners[i]);
            }

        }*/

        protected float GetAgentNavAngle(Vector3 target)
        {
            Vector3 resultDirection = _agent.velocity.normalized;
            // Good ol' power cycle fix
            // if agent gets desynced (position is not equal to titan position), disable the component and re-enable it
            Vector3 agentPositionXY = new Vector3(_agent.transform.position.x, 0, _agent.transform.position.z);
            Vector3 titanPositionXY = new Vector3(_titan.Cache.Transform.position.x, 0, _titan.Cache.Transform.position.z);
            if (_agent.isOnNavMesh && Vector3.Distance(agentPositionXY, titanPositionXY) > 1f)
            {
                resultDirection = (_agent.transform.position - _titan.Cache.Transform.position).normalized;
            }
            else if (_agent.isOnNavMesh && _agent.pathPending == false)
            {
                SetAgentDestination(target);
            }
            else if (_agent.isOnNavMesh == false)
            {
                resultDirection = GetDirectionTowardsNavMesh();
            }

            if (resultDirection == Vector3.zero)
                return _titan.TargetAngle;

            return GetChaseAngleGivenDirection(resultDirection, true);
        }

        protected bool HasClearLineOfSight(Vector3 target)
        {
            if (_usePathfinding == false)
                return true;
            float colliderRadius = _mainCollider.radius * _titan.Cache.Transform.localScale.x * 0.5f;
            // var start = _titan.Cache.Transform.TransformPoint(_mainCollider.center) + _titan.Cache.Transform.forward * -1 * colliderRadius;
            var left = _titan.Cache.Transform.TransformPoint(_mainCollider.center) + _titan.Cache.Transform.forward * -1 * colliderRadius + _titan.Cache.Transform.right * -1.1f * colliderRadius;
            var right = _titan.Cache.Transform.TransformPoint(_mainCollider.center) + _titan.Cache.Transform.forward * -1 * colliderRadius + _titan.Cache.Transform.right * 1.1f * colliderRadius;
            return HasLineOfSight(left, target) && HasLineOfSight(right, target);
        }

        protected bool HasLineOfSight(Vector3 start, Vector3 target)
        {
            RaycastHit hit;
            var direction = target - start;
            var distance = direction.magnitude - 0.2f;
            if (distance > 1000)
                return false;
            direction = direction.normalized;
            if (Physics.Raycast(start, direction, out hit, distance, _losLayer.value))
                return false;
            return true;
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
            _stateTimeLeft = UnityEngine.Random.Range(4f, 8f);
        }

        protected void Wander()
        {
            AIState = TitanAIState.Wander;
            _titan.HasDirection = true;
            _titan.TargetAngle = UnityEngine.Random.Range(0f, 360f);
            _titan.IsWalk = true;
            _titan.IsSit = false;
            if (IsCrawler())
                _titan.IsWalk = false;
            float angle = Vector3.Angle(_titan.Cache.Transform.forward, _titan.GetTargetDirection());
            if (Mathf.Abs(angle) > 60f)
                _titan.Turn(_titan.GetTargetDirection());
            _stateTimeLeft = UnityEngine.Random.Range(2f, 6f);
        }

        protected void Sit()
        {
            AIState = TitanAIState.SitIdle;
            _titan.IsSit = true;
            _stateTimeLeft = UnityEngine.Random.Range(8f, 14f);
        }

        protected void MoveToEnemy(bool avoidCollisions = true)
        {
            AIState = TitanAIState.MoveToEnemy;
            _titan.HasDirection = true;
            _titan.IsSit = false;
            _titan.IsWalk = !IsRun;
            _moveAngle = UnityEngine.Random.Range(-45f, 45f);
            if (_usePathfinding && avoidCollisions && _enemy != null && _enemy.ValidTarget())
                _titan.TargetAngle = GetAgentNavAngle(_enemy.GetPosition());
            else
                TargetEnemy();
            _stateTimeLeft = UnityEngine.Random.Range(ChaseAngleTimeMin, ChaseAngleTimeMax);
        }

        protected void TargetEnemy()
        {
            _titan.TargetAngle = GetChaseAngle(_enemy.GetPosition(), _enemyDistance > ChaseAngleMinRange);
        }

        protected void MoveToPosition()
        {
            AIState = TitanAIState.MoveToPosition;
            _titan.HasDirection = true;
            _titan.IsSit = false;
            _titan.IsWalk = !IsRun;
            if (_usePathfinding)
            {
                _moveAngle = UnityEngine.Random.Range(-5f, 5f);
                _titan.TargetAngle = GetAgentNavAngle(_moveToPosition);
            }
            else
            {
                _moveAngle = UnityEngine.Random.Range(-45f, 45f);
                _titan.TargetAngle = GetChaseAngle(_moveToPosition, true);
            }
            _stateTimeLeft = UnityEngine.Random.Range(ChaseAngleTimeMin, ChaseAngleTimeMax);
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
            _waitAttackTimeLeft = AttackWait;
        }

        protected void WaitAttack()
        {
            AIState = TitanAIState.WaitAttack;
            _titan.HasDirection = false;
            _waitAttackTimeLeft = AttackWait;
        }

        protected ITargetable FindNearestEnemy()
        {
            Vector3 position = _titan.Cache.Transform.position;
            float nearestDistance = float.PositiveInfinity;
            ITargetable nearestCharacter = null;
            var character = _titan.Detection.ClosestEnemy;
            if (character != null && !character.Dead)
            {
                float distance = Vector3.Distance(character.Cache.Transform.position, position);
                if (distance < nearestDistance && distance < DetectRange)
                {
                    nearestDistance = distance;
                    nearestCharacter = character;
                }
            }
            foreach (MapTargetable targetable in MapLoader.MapTargetables)
            {
                if (targetable == null || !targetable.ValidTarget())
                    continue;
                float distance = Vector3.Distance(targetable.GetPosition(), position);
                if (distance < nearestDistance && distance < DetectRange)
                {
                    nearestDistance = distance;
                    nearestCharacter = targetable;
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
            float r = UnityEngine.Random.Range(0f, total);
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
            if (_enemy == null || _enemy?.ValidTarget() == false || !_titan.CanAttack())
                return validAttacks;
            Vector3 worldPosition = _enemy.GetPosition();
            Vector3 velocity = Vector3.zero;
            Vector3 relativePosition;
            bool isHuman = _enemy is Human;
            bool isMapTargetable = _enemy is MapTargetable;
            if (isHuman)
            {
                velocity = ((Human)_enemy).GetVelocity();
                relativePosition = _character.Cache.Transform.InverseTransformPoint(_enemy.GetPosition());
            }
            else if (_enemy is BaseTitan)
            {
                var titan = (BaseTitan)_enemy;
                relativePosition = _character.Cache.Transform.InverseTransformPoint(titan.BaseTitanCache.Hip.position);
            }
            else
                relativePosition = _character.Cache.Transform.InverseTransformPoint(_enemy.GetPosition());
            foreach (string attackName in AttackChances.Keys)
            {
                var attackInfo = AttackInfos[attackName];
                if (isMapTargetable)
                {
                    if (!attackInfo.MapObject)
                        continue;
                }
                else if (attackInfo.HumanOnly && !isHuman)
                    continue;
                if (farOnly && !attackInfo.FarOnly)
                    continue;
                if (attackInfo.FarOnly && _rangedCooldownLeft > 0f)
                    continue;
                if (attackInfo.LeftArm && _titan.LeftArmDisabled)
                    continue;
                if (attackInfo.RightArm && _titan.RightArmDisabled)
                    continue;
                if (!SmartAttack || attackInfo.FarOnly || !isHuman || !attackInfo.HasKeyframes)
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
