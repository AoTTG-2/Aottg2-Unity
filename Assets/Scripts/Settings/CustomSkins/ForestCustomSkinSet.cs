using System.Collections.Generic;

namespace Settings
{
    class ForestCustomSkinSet : BaseSetSetting
    {
        public BoolSetting RandomizedPairs = new BoolSetting(false);
        public ListSetting<StringSetting> TreeTrunks = new ListSetting<StringSetting>(new StringSetting(string.Empty, maxLength: 200), 8);
        public ListSetting<StringSetting> TreeLeafs = new ListSetting<StringSetting>(new StringSetting(string.Empty, maxLength: 200), 8);
        public StringSetting Ground = new StringSetting(string.Empty, maxLength: 200);

        protected override bool Validate()
        {
            return TreeTrunks.Value.Count == 8 && TreeLeafs.Value.Count == 8;
        }
    }
}
