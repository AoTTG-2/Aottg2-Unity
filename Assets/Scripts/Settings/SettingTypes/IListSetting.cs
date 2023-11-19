using System.Collections;
using System.Collections.Generic;
using SimpleJSONFixed;
using UnityEngine;

namespace Settings
{
    interface IListSetting
    {
        int GetCount();
        BaseSetting GetItemAt(int index);
        List<BaseSetting> GetItems();
        void AddItem(BaseSetting item);
        void Clear();
    }
}
