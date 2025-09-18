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
    class CharacterEditorTitanCostumePanel: HeadedPanel
    {
        protected override string Title => UIManager.GetLocale("CharacterEditor", "Costume", "Title");
        protected override float Width => 380f;
        protected override float Height => 1020f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 25;
        protected override int VerticalPadding => 25;
        protected override bool ScrollBar => true;
        private CharacterEditorMenu _menu;
        private bool _shouldGeneratePreviewAfterRebuild = false;
        private string _previousProfileId = null;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _menu = (CharacterEditorMenu)UIManager.CurrentMenu;
            var currentSet = (TitanCustomSet)SettingsManager.TitanCustomSettings.TitanCustomSets.GetSelectedSet();
            _previousProfileId = currentSet.UniqueId.Value;
            ElementStyle style = new ElementStyle(titleWidth: 130f, themePanel: ThemePanel);
            var settings = SettingsManager.TitanCustomSettings;
            string cat = "CharacterEditor";
            string sub = "Costume";
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Quit"), onClick: () => OnButtonClick("Quit"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocale(cat, sub, "SaveQuit"), onClick: () => OnButtonClick("SaveQuit"));
            var set = (TitanCustomSet)settings.TitanCustomSets.GetSelectedSet();
            float dropdownWidth = 170f;
            ElementFactory.CreateDropdownSetting(SinglePanel, style, settings.TitanCustomSets.GetSelectedSetIndex(),
                "Custom set", settings.TitanCustomSets.GetSetNames(), elementWidth: dropdownWidth, onDropdownOptionSelect: () => OnCustomSetSelected());
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
            var options = GetOptions("Head", BasicTitanSetup.HeadCount);
            ElementFactory.CreateIconPickSetting(SinglePanel, style, set.Head, UIManager.GetLocale(cat, sub, "Head"), options, GetIcons(options),
                UIManager.CurrentMenu.IconPickPopup, elementWidth: dropdownWidth, elementHeight: 40f, onSelect: () => OnCharacterChanged());
            options = GetOptions("Body", BasicTitanSetup.BodyCount);
            ElementFactory.CreateIconPickSetting(SinglePanel, style, set.Body, UIManager.GetLocale(cat, sub, "Body"), options, GetIcons(options),
                UIManager.CurrentMenu.IconPickPopup, elementWidth: dropdownWidth, elementHeight: 40f, onSelect: () => OnCharacterChanged());
            options = GetOptions("Eye", BasicTitanSetup.EyeCount);
            ElementFactory.CreateIconPickSetting(SinglePanel, style, set.Eye, UIManager.GetLocale(cat, sub, "Eye"), options, GetIcons(options),
                UIManager.CurrentMenu.IconPickPopup, elementWidth: dropdownWidth, elementHeight: 40f, onSelect: () => OnCharacterChanged());
            options = GetHairOptions();
            ElementFactory.CreateIconPickSetting(SinglePanel, style, set.Hair, UIManager.GetLocale(cat, sub, "Hair"), options, GetIcons(options),
                UIManager.CurrentMenu.IconPickPopup, elementWidth: dropdownWidth, elementHeight: 40f, onSelect: () => OnCharacterChanged());
            ElementFactory.CreateColorSetting(SinglePanel, style, set.SkinColor, UIManager.GetLocale(cat, sub, "SkinColor"), UIManager.CurrentMenu.ColorPickPopup,
                onChangeColor: () => OnCharacterChanged());
            ElementFactory.CreateColorSetting(SinglePanel, style, set.HairColor, UIManager.GetLocale(cat, sub, "HairColor"), UIManager.CurrentMenu.ColorPickPopup,
                onChangeColor: () => OnCharacterChanged());
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
            for (int i = 0; i < BasicTitanSetup.HairMCount; i++)
                names.Add("HairM" + i.ToString());
            for (int i = 0; i < BasicTitanSetup.HairFCount; i++)
                names.Add("HairF" + i.ToString());
            return names.ToArray();
        }

        private string[] GetIcons(string[] options)
        {
            List<string> icons = new List<string>();
            foreach (string option in options)
                icons.Add(ResourcePaths.Characters + "/Titans/Previews/" + option);
            return icons.ToArray();
        }

        private void OnCustomSetSelected()
        {
            var currentSet = (TitanCustomSet)SettingsManager.TitanCustomSettings.TitanCustomSets.GetSelectedSet();
            string currentProfileId = currentSet.UniqueId.Value;
            _menu.RebuildPanels(true);
            _menu.ResetCharacter(true);
            if (_previousProfileId != null && _previousProfileId != currentProfileId)
            {
                var gameManager = (CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager;
                if (gameManager != null)
                {
                    gameManager.StartCoroutine(CapturePreviousTitanProfilePreview(_previousProfileId, currentProfileId));
                }
            }
            else if (_shouldGeneratePreviewAfterRebuild)
            {
                _shouldGeneratePreviewAfterRebuild = false;
                Utility.CharacterPreviewGenerator.GeneratePreviewForTitanSet(_menu as CharacterEditorTitanMenu, isRebuild: true);
            }
            _previousProfileId = currentProfileId;
        }

        private System.Collections.IEnumerator CapturePreviousTitanProfilePreview(string previousProfileId, string currentProfileId)
        {
            var settings = SettingsManager.TitanCustomSettings;
            TitanCustomSet previousSet = null;
            int previousSetIndex = -1;
            var customSets = settings.TitanCustomSets.GetSets().GetItems();
            for (int i = 0; i < customSets.Count; i++)
            {
                var set = (TitanCustomSet)customSets[i];
                if (set.UniqueId.Value == previousProfileId)
                {
                    previousSet = set;
                    previousSetIndex = i;
                    break;
                }
            }
            if (previousSet != null)
            {
                int currentSelectedIndex = settings.TitanCustomSets.SelectedSetIndex.Value;
                settings.TitanCustomSets.SelectedSetIndex.Value = previousSetIndex;
                var character = (DummyTitan)((CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager).Character;
                character.Setup.Load(previousSet);
                yield return Util.WaitForFrames(3);
                Utility.CharacterPreviewGenerator.CaptureCurrentCharacterPreview(false);
                settings.TitanCustomSets.SelectedSetIndex.Value = currentSelectedIndex;
                var currentSet = (TitanCustomSet)settings.TitanCustomSets.GetSelectedSet();
                character.Setup.Load(currentSet);
                character.Idle();
            }
        }

        private void OnCharacterChanged()
        {
            _menu.ResetCharacter(fullReset: false);
        }

        private void GeneratePreviewForCurrentSet()
        {
            Utility.CharacterPreviewGenerator.GeneratePreviewForTitanSet(_menu as CharacterEditorTitanMenu, isRebuild: false);
        }

        private void OnButtonClick(string name)
        {
            var settings = SettingsManager.TitanCustomSettings;
            SetNamePopup setNamePopup = UIManager.CurrentMenu.SetNamePopup;
            switch (name)
            {
                case "Create":
                    setNamePopup.Show("New set", () => OnCostumeSetOperationFinish(name), UIManager.GetLocaleCommon("Create"));
                    break;
                case "Delete":
                    if (settings.TitanCustomSets.CanDeleteSelectedSet())
                        UIManager.CurrentMenu.ConfirmPopup.Show(UIManager.GetLocaleCommon("DeleteWarning"), () => OnCostumeSetOperationFinish(name),
                            UIManager.GetLocaleCommon("Delete"));
                    break;
                case "Rename":
                        string currentSetName = settings.TitanCustomSets.GetSelectedSet().Name.Value;
                        setNamePopup.Show(currentSetName, () => OnCostumeSetOperationFinish(name), UIManager.GetLocaleCommon("Rename"));
                    break;
                case "Copy":
                    setNamePopup.Show("New set", () => OnCostumeSetOperationFinish(name), UIManager.GetLocaleCommon("Copy"));
                    break;
                case "SaveQuit":
                    SettingsManager.TitanCustomSettings.Save();
                    var gameManager = (GameManagers.CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager;
                    if (gameManager?.Character is DummyTitan dummyTitan)
                    {
                        var currentSet = (TitanCustomSet)SettingsManager.TitanCustomSettings.TitanCustomSets.GetSelectedSet();
                        dummyTitan.Setup.Load(currentSet);
                    }
                    StartCoroutine(SaveQuitCaptureCoroutineInternal());
                    return;
                case "LoadPreset":
                    List<string> sets = new List<string>(SettingsManager.TitanCustomSettings.TitanCustomSets.GetSetNames());
                    UIManager.CurrentMenu.SelectListPopup.ShowLoad(sets, "Presets", onLoad: () => OnCostumeSetOperationFinish("LoadPreset"));
                    break;
                case "Import":
                    UIManager.CurrentMenu.ImportPopup.Show(onSave: () => OnCostumeSetOperationFinish(name));
                    break;
                case "Export":
                    var set = (TitanCustomSet)settings.TitanCustomSets.GetSelectedSet();
                    var json = set.SerializeToJsonObject();
                    if (json.HasKey("Preset"))
                        json["Preset"] = false;
                    UIManager.CurrentMenu.ExportPopup.Show(json.ToString(aIndent: 4));
                    break;
                case "Quit":
                    UIManager.CurrentMenu.ConfirmPopup.Show(UIManager.GetLocaleCommon("QuitWithoutSaveConfirm"), () =>
                    {
                        Utility.CharacterPreviewGenerator.ClearSessionGeneratedPreviews();
                        SettingsManager.TitanCustomSettings.Load();
                        SceneLoader.LoadScene(SceneName.MainMenu);
                    }, UIManager.GetLocaleCommon("Quit"));
                    break;
            }
        }

        private void OnCostumeSetOperationFinish(string name)
        {
            var setNamePopup = UIManager.CurrentMenu.SetNamePopup;
            var settings = SettingsManager.TitanCustomSettings.TitanCustomSets;
            switch (name)
            {
                case "Create":
                    settings.CreateSet(setNamePopup.NameSetting.Value);
                    settings.GetSelectedSetIndex().Value = settings.GetSets().GetCount() - 1;
                    _shouldGeneratePreviewAfterRebuild = true;
                    break;
                case "Delete":
                    settings.DeleteSelectedSet();
                    settings.GetSelectedSetIndex().Value = 0;
                    Utility.CharacterPreviewGenerator.CleanupOrphanedPreviews();
                    break;
                case "Rename":
                    settings.GetSelectedSet().Name.Value = setNamePopup.NameSetting.Value;
                    break;
                case "Copy":
                    settings.CopySelectedSet(setNamePopup.NameSetting.Value);
                    settings.GetSelectedSetIndex().Value = settings.GetSets().GetCount() - 1;
                    _shouldGeneratePreviewAfterRebuild = true;
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
                        importPopup.ShowError("Invalid titan preset.");
                    }
                    break;
            }
            OnCustomSetSelected();
        }

        private System.Collections.IEnumerator SaveQuitCaptureCoroutineInternal()
        {
            yield return Util.WaitForFrames(3);
            Utility.CharacterPreviewGenerator.CaptureCurrentCharacterPreview(false);
            Utility.CharacterPreviewGenerator.SaveCachedPreviewsToDisk();
            SceneLoader.LoadScene(SceneName.MainMenu);
        }
    }
}
