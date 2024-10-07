using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    class ChatSettings : SaveableSettingsContainer
    {
        protected override string FileName
        {
            get { return "ChatFilter.json"; }
        }

        public readonly Dictionary<string, string> filter_patterns = new Dictionary<
            string,
            string
        >()
        {
            { @".*(n[i1]gg(?:(?=r)r|(?=[e3])[e3]r|(?=[a4])[a4])).*", "" },
            { @".*(beaner).*", "" },
            { @".*(buddhahead).*", "" },
            { @".*(camel-jockey).*", "" },
            { @".*(cheese-monkey).*", "" },
            { @".*(chink).*", "" },
            { @".*(coon).*", "" },
            { @".*(curry-muncher).*", "" },
            { @".*(darkie).*", "" },
            { @".*(dune-coon).*", "" },
            { @".*(gook).*", "" },
            { @".*(jigaboo).*", "" },
            { @".*(kike).*", "" },
            { @".*(paki).*", "" },
            { @".*(petrol-sniffer).*", "" },
            { @".*(pikey).*", "" },
            { @".*(slanteye).*", "" },
            { @".*(spearchucker).*", "" },
            { @".*(spic).*", "" },
            { @".*(swamp-guinea).*", "" },
            { @".*(towelhead).*", "" },
            { @".*(wetback).*", "" },
            { @".*(white power).*", "" },
            { @".*(black power).*", "" },
            { @".*(zipperhead).*", "" },
            { @".*(honkey).*", "" },
            { @".*(honky).*", "" },
            { @".*(fagg[0o]t).*", "" },
            { @".*(moon-cricket).*", "" },
            { @".*(slurpie).*", "" },
            { @".*(pajeet).*", "" },
            { @".*(phosphorus).*", "" },
        };

        public readonly bool FilterEnabled = true;

        public void AddFilter(string newFilter, string replacement = "")
        {
            filter_patterns.Add(newFilter, replacement);
        }

        public void RemoveFilter(string filterName)
        {
            filter_patterns.Remove(filterName);
        }
    }
}
