using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utility
{
    // define a generic class that takes in two type parameters, the first is the key type and the second is the value type, the key will be the only part used in order to determine the uniqueness of the item'
    public class SetItem<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public SetItem(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            SetItem<TKey, TValue> item = (SetItem<TKey, TValue>)obj;
            return Key.Equals(item.Key);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}
