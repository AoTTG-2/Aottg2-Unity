using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    class TitanCustomSkinSet: BaseSetSetting
    {
        public BoolSetting RandomizedPairs = new BoolSetting(false);
        public ListSetting<StringSetting> Hairs = new ListSetting<StringSetting>(new StringSetting(string.Empty, maxLength: 200), 8);
        public ListSetting<IntSetting> HairModels = new ListSetting<IntSetting>(new IntSetting(-1, minValue: -1), 8);
        public ListSetting<StringSetting> Bodies = new ListSetting<StringSetting>(new StringSetting(string.Empty, maxLength: 200), 8);
        public ListSetting<StringSetting> BodyModels = new ListSetting<StringSetting>(new StringSetting(string.Empty, maxLength: 200), 8);
        public ListSetting<StringSetting> Heads = new ListSetting<StringSetting>(new StringSetting(string.Empty, maxLength: 200), 8);
        public ListSetting<IntSetting> HeadModels = new ListSetting<IntSetting>(new IntSetting(-1, minValue: -1), 8);
        public ListSetting<StringSetting> Eyes = new ListSetting<StringSetting>(new StringSetting(string.Empty, maxLength: 200), 8);

        protected override bool Validate()
        {
            return Hairs.Value.Count == 8 && HairModels.Value.Count == 8 && Bodies.Value.Count == 8 && Eyes.Value.Count == 8
                && BodyModels.Value.Count == 8 && Heads.Value.Count == 8 && HeadModels.Value.Count == 8;
        }
    }
}
