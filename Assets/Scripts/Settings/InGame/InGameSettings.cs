using System;
using UnityEngine;

namespace Settings
{
    class InGameSettings : PresetSettingsContainer
    {
        protected override string FileName { get { return "InGame.json"; } }
        public SetSettingsContainer<InGameSet> InGameSets = new SetSettingsContainer<InGameSet>();
    }
}
