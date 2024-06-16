using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class OutdatedPopup: MessagePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 600f;
        protected override float Height => 300f;
        protected override int VerticalPadding => 30;
        protected override int HorizontalPadding => 30;
        protected override float LabelHeight => 120f;
    }
}
