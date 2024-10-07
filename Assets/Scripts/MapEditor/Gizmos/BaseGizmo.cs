using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using Map;
using GameManagers;

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

        public virtual void OnSelectionChange()
        {
        }
    }
}
