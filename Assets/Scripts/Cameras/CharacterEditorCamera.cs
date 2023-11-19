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
    class CharacterEditorCamera : BaseCamera
    {
        private const float MaxDistance = 4f;
        private const float MinDistance = 1f;
        private const float DefaultDistance = 3f;
        private Vector3 AnchorPoint = new Vector3(0f, 1.1f, 0f);

        protected override void SetDefaultCameraPosition()
        {
            Cache.Transform.position = new Vector3(0f, 1.1f, DefaultDistance);
            Cache.Transform.LookAt(AnchorPoint);
        }

        protected void Update()
        {
            float mouseX = Input.mousePosition.x;
            if (UIManager.CurrentMenu == null || UIManager.CurrentMenu.GetComponent<CharacterEditorMenu>() == null)
                return;
            CharacterEditorMenu menu = (CharacterEditorMenu)UIManager.CurrentMenu;
            if (mouseX < menu.GetMinMouseX() || mouseX > menu.GetMaxMouseX() || menu.IsPopupActive())
                return;
            float rotationSpeed = Time.deltaTime * 200f;
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            {
                float inputX = Input.GetAxis("Mouse X");
                float inputY = Input.GetAxis("Mouse Y");
                Cache.Transform.RotateAround(AnchorPoint, Vector3.up, inputX * rotationSpeed);
                Cache.Transform.RotateAround(AnchorPoint, Cache.Transform.right, -1f * inputY * rotationSpeed);
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