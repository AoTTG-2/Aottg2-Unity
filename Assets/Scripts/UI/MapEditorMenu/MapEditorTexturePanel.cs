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
using Map;
using Utility;

namespace UI
{
    class MapEditorTexturePanel : MapEditorAddObjectPanel
    {
        protected override List<string> GetItems(string category)
        {
            if (category == "All")
                return BuiltinMapTextures.AllTexturesNoLegacy.Keys.ToList();
            else
                return BuiltinMapTextures.TextureCategories[category].Select(x => x.Texture.Split('/')[1]).ToList();
        }

        protected override string GetPreviewName(string item)
        {
            return item + "TexturePreview";
        }

        protected override void OnSelectObject(string name)
        {
            ((MapEditorMenu)UIManager.CurrentMenu).InspectPanel.OnSelectTexture(name);
            Parent.Hide();
        }
    }
}
