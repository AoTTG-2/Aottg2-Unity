using ApplicationManagers;
using System;
using UnityEngine;
using Cameras;

namespace Settings
{
    class EMSettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "EMSettings.json"; } }

        public StringSetting TPcoords = new StringSetting("0 0 0");
        /* 
           public StringSetting Language = new StringSetting("English");
           public FloatSetting MouseSpeed = new FloatSetting(0.5f, minValue: 0.01f, maxValue: 1f);
           public BoolSetting MinimapEnabled = new BoolSetting(true);
           public IntSetting SnapshotsMinimumDamage = new IntSetting(0, minValue: 0);
        */



        public override void Apply()
        {
            //Zippy: Apply Em Settings live here
        }
    }
}
