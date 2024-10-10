using UnityEngine;
using Characters;

namespace Map
{
    class MapTargetable : ITargetable
    {
        private Transform _transform;
        private Vector3 _center;

        public string Team { get; set; }
        public bool Enabled { get; set; } = true;

        public MapTargetable(Transform transform, Vector3 center, string team)
        {
            _transform = transform;
            _center = center;
            Team = team;
        }

        public bool ValidTarget()
        {
            return Enabled && _transform != null;
        }

        public Vector3 GetPosition()
        {
            return _transform.TransformPoint(_center);
        }

        public string GetTeam()
        {
            return Team;
        }
    }
}