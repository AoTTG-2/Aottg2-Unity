using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Settings
{
    class CustomSkinSettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "CustomSkins.json"; } }
        public HumanCustomSkinSettings Human = new HumanCustomSkinSettings();
        public BaseCustomSkinSettings<TitanCustomSkinSet> Titan = new BaseCustomSkinSettings<TitanCustomSkinSet>();
        public BaseCustomSkinSettings<ShifterCustomSkinSet> Shifter = new BaseCustomSkinSettings<ShifterCustomSkinSet>();
        public BaseCustomSkinSettings<SkyboxCustomSkinSet> Skybox = new BaseCustomSkinSettings<SkyboxCustomSkinSet>();
        //public BaseCustomSkinSettings<ForestCustomSkinSet> Forest = new BaseCustomSkinSettings<ForestCustomSkinSet>();
        //public BaseCustomSkinSettings<CityCustomSkinSet> City = new BaseCustomSkinSettings<CityCustomSkinSet>();
    }
}
