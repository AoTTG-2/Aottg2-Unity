using System.Collections.Generic;

namespace Settings
{
    class ChatSettings : SaveableSettingsContainer
    {
        protected override string FileName
        {
            get { return "ChatFilter.json"; }
        }

        public DictionarySetting<StringSetting, StringSetting> filters = new DictionarySetting<
            StringSetting,
            StringSetting
        >(
            new Dictionary<StringSetting, StringSetting>()
            {
                {
                    new StringSetting(@".*(n[i1]gg(?:(?=r)r|(?=[e3])[e3]r|(?=[a4])[a4])).*"),
                    new StringSetting("")
                },
                { new StringSetting(@".*(beaner).*"), new StringSetting("") },
                { new StringSetting(@".*(buddhahead).*"), new StringSetting("") },
                { new StringSetting(@".*(camel-jockey).*"), new StringSetting("") },
                { new StringSetting(@".*(cheese-monkey).*"), new StringSetting("") },
                { new StringSetting(@".*(chink).*"), new StringSetting("") },
                { new StringSetting(@".*(coon).*"), new StringSetting("") },
                { new StringSetting(@".*(curry-muncher).*"), new StringSetting("") },
                { new StringSetting(@".*(darkie).*"), new StringSetting("") },
                { new StringSetting(@".*(dune-coon).*"), new StringSetting("") },
                { new StringSetting(@".*(gook).*"), new StringSetting("") },
                { new StringSetting(@".*(jigaboo).*"), new StringSetting("") },
                { new StringSetting(@".*(kike).*"), new StringSetting("") },
                { new StringSetting(@".*(paki).*"), new StringSetting("") },
                { new StringSetting(@".*(petrol-sniffer).*"), new StringSetting("") },
                { new StringSetting(@".*(pikey).*"), new StringSetting("") },
                { new StringSetting(@".*(slanteye).*"), new StringSetting("") },
                { new StringSetting(@".*(spearchucker).*"), new StringSetting("") },
                { new StringSetting(@".*(spic).*"), new StringSetting("") },
                { new StringSetting(@".*(swamp-guinea).*"), new StringSetting("") },
                { new StringSetting(@".*(towelhead).*"), new StringSetting("") },
                { new StringSetting(@".*(wetback).*"), new StringSetting("") },
                { new StringSetting(@".*(white power).*"), new StringSetting("") },
                { new StringSetting(@".*(black power).*"), new StringSetting("") },
                { new StringSetting(@".*(zipperhead).*"), new StringSetting("") },
                { new StringSetting(@".*(honkey).*"), new StringSetting("") },
                { new StringSetting(@".*(honky).*"), new StringSetting("") },
                { new StringSetting(@".*(fagg[0o]t).*"), new StringSetting("") },
                { new StringSetting(@".*(moon-cricket).*"), new StringSetting("") },
                { new StringSetting(@".*(slurpie).*"), new StringSetting("") },
                { new StringSetting(@".*(pajeet).*"), new StringSetting("") },
                { new StringSetting(@".*(phosphorus).*"), new StringSetting("") },
                { new StringSetting(@".*(phosphorus).*"), new StringSetting("") }
            }
        );

        public readonly bool FilterEnabled = true;
    }
}
