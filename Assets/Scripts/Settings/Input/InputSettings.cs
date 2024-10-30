using UnityEngine;

namespace Settings
{
    class InputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "Input.json"; } }
        public GeneralInputSettings General = new GeneralInputSettings();
        public HumanInputSettings Human = new HumanInputSettings();
        public TitanInputSettings Titan = new TitanInputSettings();
        public AnnieShifterInputSettings AnnieShifter = new AnnieShifterInputSettings();
        public ErenShifterInputSettings ErenShifter = new ErenShifterInputSettings();
        public InteractionInputSettings Interaction = new InteractionInputSettings();
        public MapEditorInputSettings MapEditor = new MapEditorInputSettings();
    }
}
