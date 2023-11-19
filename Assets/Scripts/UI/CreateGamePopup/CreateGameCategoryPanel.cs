using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class CreateGameCategoryPanel : CategoryPanel
    {
        protected override bool DoublePanel => true;
        protected override bool DoublePanelDivider => true;
    }
}
