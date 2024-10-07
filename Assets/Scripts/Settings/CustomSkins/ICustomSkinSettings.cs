namespace Settings
{
    interface ICustomSkinSettings: ISetSettingsContainer
    {
        BoolSetting GetSkinsLocal();
        BoolSetting GetSkinsEnabled();
        IListSetting GetSkinSets();
    }
}
