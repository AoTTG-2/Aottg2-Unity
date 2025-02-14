using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using Map;
using GameManagers;
using Cameras;

namespace MapEditor
{
    class BaseGizmo : MonoBehaviour
    {
        protected MapEditorGameManager _gameManager;
        protected Transform _transform;
        protected MapEditorMenu _menu;

        public virtual bool IsActive()
        {
            return false;
        }

        protected virtual void Awake()
        {
            _gameManager = ((MapEditorGameManager)SceneLoader.CurrentGameManager);
            _menu = (MapEditorMenu)UIManager.CurrentMenu;
            _transform = transform;
        }

        protected virtual void Update()
        {
            float distance = Vector3.Distance(SceneLoader.CurrentCamera.Cache.Transform.position, _transform.position) / 200f;
            if (SceneLoader.CurrentCamera.Camera.orthographic)
            {
                distance = 10f * SceneLoader.CurrentCamera.Camera.orthographicSize / Screen.height;
            }
            _transform.localScale = Vector3.one * distance;
        }

        public virtual void OnSelectionChange()
        {
        }
    }
}
