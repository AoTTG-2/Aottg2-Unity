using UnityEngine;
using SimpleJSONFixed;

namespace Characters
{
    class TitanAttackKeyframe
    {
        public int Frame;
        private Vector3 _localPosition;
        private float _radius;

        public TitanAttackKeyframe(JSONNode data)
        {
            Frame = data["f"].AsInt;
            _localPosition = new Vector3(data["x"].AsFloat, data["y"].AsFloat, data["z"].AsFloat);
            _radius = data["r"].AsFloat;
        }

        public bool CheckCollision(Transform titan, Vector3 position, Vector3 velocity, float attackSpeed, float size)
        {
            var worldPosition = titan.TransformPoint(_localPosition);
            // var predictedPosition = position + velocity * Frame * Time.fixedDeltaTime / attackSpeed;
            var predictedPosition = position;

            // this is slightly faster than Vector3.Distance
            float radiusSquared = Mathf.Pow(_radius * size, 2);
            return ((predictedPosition - worldPosition).sqrMagnitude <= radiusSquared);
        }
    }
}
