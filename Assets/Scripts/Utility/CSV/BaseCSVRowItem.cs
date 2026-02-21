using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utility
{
    public class BaseCSVRowItem: BaseCSVObject
    {
        public override char Delimiter => '|';
    }
}
