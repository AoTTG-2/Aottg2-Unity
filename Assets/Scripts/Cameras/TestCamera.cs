using UnityEngine;
using Utility;
using Settings;
using UI;
using Weather;
using System.Collections;
using GameProgress;
using Map;
using GameManagers;
using Events;

namespace Cameras
{
    class TestCamera : BaseCamera
    {
        private const float DefaultDistance = 30f;
        private Vector3 AnchorPoint = new Vector3(0f, 0f, 0f);
        private float MinDistance = 1f;
        private float MaxDistance = 100f;

        protected override void SetDefaultCameraPosition()
        {
            Cache.Transform.position = new Vector3(0f, 0f, -DefaultDistance);
            Cache.Transform.LookAt(AnchorPoint);
        }

        protected void Update()
        {
            float mouseX = Input.mousePosition.x;
            float rotationSpeed = Time.deltaTime * 150f;
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            {
                float inputX = Input.GetAxis("Mouse X");
                float inputY = Input.GetAxis("Mouse Y");
                if (inputX < 0f)
                    Cache.Transform.RotateAround(AnchorPoint, Vector3.up, -rotationSpeed);
                else if (inputX > 0f)
                    Cache.Transform.RotateAround(AnchorPoint, Vector3.up, rotationSpeed);
                if (inputY < 0f)
                    Cache.Transform.RotateAround(AnchorPoint, Cache.Transform.right, rotationSpeed);
                else if (inputY > 0f)
                    Cache.Transform.RotateAround(AnchorPoint, Cache.Transform.right, -rotationSpeed);
            }
            float inputScroll = Input.GetAxis("Mouse ScrollWheel");
            float scrollSpeed = Time.deltaTime * 30f;
            Vector3 direction = (Cache.Transform.position - AnchorPoint).normalized;
            float distance = Vector3.Distance(AnchorPoint, Cache.Transform.position);
            if (inputScroll < 0f)
                Cache.Transform.position = AnchorPoint + direction * Mathf.Min(MaxDistance, distance + scrollSpeed);
            else if (inputScroll > 0f)
                Cache.Transform.position = AnchorPoint + direction * Mathf.Max(MinDistance, distance - scrollSpeed);
        }
    }
}