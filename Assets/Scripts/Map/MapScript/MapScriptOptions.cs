using UnityEngine;
using Utility;

namespace Map
{
    class MapScriptOptions: BaseCSVRow
    {
        protected override bool NamedParams => true;
        public override char Delimiter => '|';
        public string EditorVersion = "1.0";
        public string Description = string.Empty;
    }
}
