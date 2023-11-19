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

namespace UI
{
    class CharacterEditorCostumePanel: HeadedPanel
    {
        protected override string Title => UIManager.GetLocale("CharacterEditor", "Costume", "Title");
        protected override float Width => 380f;
        protected override float Height => 950f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 25;
        protected override int VerticalPadding => 25;
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
            foreach (string button in new string[] { "Create", "Delete" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnButtonClick(button));
            }
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 10f, TextAnchor.UpperLeft);
            foreach (string button in new string[] { "Rename", "Copy" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnButtonClick(button));
            }
            CreateHorizontalDivider(SinglePanel);
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Sex, UIManager.GetLocale(cat, sub, "Sex"), new string[] { "Male", "Female" }, 
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => OnSexChanged());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Eye, UIManager.GetLocale(cat, sub, "Eye"), GetOptionNames("Eye", 28), 
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Face, UIManager.GetLocale(cat, sub, "Face"), GetOptionNames("Face", 13, true), 
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Glass, UIManager.GetLocale(cat, sub, "Glass"), GetOptionNames("Glass", 9, true), 
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Hair, UIManager.GetLocale(cat, sub, "Hair"), GetHairOptionNames(11, 11), 
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Skin, UIManager.GetLocale(cat, sub, "Skin"), GetOptionNames("Skin", 2), 
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter());
            int costumes = set.Sex.Value == (int)HumanSex.Male ? 15 : 12;
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Costume, UIManager.GetLocale(cat, sub, "Costume"), GetOptionNames("Costume", costumes), 
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Cape, UIManager.GetLocale(cat, sub, "Cape"), new string[] {"No cape", "Cape"}, 
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, set.Logo, UIManager.GetLocale(cat, sub, "Logo"), GetOptionNames("Logo", 4),
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter());
            ElementFactory.CreateColorSetting(SinglePanel, style, set.HairColor, UIManager.GetLocale(cat, sub, "HairColor"), UIManager.CurrentMenu.ColorPickPopup,
                onChangeColor: () => _menu.ResetCharacter());
        }

        private string[] GetOptionNames(string prefix, int options, bool includeNone = false)
        {
            List<string> names = new List<string>();
            if (includeNone)
                names.Add(prefix + "None");
            for (int i = 0; i < options; i++)
                names.Add(prefix + i.ToString());
            return names.ToArray();
        }

        private string[] GetHairOptionNames(int maleOptions, int femaleOptions)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < maleOptions; i++)
                names.Add("HairM" + i.ToString());
            for (int i = 0; i < femaleOptions; i++)
                names.Add("HairF" + i.ToString());
            return names.ToArray();
        }

        private void OnSexChanged()
        {
            var settings = SettingsManager.HumanCustomSettings;
            var set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
            if (set.Sex.Value == (int)HumanSex.Male)
                set.Hair.Value = "HairM0";
            else
                set.Hair.Value = "HairF0";
            OnCustomSetSelected();
        }

        private void OnCustomSetSelected()
        {
            _menu.RebuildPanels();
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
            }
            OnCustomSetSelected();
        }
    }
}
