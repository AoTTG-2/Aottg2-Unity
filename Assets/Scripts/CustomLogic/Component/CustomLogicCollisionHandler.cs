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
            var builtin = GetBuiltin(other);
            if (builtin == null)
                return;
            foreach (var classInstance in _classInstances)
                classInstance.OnCollisionEnter(builtin);
        }

        protected void OnTriggerStay(Collider other)
        {
            var builtin = GetBuiltin(other);
            if (builtin == null)
                return;
            foreach (var classInstance in _classInstances)
                classInstance.OnCollisionStay(builtin);
        }

        protected void OnTriggerExit(Collider other)
        {
            var builtin = GetBuiltin(other);
            if (builtin == null)
                return;
            foreach (var classInstance in _classInstances)
                classInstance.OnCollisionExit(builtin);
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
