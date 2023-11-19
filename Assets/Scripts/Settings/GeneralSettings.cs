using ApplicationManagers;
using System;
using UnityEngine;
using Cameras;

namespace Settings
{
    class GeneralSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "General.json"; } }
        public StringSetting Language = new StringSetting("English");
        public FloatSetting MouseSpeed = new FloatSetting(0.5f, minValue: 0.01f, maxValue: 1f);
        public FloatSetting CameraDistance = new FloatSetting(1f, minValue: 0f, maxValue: 1f);
        public FloatSetting CameraHeight = new FloatSetting(1f, minValue: 0f, maxValue: 2f);
        public BoolSetting InvertMouse = new BoolSetting(false);
        public BoolSetting CameraTilt = new BoolSetting(true);
        public BoolSetting SnapshotsEnabled = new BoolSetting(false);
        public BoolSetting SnapshotsShowInGame = new BoolSetting(false);
        public IntSetting SnapshotsMinimumDamage = new IntSetting(0, minValue: 0);
        public BoolSetting MinimapEnabled = new BoolSetting(true);
        public FloatSetting MinimapHeight = new FloatSetting(500, minValue: 1);
        public IntSetting CameraMode = new IntSetting((int)CameraInputMode.TPS);
        public BoolSetting SkipCutscenes = new BoolSetting(false);
        public FloatSetting FOVMin = new FloatSetting(50f, minValue: 1f, maxValue: 120f);
        public FloatSetting FOVMax = new FloatSetting(100f, minValue: 1f, maxValue: 120f);
        public FloatSetting FPSFOVMin = new FloatSetting(80f, minValue: 1f, maxValue: 120f);
        public FloatSetting FPSFOVMax = new FloatSetting(100f, minValue: 1f, maxValue: 120f);

        public override void Apply()
        {
            if (SceneLoader.CurrentCamera is InGameCamera)
                ((InGameCamera)SceneLoader.CurrentCamera).ApplyGeneralSettings();
        }
    }

    public enum CameraInputMode
    {
        TPS,
        Original
    }
}
