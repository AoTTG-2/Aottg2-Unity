using System.Collections.Generic;
using Utility;

namespace Map
{
    class MapScriptComponent: BaseCSVRowItem
    {
        [Order(1)] public string ComponentName = string.Empty;
        [Order(2)] public List<string> Parameters = new List<string>();
    }
}
