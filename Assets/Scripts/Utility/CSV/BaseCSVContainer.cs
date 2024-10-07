using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Utility
{
    class BaseCSVContainer: BaseCSVObject
    {
        public override char Delimiter => ';';
        protected virtual bool UseNewlines => true;

        public override string Serialize()
        {
            string serialized = base.Serialize();
            if (UseNewlines)
                serialized = InsertNewlines(serialized);
            return serialized;
        }

        public virtual string InsertNewlines(string str)
        {
            string[] strArray = str.Split(Delimiter);
            return string.Join(Delimiter.ToString() + "\n", strArray);
        }
    }
}
