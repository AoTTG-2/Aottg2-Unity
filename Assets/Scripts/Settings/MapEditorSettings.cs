using UnityEngine;
using UI;
using ApplicationManagers;
using Cameras;

namespace Settings
{
    class MapEditorSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "MapEditor.json"; } }
        public StringSetting CurrentMap = new StringSetting(string.Empty, maxLength: 100);
        public FloatSetting CameraMoveSpeed = new FloatSetting(250f);
        public FloatSetting CameraSlowMoveSpeed = new FloatSetting(50f);
        public FloatSetting CameraFastMoveSpeed = new FloatSetting(750f);
        public FloatSetting CameraRotateSpeed = new FloatSetting(300f);
        public IntSetting RenderDistance = new IntSetting(1500, minValue: 10, maxValue: 1000000);
        public FloatSetting SnapMove = new FloatSetting(10f, minValue: 0f);
        // public FloatSetting SnapRotate = new FloatSetting(15f, minValue: 0f);
        // public FloatSetting SnapScale = new FloatSetting(0.1f, minValue: 0f);

        public override void Apply()
        {
            if (SceneLoader.CurrentCamera is MapEditorCamera)
                ((MapEditorCamera)SceneLoader.CurrentCamera).ApplyGraphicsSettings();
        }
    }
}
