using ApplicationManagers;
using System;
using UnityEngine;
using Cameras;

namespace Settings
{
    class SoundSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "Sound.json"; } }
        public FloatSetting Volume = new FloatSetting(0.5f, minValue: 0f, maxValue: 1f);
        public FloatSetting Music = new FloatSetting(0.5f, minValue: 0f, maxValue: 1f);
        public BoolSetting TitanGrabMusic = new BoolSetting(true);
        public BoolSetting TitanVocalEffect = new BoolSetting(true);
        public BoolSetting GasEffect = new BoolSetting(true);
        public BoolSetting ReelInEffect = new BoolSetting(true);
        public BoolSetting ReelOutEffect = new BoolSetting(true);
        public BoolSetting HookRetractEffect = new BoolSetting(true);
        public BoolSetting HookImpactEffect = new BoolSetting(true);
        public BoolSetting OldHookEffect = new BoolSetting(false);
        public BoolSetting OldBladeEffect = new BoolSetting(false);
        public BoolSetting OldNapeEffect = new BoolSetting(false);
        public StringSetting ForcePlaylist = new StringSetting("Default");
        public StringSetting CustomPlaylist = new StringSetting("");

        public override void Apply()
        {
            AudioListener.volume = Volume.Value;
            MusicManager.ApplySoundSettings();
        }
    }
}
