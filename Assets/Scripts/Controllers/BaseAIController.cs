using System;
using UnityEngine;
using ApplicationManagers;
using GameManagers;
using UnityEngine.UI;
using Settings;
using Characters;
using UI;

namespace Controllers
{
    class BaseAIController: MonoBehaviour
    {
        protected BaseCharacter _character;

        protected virtual void Awake()
        {
            _character = GetComponent<BaseCharacter>();
        }

        protected virtual void Start()
        {
        }

        protected virtual void FixedUpdate()
        {
        }

        protected float GetTargetAngle(Vector3 direction)
        {
            direction = new Vector3(direction.x, 0f, direction.z).normalized;
            if (direction == Vector3.zero)
                return _character.transform.eulerAngles.y;

            return Quaternion.LookRotation(direction, Vector3.up).eulerAngles.y;
        }

        protected Quaternion GetTargetRotation(float angle)
        {
            return Quaternion.Euler(0f, angle, 0f);
        }

        protected Vector3 GetTargetDirection(float angle)
        {
            float angleRadians = (90f - angle) * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(angleRadians), 0f, Mathf.Sin(angleRadians)).normalized;
        }
    }
}
