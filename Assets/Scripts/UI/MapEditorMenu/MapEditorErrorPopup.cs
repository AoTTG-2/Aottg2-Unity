using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class MapEditorErrorPopup: ExportPopup
    {
        protected override string Title => "Load Errors";
        protected override float Width => 500f;
        protected override float Height => 590f;
        protected override int VerticalPadding => 20;
        protected override int HorizontalPadding => 20;
    }
}
