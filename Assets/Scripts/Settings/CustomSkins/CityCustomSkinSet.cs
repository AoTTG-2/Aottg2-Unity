using SimpleJSONFixed;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    class CityCustomSkinSet : BaseSetSetting
    {
        public ListSetting<StringSetting> Houses = new ListSetting<StringSetting>(new StringSetting(string.Empty, maxLength: 200), 8);
        public StringSetting Ground = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Wall = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Gate = new StringSetting(string.Empty, maxLength: 200);

        protected override bool Validate()
        {
            return Houses.Value.Count == 8;
        }
    }
}
