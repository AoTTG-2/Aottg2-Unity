using Characters;
using System;
using UnityEngine;

namespace Settings
{
    class HumanCustomSettings : PresetSettingsContainer
    {
        protected override string FileName { get { return "HumanCustom.json"; } }
        public SetSettingsContainer<HumanCustomSet> CustomSets = new SetSettingsContainerNoPresets<HumanCustomSet>();
        public SetSettingsContainer<HumanCustomSet> Costume1Sets = new SetSettingsContainerPresetsOnly<HumanCustomSet>();
        public SetSettingsContainer<HumanCustomSet> Costume2Sets = new SetSettingsContainerPresetsOnly<HumanCustomSet>();
        public SetSettingsContainer<HumanCustomSet> Costume3Sets = new SetSettingsContainerPresetsOnly<HumanCustomSet>();
    }
}
