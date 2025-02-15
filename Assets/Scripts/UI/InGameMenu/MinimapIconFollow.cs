using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using Characters;
using GameManagers;
using ApplicationManagers;
using Utility;

namespace UI
{
    class MinimapIconFollow : MonoBehaviour
    {
        private BaseCharacter _character;
        private float _scaleOffset;
        private bool _isStatic;

        /*
        public void Init(float height, BaseCharacter character)
        {
            Setup(height);
            _character = character;
            _isStatic = false;
        }

        public void Init(float height, Transform staticTransform)
        {
            Setup(height);
            _isStatic = true;
            var position = staticTransform.position;
            transform.position = new Vector3(position.x, _cameraTransform.position.y - _scaleOffset * 0.5f - 10, position.z);
        }

        private void Setup(float height)
        {
            _scaleOffset = height * 0.1f;
            transform.localScale = Vector3.one * height;
        }

        private void Update()
        {
            if (!_isStatic)
            {
                if (_character == null || _character.Dead)
                {
                    Destroy(gameObject);
                    return;
                }
                var position = _character.Cache.Transform.position;
                transform.position = new Vector3(position.x, _cameraTransform.position.y - _scaleOffset * 0.5f - 10, position.z);
            }
            transform.rotation = _cameraTransform.rotation;
            transform.RotateAround(transform.position, Vector3.up, 180f);
        }
        */

    }
}
