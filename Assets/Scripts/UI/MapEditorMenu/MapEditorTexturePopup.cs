using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using ApplicationManagers;
using GameManagers;

namespace UI
{
    class MapEditorTexturePopup : MapEditorAddObjectPopup
    {
        protected override string DefaultCategoryPanel => "All";
        protected override string[] GetCategories()
        {
            return new string[] { "All", "Nature", "Brick", "Metal", "Wood", "Stone", "Misc" };
        }

        protected override void RegisterCategoryPanels()
        {
            foreach (string buttonName in _topButtons.Keys)
                _categoryPanelTypes.Add(buttonName, typeof(MapEditorTexturePanel));
        }
    }
}
