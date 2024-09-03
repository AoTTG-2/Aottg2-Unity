using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using ApplicationManagers;
using GameManagers;
using Characters;
using Utility;

namespace UI
{
    class CharacterEditorCostumePanel: HeadedPanel
    {
        protected override string Title => UIManager.GetLocale("CharacterEditor", "Costume", "Title");
        protected override float Width => 380f;
        protected override float Height => 1020f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 25;
        protected override int VerticalPadding => 25;
        protected override bool ScrollBar => true;
        private CharacterEditorMenu _menu;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _menu = (CharacterEditorMenu)UIManager.CurrentMenu;
            ElementStyle style = new ElementStyle(titleWidth: 130f, themePanel: ThemePanel);
            HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
            string cat = "CharacterEditor";
            string sub = "Costume";
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("LoadPreset"), onClick: () => OnButtonClick("LoadPreset"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocale(cat, sub, "SaveQuit"), onClick: () => OnButtonClick("SaveQuit"));
            HumanCustomSet set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
            float dropdownWidth = 170f;
            ElementFactory.CreateDropdownSetting(SinglePanel, style, settings.CustomSets.GetSelectedSetIndex(),
                "Custom set", settings.CustomSets.GetSetNames(), elementWidth: dropdownWidth, onDropdownOptionSelect: () => OnCustomSetSelected());
            GameObject group = ElementFactory.CreateHorizontalGroup(SinglePanel, 10f, TextAnchor.UpperLeft);
            foreach (string button in new string[] { "Create", "Delete", "Copy" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnButtonClick(button));
            }
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 10f, TextAnchor.UpperLeft);
            foreach (string button in new string[] { "Rename", "Import", "Export" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnButtonClick(button));
            }
            CreateHorizontalDivider(SinglePanel);
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Sex, UIManager.GetLocale(cat, sub, "Sex"), new string[] { "Male", "Female" }, 
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => OnSexChanged());
            string[] options = GetOptions("Eye", HumanSetup.EyeCount);
            ElementFactory.CreateIconPickSetting(SinglePanel, style, set.Eye, UIManager.GetLocale(cat, sub, "Eye"), options, GetIcons(options), 
                UIManager.CurrentMenu.IconPickPopup, elementWidth: dropdownWidth, elementHeight: 40f, onSelect: () => _menu.ResetCharacter());
            options = GetOptions("Face", HumanSetup.FaceCount, true);
            ElementFactory.CreateIconPickSetting(SinglePanel, style, set.Face, UIManager.GetLocale(cat, sub, "Face"), options, GetIcons(options),
                UIManager.CurrentMenu.IconPickPopup, elementWidth: dropdownWidth, elementHeight: 40f, onSelect: () => _menu.ResetCharacter());
            options = GetOptions("Glass", HumanSetup.GlassCount, true);
            ElementFactory.CreateIconPickSetting(SinglePanel, style, set.Glass, UIManager.GetLocale(cat, sub, "Glass"), options, GetIcons(options),
                UIManager.CurrentMenu.IconPickPopup, elementWidth: dropdownWidth, elementHeight: 40f, onSelect: () => _menu.ResetCharacter());
            options = GetHairOptions();
            ElementFactory.CreateIconPickSetting(SinglePanel, style, set.Hair, UIManager.GetLocale(cat, sub, "Hair"), options, GetIcons(options),
                UIManager.CurrentMenu.IconPickPopup, elementWidth: dropdownWidth, elementHeight: 40f, onSelect: () => _menu.ResetCharacter());
            options = GetCostumeOptions(set);
            ElementFactory.CreateIconPickSetting(SinglePanel, style, set.Costume, UIManager.GetLocale(cat, sub, "Costume"), options, GetIcons(options),
                UIManager.CurrentMenu.IconPickPopup, elementWidth: dropdownWidth, elementHeight: 40f, onSelect: () => _menu.ResetCharacter());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Boots, UIManager.GetLocale(cat, sub, "Boots"), GetOptions("Boots", 2),
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Cape, UIManager.GetLocale(cat, sub, "Cape"), new string[] {"No cape", "Cape"}, 
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Logo, UIManager.GetLocale(cat, sub, "Logo"), GetOptions("Logo", 4),
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter());
            ElementFactory.CreateColorSetting(SinglePanel, style, set.SkinColor, UIManager.GetLocale(cat, sub, "SkinColor"), UIManager.CurrentMenu.ColorPickPopup,
                onChangeColor: () => _menu.ResetCharacter());
            ElementFactory.CreateColorSetting(SinglePanel, style, set.HairColor, UIManager.GetLocale(cat, sub, "HairColor"), UIManager.CurrentMenu.ColorPickPopup,
                onChangeColor: () => _menu.ResetCharacter());
            ElementFactory.CreateColorSetting(SinglePanel, style, set.ShirtColor, UIManager.GetLocale(cat, sub, "ShirtColor"), UIManager.CurrentMenu.ColorPickPopup,
                onChangeColor: () => _menu.ResetCharacter());
            ElementFactory.CreateColorSetting(SinglePanel, style, set.StrapsColor, UIManager.GetLocale(cat, sub, "StrapsColor"), UIManager.CurrentMenu.ColorPickPopup,
                onChangeColor: () => _menu.ResetCharacter());
            ElementFactory.CreateColorSetting(SinglePanel, style, set.PantsColor, UIManager.GetLocale(cat, sub, "PantsColor"), UIManager.CurrentMenu.ColorPickPopup,
                onChangeColor: () => _menu.ResetCharacter());
            ElementFactory.CreateColorSetting(SinglePanel, style, set.JacketColor, UIManager.GetLocale(cat, sub, "JacketColor"), UIManager.CurrentMenu.ColorPickPopup,
                onChangeColor: () => _menu.ResetCharacter());
            ElementFactory.CreateColorSetting(SinglePanel, style, set.BootsColor, UIManager.GetLocale(cat, sub, "BootsColor"), UIManager.CurrentMenu.ColorPickPopup,
                onChangeColor: () => _menu.ResetCharacter());
        }

        private string[] GetOptions(string prefix, int options, bool includeNone = false)
        {
            List<string> names = new List<string>();
            if (includeNone)
                names.Add(prefix + "None");
            for (int i = 0; i < options; i++)
                names.Add(prefix + i.ToString());
            return names.ToArray();
        }

        private string[] GetHairOptions()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < HumanSetup.HairMCount; i++)
                names.Add("HairM" + i.ToString());
            for (int i = 0; i < HumanSetup.HairFCount; i++)
                names.Add("HairF" + i.ToString());
            return names.ToArray();
        }

        private string[] GetCostumeOptions(HumanCustomSet set)
        {
            bool male = set.Sex.Value == 0;
            List<string> names = new List<string>();
            if (male)
            {
                for (int i = 0; i < HumanSetup.CostumeMCount; i++)
                    names.Add("CostumeM" + i.ToString());
            }
            else
            {
                for (int i = 0; i < HumanSetup.CostumeFCount; i++)
                    names.Add("CostumeF" + i.ToString());
            }
            return names.ToArray();
        }

        private string[] GetIcons(string[] options)
        {
            List<string> icons = new List<string>();
            foreach (string option in options)
                icons.Add(ResourcePaths.Characters + "/Human/Previews/" + option);
            return icons.ToArray();
        }

        private void OnSexChanged()
        {
            var settings = SettingsManager.HumanCustomSettings;
            var set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
            if (set.Sex.Value == (int)HumanSex.Male)
                set.Hair.Value = "HairM0";
            else
                set.Hair.Value = "HairF0";
            set.Costume.Value = 0;
            OnCustomSetSelected();
        }

        private void OnCustomSetSelected()
        {
            _menu.RebuildPanels(true);
            _menu.ResetCharacter(true);
        }

        private void OnButtonClick(string name)
        {
            HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
            SetNamePopup setNamePopup = UIManager.CurrentMenu.SetNamePopup;
            switch (name)
            {
                case "Create":
                    setNamePopup.Show("New set", () => OnCostumeSetOperationFinish(name), UIManager.GetLocaleCommon("Create"));
                    break;
                case "Delete":
                    if (settings.CustomSets.CanDeleteSelectedSet())
                        UIManager.CurrentMenu.ConfirmPopup.Show(UIManager.GetLocaleCommon("DeleteWarning"), () => OnCostumeSetOperationFinish(name),
                            UIManager.GetLocaleCommon("Delete"));
                    break;
                case "Rename":
                        string currentSetName = settings.CustomSets.GetSelectedSet().Name.Value;
                        setNamePopup.Show(currentSetName, () => OnCostumeSetOperationFinish(name), UIManager.GetLocaleCommon("Rename"));
                    break;
                case "Copy":
                    setNamePopup.Show("New set", () => OnCostumeSetOperationFinish(name), UIManager.GetLocaleCommon("Copy"));
                    break;
                case "SaveQuit":
                    SettingsManager.HumanCustomSettings.Save();
                    SceneLoader.LoadScene(SceneName.MainMenu);
                    break;
                case "LoadPreset":
                    List<string> sets = new List<string>(SettingsManager.HumanCustomSettings.Costume1Sets.GetSetNames());
                    UIManager.CurrentMenu.SelectListPopup.ShowLoad(sets, "Presets", onLoad: () => OnCostumeSetOperationFinish("LoadPreset"));
                    break;
                case "Import":
                    UIManager.CurrentMenu.ImportPopup.Show(onSave: () => OnCostumeSetOperationFinish(name));
                    break;
                case "Export":
                    var set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
                    var json = set.SerializeToJsonObject();
                    if (json.HasKey("Preset"))
                        json["Preset"] = false;
                    UIManager.CurrentMenu.ExportPopup.Show(json.ToString(aIndent: 4));
                    break;
            }
        }

        private void OnCostumeSetOperationFinish(string name)
        {
            SetNamePopup setNamePopup = UIManager.CurrentMenu.SetNamePopup;
            SetSettingsContainer<HumanCustomSet> settings = SettingsManager.HumanCustomSettings.CustomSets;
            switch (name)
            {
                case "Create":
                    settings.CreateSet(setNamePopup.NameSetting.Value);
                    settings.GetSelectedSetIndex().Value = settings.GetSets().GetCount() - 1;
                    break;
                case "Delete":
                    settings.DeleteSelectedSet();
                    settings.GetSelectedSetIndex().Value = 0;
                    break;
                case "Rename":
                    settings.GetSelectedSet().Name.Value = setNamePopup.NameSetting.Value;
                    break;
                case "Copy":
                    settings.CopySelectedSet(setNamePopup.NameSetting.Value);
                    settings.GetSelectedSetIndex().Value = settings.GetSets().GetCount() - 1;
                    break;
                case "LoadPreset":
                    string setName = settings.GetSelectedSet().Name.Value;
                    foreach (HumanCustomSet findSet in SettingsManager.HumanCustomSettings.Costume1Sets.GetSets().GetItems())
                    {
                        if (findSet.Name.Value == UIManager.CurrentMenu.SelectListPopup.FinishSetting.Value)
                        {
                            settings.GetSelectedSet().Copy(findSet);
                            settings.GetSelectedSet().Preset.Value = false;
                            settings.GetSelectedSet().Name.Value = setName;
                        }
                    }
                    break;
                case "Import":
                    ImportPopup importPopup = UIManager.CurrentMenu.ImportPopup;
                    try
                    {
                        var setName2 = settings.GetSelectedSet().Name.Value;
                        settings.GetSelectedSet().DeserializeFromJsonString(importPopup.ImportSetting.Value);
                        settings.GetSelectedSet().Preset.Value = false;
                        settings.GetSelectedSet().Name.Value = setName2;
                        importPopup.Hide();
                    }
                    catch
                    {
                        importPopup.ShowError("Invalid human preset.");
                    }
                    break;
            }
            OnCustomSetSelected();
        }
    }
}
