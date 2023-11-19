using System.Collections.Generic;
using SimpleJSONFixed;
using UnityEngine;

namespace Settings
{
    class ListSetting<T> : TypedSetting<List<T>>, IListSetting where T : BaseSetting, new()
    {
        public ListSetting(List<T> defaultValue) : base(defaultValue)
        {
        }

        public ListSetting(T defaultValue)
        {
            DefaultValue = new List<T>() { defaultValue };
            SetDefault();
        }

        public ListSetting(T defaultValue, int count)
        {
            List<T> settings = new List<T>();
            JSONNode json = defaultValue.SerializeToJsonObject();
            for (int i = 0; i < count; i++)
            {
                T setting = new T();
                CopyLimits(defaultValue, setting);
                setting.DeserializeFromJsonObject(json);
                settings.Add(setting);
            }
            DefaultValue = settings;
            SetDefault();
        }

        public ListSetting()
        {
            DefaultValue = new List<T>();
            SetDefault();
        }

        public override void SetDefault()
        {
            List<T> settings = new List<T>();
            foreach (T setting in DefaultValue)
            {
                T copy = new T();
                CopyDefaultLimits(copy);
                copy.DeserializeFromJsonObject(setting.SerializeToJsonObject());
                settings.Add(copy);
            }
            Value = settings;
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            List<T> newValue = new List<T>();
            JSONArray array = json.AsArray;
            foreach (JSONNode node in array)
            {
                T setting = new T();
                CopyDefaultLimits(setting);
                setting.DeserializeFromJsonObject(node);
                newValue.Add(setting);
            }
            Value = newValue;
        }

        public override JSONNode SerializeToJsonObject()
        {
            JSONArray array = new JSONArray();
            foreach (BaseSetting setting in Value)
                array.Add(setting.SerializeToJsonObject());
            return array;
        }

        public int GetCount()
        {
            return Value.Count;
        }

        public BaseSetting GetItemAt(int index)
        {
            return Value[index];
        }

        public List<BaseSetting> GetItems()
        {
            List<BaseSetting> settings = new List<BaseSetting>();
            foreach (BaseSetting setting in Value)
                settings.Add(setting);
            return settings;
        }

        public void AddItem(BaseSetting item)
        {
            Value.Add((T)item);
        }

        public void Clear()
        {
            Value.Clear();
        }

        private void CopyLimits(T from, T to)
        {
            if (from is IntSetting)
            {
                ((IntSetting)(object)to).MinValue = ((IntSetting)(object)from).MinValue;
                ((IntSetting)(object)to).MaxValue = ((IntSetting)(object)from).MaxValue;
            }
            else if (from is ColorSetting)
                ((ColorSetting)(object)to).MinAlpha = ((ColorSetting)(object)from).MinAlpha;
            else if (from is FloatSetting)
            {
                ((FloatSetting)(object)to).MinValue = ((FloatSetting)(object)from).MinValue;
                ((FloatSetting)(object)to).MaxValue = ((FloatSetting)(object)from).MaxValue;
            }
            else if (from is StringSetting)
                ((StringSetting)(object)to).MaxLength = ((StringSetting)(object)from).MaxLength;
        }

        private void CopyDefaultLimits(T to)
        {
            if (DefaultValue.Count > 0)
            {
                CopyLimits(DefaultValue[0], to);
            }
        }
    }
}
