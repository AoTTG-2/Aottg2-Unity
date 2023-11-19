using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace Settings
{
    class SetSettingsContainer<T> : BaseSettingsContainer, ISetSettingsContainer where T : BaseSetSetting, new()
    {
        public IntSetting SelectedSetIndex = new IntSetting(0, minValue: 0);
        public ListSetting<T> Sets = new ListSetting<T>(new T());
        protected virtual bool PresetsOnly => false;
        protected virtual bool AllowPresets => true;

        protected override bool Validate()
        {
            return Sets.GetCount() > 0;
        }

        public BaseSetSetting GetSelectedSet()
        {
            int setIndex = SelectedSetIndex.Value;
            setIndex = Math.Min(setIndex, Sets.GetCount() - 1);
            setIndex = Math.Max(setIndex, 0);
            return (BaseSetSetting)Sets.GetItemAt(setIndex);
        }
        
        public IntSetting GetSelectedSetIndex()
        {
            return SelectedSetIndex;
        }

        public void CreateSet(string name)
        {
            T newSet = new T();
            newSet.Name.Value = name;
            Sets.Value.Add(newSet);
        }

        public void CopySelectedSet(string name)
        {
            T newSet = new T();
            newSet.Copy(GetSelectedSet());
            newSet.Name.Value = name;
            newSet.Preset.Value = false;
            Sets.Value.Add(newSet);
        }

        public bool CanDeleteSelectedSet()
        {
            return Sets.GetCount() > 1 && CanEditSelectedSet();
        }

        public bool CanEditSelectedSet()
        {
            return !GetSelectedSet().Preset.Value;
        }

        public void DeleteSelectedSet()
        {
            Sets.Value.Remove((T)GetSelectedSet());
        }

        public IListSetting GetSets()
        {
            return Sets;
        }

        public void SetPresetsFromJsonString(string json)
        {
            SetSettingsContainer<T> setSettings = new SetSettingsContainer<T>();
            setSettings.DeserializeFromJsonString(json);
            if (!AllowPresets)
            {
                Sets.Value.RemoveAll(x => x.Preset.Value);
                return;
            }
            if (PresetsOnly)
                Sets.Value.Clear();
            else
                Sets.Value.RemoveAll(x => x.Preset.Value);
            for (int i = 0; i < setSettings.Sets.Value.Count; i++)
            {
                setSettings.Sets.Value[i].Preset.Value = true;
                Sets.Value.Insert(i, setSettings.Sets.Value[i]);
            }
        }

        public string[] GetSetNames()
        {
            List<string> names = new List<string>();
            foreach (BaseSetSetting set in Sets.GetItems())
            {
                names.Add(set.Name.Value);
            }
            return names.ToArray();
        }
    }
}
