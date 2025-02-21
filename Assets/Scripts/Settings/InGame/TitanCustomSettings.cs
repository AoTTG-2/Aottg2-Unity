using Characters;
using System;
using UnityEngine;

namespace Settings
{
    class TitanCustomSettings : PresetSettingsContainer
    {
        protected override string FileName { get { return "TitanCustom.json"; } }
        public SetSettingsContainer<TitanCustomSet> TitanCustomSets = new SetSettingsContainerNoPresets<TitanCustomSet>();
    }
}
