using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class MapEditorSelectComponentPopup: SelectListPopup
    {
        protected override float VerticalSpacing => 5f;
        protected override int ItemFontSize => 18;
        protected override float DeleteButtonSize => 20f;
    }
}
