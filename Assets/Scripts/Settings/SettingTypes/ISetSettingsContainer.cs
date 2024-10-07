namespace Settings
{
    interface ISetSettingsContainer
    {
        BaseSetSetting GetSelectedSet();
        IntSetting GetSelectedSetIndex();
        IListSetting GetSets();
        string[] GetSetNames();
        void CreateSet(string name);
        void CopySelectedSet(string name);
        void DeleteSelectedSet();
        bool CanDeleteSelectedSet();
        bool CanEditSelectedSet();
        void SetPresetsFromJsonString(string json);
    }
}
