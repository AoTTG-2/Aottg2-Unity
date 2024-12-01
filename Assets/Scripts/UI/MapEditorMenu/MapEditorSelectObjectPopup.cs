using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Settings;

namespace UI
{
    class MapEditorSelectObjectPopup : BasePopup
   {
        protected override string Title => "Load Errors";
        protected override float Width => 500f;
        protected override float Height => 590f;
        protected override int VerticalPadding => 20;
        protected override int HorizontalPadding => 20;
    }
}
