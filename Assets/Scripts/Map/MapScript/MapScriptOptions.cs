using UnityEngine;
using Utility;

namespace Map
{
    public class MapScriptOptions: BaseCSVRow
    {
        protected override bool NamedParams => true;
        public override char Delimiter => '|';
        public string EditorVersion = "1.0";
        public string Description = string.Empty;
        public bool HasWeather = false;
        // public string Background = "None";
        // public Vector3 BackgroundPosition = Vector3.zero;
        // public Vector3 BackgroundRotation = Vector3.zero;
    }
}
