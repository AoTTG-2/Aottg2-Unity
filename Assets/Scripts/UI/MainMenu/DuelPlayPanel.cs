using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using ApplicationManagers;
using GameManagers;
using Photon.Realtime;
using Photon.Pun;
using Utility;
using CustomLogic;

namespace UI
{
    class DuelPlayPanel: DuelCategoryPanel
    {
        private List<Transform> _rows = new List<Transform>();
        private Transform _header;
        private const float MaxSyncDelay = 0.2f;
        private float _currentSyncDelay = 0.2f;
        protected override float VerticalSpacing => 10f;
        protected override int VerticalPadding => 15;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
        }
       
    }
}
