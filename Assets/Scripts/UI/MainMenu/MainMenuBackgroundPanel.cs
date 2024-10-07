using ApplicationManagers;
using GameManagers;
using Settings;
using SimpleJSONFixed;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class MainMenuBackgroundPanel : LoadingBackgroundPanel
    {
        protected override float AnimationTime => 1f;
    }
}
