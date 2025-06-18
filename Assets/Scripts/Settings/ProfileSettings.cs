using UnityEngine;
using UI;
using System;

namespace Settings
{
    class ProfileSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "Profile.json"; } }
        public StringSetting ProfileIcon = new StringSetting("Levi1", maxLength: 100);
        public NameSetting Name = new NameSetting("GUEST" + UnityEngine.Random.Range(0, 100000), maxLength: 200, maxStrippedLength: 18);
        public NameSetting Guild = new NameSetting(string.Empty, maxLength: 200, maxStrippedLength: 18);
        public StringSetting Social = new StringSetting(string.Empty, maxLength: 100);
        public StringSetting About = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting ID = new StringSetting(Guid.NewGuid().ToString(), maxLength: 200);
    }
}
