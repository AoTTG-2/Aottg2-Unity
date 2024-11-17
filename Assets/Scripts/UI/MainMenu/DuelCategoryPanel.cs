﻿using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class DuelCategoryPanel : CategoryPanel
    {
        protected override bool ScrollBar => true;
        protected override string ThemePanel => "ScoreboardPopup";
    }
}
