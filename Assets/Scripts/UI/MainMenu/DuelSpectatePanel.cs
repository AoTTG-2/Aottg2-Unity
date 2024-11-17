using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using ApplicationManagers;
using GameManagers;
using System.Collections;
using Utility;
using System.Collections.Specialized;
using Photon.Pun;
using static UnityEngine.Rendering.DebugUI;
using CustomLogic;

namespace UI
{
    class DuelSpectatePanel: DuelCategoryPanel
    {
        protected override bool DoublePanel => true;
        protected override float VerticalSpacing => 15f;
        protected override int VerticalPadding => 15;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
        }
    }
}
