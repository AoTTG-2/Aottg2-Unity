using UnityEngine;
using CustomLogic;
using Map;
using Characters;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicCollisionHandler : MonoBehaviour
    {
        List<CustomLogicComponentInstance> _classInstances = new List<CustomLogicComponentInstance>();
        private float _lastEnterTime;
        private bool _hasConvexTriggerCollider;
        private MeshCollider _nonConvexMeshCollider;
        private bool _convexTriggerIsColliding;

        public void SetConvexTrigger(MeshCollider nonConvexMeshCollider)
        {
            _hasConvexTriggerCollider = true;
            _nonConvexMeshCollider = nonConvexMeshCollider;
        }

        public void RegisterInstance(CustomLogicComponentInstance classInstance)
        {
            _classInstances.Add(classInstance);
        }

        public void GetHit(BaseCharacter character, string name, int damage, string type)
        {
            CustomLogicCharacterBuiltin builtin = null;
            if (character is Human)
                builtin = new CustomLogicHumanBuiltin((Human)character);
            else if (character is BasicTitan)
                builtin = new CustomLogicTitanBuiltin((BasicTitan)character);
            else if (character is BaseShifter)
                builtin = new CustomLogicShifterBuiltin((BaseShifter)character);
            foreach (var classInstance in _classInstances)
                classInstance.OnGetHit(builtin, name, damage, type);
        }

        public void GetHooked(Human human, Vector3 position, bool left)
        {
            CustomLogicHumanBuiltin builtin = new CustomLogicHumanBuiltin(human);
            foreach (var classInstance in _classInstances)
                classInstance.OnGetHooked(builtin, new CustomLogicVector3Builtin(position), left);
        }

        protected void OnCollisionEnter(Collision other)
        {
            var builtin = GetBuiltin(other.collider);
            if (builtin == null)
                return;
            foreach (var classInstance in _classInstances)
                classInstance.OnCollisionEnter(builtin);
        }

        protected void OnCollisionStay(Collision other)
        {
            var builtin = GetBuiltin(other.collider);
            if (builtin == null)
                return;
            foreach (var classInstance in _classInstances)
                classInstance.OnCollisionStay(builtin);
        }

        protected void OnCollisionExit(Collision other)
        {
            var builtin = GetBuiltin(other.collider);
            if (builtin == null)
                return;
            foreach (var classInstance in _classInstances)
                classInstance.OnCollisionExit(builtin);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (Time.fixedTime == _lastEnterTime)
                return;
            _lastEnterTime = Time.fixedTime;
            if (_hasConvexTriggerCollider)
            {
                _convexTriggerIsColliding = CheckConvexTriggerCollision(other);
                if (!_convexTriggerIsColliding)
                    return;
            }
            var builtin = GetBuiltin(other);
            if (builtin == null)
                return;
            foreach (var classInstance in _classInstances)
                classInstance.OnCollisionEnter(builtin);
        }

        protected void OnTriggerStay(Collider other)
        {
            if (_hasConvexTriggerCollider)
            {
                _convexTriggerIsColliding = CheckConvexTriggerCollision(other);
                if (!_convexTriggerIsColliding)
                    return;
            }
            var builtin = GetBuiltin(other);
            if (builtin == null)
                return;
            foreach (var classInstance in _classInstances)
                classInstance.OnCollisionStay(builtin);
        }

        protected void OnTriggerExit(Collider other)
        {
            if (_hasConvexTriggerCollider)
            {
                bool wasColliding = _convexTriggerIsColliding;
                _convexTriggerIsColliding = false;
                if (!wasColliding)
                    return;
            }
            var builtin = GetBuiltin(other);
            if (builtin == null)
                return;
            foreach (var classInstance in _classInstances)
                classInstance.OnCollisionExit(builtin);
        }

        private bool CheckConvexTriggerCollision(Collider other)
        {
            Vector3 direction;
            float distance;
            bool pen = Physics.ComputePenetration(_nonConvexMeshCollider, _nonConvexMeshCollider.transform.position, _nonConvexMeshCollider.transform.rotation,
                                              other, other.transform.position, other.transform.rotation, out direction, out distance);
            return pen;
        }


        public static CustomLogicBaseBuiltin GetBuiltin(Collider other)
        {
            if (other == null || other.transform == null)
                return null;
            var root = other.transform.root;
            if (root == null)
                return null;
            var character = root.GetComponent<BaseCharacter>();
            if (character != null)
            {
                CustomLogicBaseBuiltin builtin = null;
                if (character is Human)
                    builtin = new CustomLogicHumanBuiltin((Human)character);
                else if (character is BasicTitan)
                    builtin = new CustomLogicTitanBuiltin((BasicTitan)character);
                else if (character is BaseShifter)
                    builtin = new CustomLogicShifterBuiltin((BaseShifter)character);
                return builtin;
            }
            else if (MapLoader.GoToMapObject.ContainsKey(root.gameObject))
            {
                var mapObject = MapLoader.GoToMapObject[root.gameObject];
                var builtin = new CustomLogicMapObjectBuiltin(mapObject);
                return builtin;
            }
            return null;
        }
    }
}
