using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Map
{
    class MapScriptBaseObject: BaseCSVRow
    {
        // info
        [Order(1)] public string Type;
        [Order(2)] public string Asset = "None";
        [Order(3)] public int Id;
        [Order(4)] public int Parent = 0;
        [Order(5)] public bool Active = true;
        [Order(6)] public bool Static = true;
        [Order(7)] public bool Visible = true;
        [Order(8)] public bool Networked = false;
        [Order(9)] public string Name = "Unnamed";

        // transform
        [Order(10)] public float PositionX;
        [Order(11)] public float PositionY;
        [Order(12)] public float PositionZ;
        [Order(13)] public float RotationX;
        [Order(14)] public float RotationY;
        [Order(15)] public float RotationZ;
        [Order(16)] public float ScaleX = 1f;
        [Order(17)] public float ScaleY = 1f;
        [Order(18)] public float ScaleZ = 1f;

        public void SetPosition(Vector3 position)
        {
            PositionX = position.x;
            PositionY = position.y;
            PositionZ = position.z;
        }

        public void SetScale(Vector3 scale)
        {
            ScaleX = scale.x;
            ScaleY = scale.y;
            ScaleZ = scale.z;
        }

        public void SetRotation(Vector3 rotation)
        {
            RotationX = rotation.x;
            RotationY = rotation.y;
            RotationZ = rotation.z;
        }

        public void SetRotation(Quaternion rotation)
        {
            SetRotation(rotation.eulerAngles);
        }

        public Vector3 GetPosition()
        {
            return new Vector3(PositionX, PositionY, PositionZ);
        }

        public Vector3 GetRotation()
        {
            return new Vector3(RotationX, RotationY, RotationZ);
        }

        public Vector3 GetScale()
        {
            return new Vector3(ScaleX, ScaleY, ScaleZ);
        }
    }
}
