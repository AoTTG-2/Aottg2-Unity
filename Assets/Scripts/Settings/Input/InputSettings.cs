using UnityEngine;

namespace Settings
{
    class InputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "Input.json"; } }
        public GeneralInputSettings General = new GeneralInputSettings();
        public HumanInputSettings Human = new HumanInputSettings();
        public TitanInputSettings Titan = new TitanInputSettings();
        public ShifterInputSettings Shifter = new ShifterInputSettings();
        public InteractionInputSettings Interaction = new InteractionInputSettings();
        public MapEditorInputSettings MapEditor = new MapEditorInputSettings();
    }
}
