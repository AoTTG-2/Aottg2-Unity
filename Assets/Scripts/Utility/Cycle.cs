using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Cycle<T> where T : Enum
    {
        private T[] values;
        private int index;
        public Cycle()
        {
            values = (T[])Enum.GetValues(typeof(T));
            index = 0;
        }
        public T Next()
        {
            index = (index + 1) % values.Length;
            return values[index];
        }
        public T Previous()
        {
            index = (index - 1 + values.Length) % values.Length;
            return values[index];
        }
        public T Current()
        {
            return values[index];
        }
        public T Set(T value)
        {
            index = Array.IndexOf(values, value);
            return values[index];
        }
    }
}
