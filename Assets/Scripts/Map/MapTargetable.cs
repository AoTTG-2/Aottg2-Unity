using Settings;
using UnityEngine;
using Characters;

namespace Map
{
    class MapTargetable : ITargetable
    {
        private Transform _transform;
        private Vector3 _center;
        private string _team;

        public MapTargetable(Transform transform, Vector3 center, string team)
        {
            _transform = transform;
            _center = center;
            _team = team;
        }

        public bool ValidTarget()
        {
            return _transform != null;
        }

        public Vector3 GetPosition()
        {
            return _transform.TransformPoint(_center);
        }

        public string GetTeam()
        {
            return _team;
        }
    }
}