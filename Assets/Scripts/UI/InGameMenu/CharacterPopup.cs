using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Settings;
using ApplicationManagers;
using GameManagers;
using CustomLogic;
using Photon.Pun;

namespace UI
{
    class CharacterPopup : BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1000f;
        protected override float Height => 470f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "";
        public string LocaleCategory = "CharacterPopup";
        protected List<string> _allowedCategories = new List<string>();

        public override void Setup(BasePanel parent = null)
        {
            SetAllowedCategories();
            if (!_allowedCategories.Contains(UIManager.GetLastcategory(GetType())))
                UIManager.SetLastCategory(GetType(), _allowedCategories[0]);
            base.Setup(parent);
            SetupBottomButtons();
        }

        protected virtual void SetAllowedCategories()
        {
            InGameMiscSettings settings = SettingsManager.InGameCurrent.Misc;
            if (settings.AllowAHSS.Value || settings.AllowBlades.Value || settings.AllowThunderspears.Value || settings.AllowAPG.Value)
                _allowedCategories.Add("Human");
            if (settings.AllowPlayerTitans.Value)
                _allowedCategories.Add("Titan");
            if (settings.AllowShifters.Value)
                _allowedCategories.Add("Shifter");
            if (_allowedCategories.Count == 0)
                _allowedCategories.Add("Human");
        }

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);
            
            foreach (string buttonName in _allowedCategories)
            {
                GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, UIManager.GetLocaleCommon(buttonName),
                    onClick: () => SetCategoryPanel(buttonName));
                _topButtons.Add(buttonName, obj.GetComponent<Button>());
            }
            base.SetupTopButtons();
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("Human", typeof(CharacterHumanPanel));
            _categoryPanelTypes.Add("Titan", typeof(CharacterTitanPanel));
            _categoryPanelTypes.Add("Shifter", typeof(CharacterShifterPanel));
        }

        protected virtual void SetupBottomButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocale(LocaleCategory, "Bottom", "SpectateButton"),
                    onClick: () => OnBottomBarButtonClick("Spectate"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Join"),
                    onClick: () => OnBottomBarButtonClick("Join"));
        }

        private void OnBottomBarButtonClick(string name)
        {
            var manager = (InGameManager)SceneLoader.CurrentGameManager;
            switch (name)
            {
                case "Spectate":
                    SettingsManager.InGameCharacterSettings.ChooseStatus.Value = (int)ChooseCharacterStatus.Spectating;
                    InGameManager.UpdatePlayerName();
                    InGameManager.UpdateRoundPlayerProperties();
                    Hide();
                    break;
                case "Join":
                    SettingsManager.InGameCharacterSettings.ChooseStatus.Value = (int)ChooseCharacterStatus.Chosen;
                    bool canJoin = PhotonNetwork.IsMasterClient || CustomLogicManager.Evaluator.CurrentTime < SettingsManager.InGameCurrent.Misc.AllowSpawnTime.Value;
                    if (canJoin && !manager.HasSpawned)
                        manager.SpawnPlayer(false);
                    InGameManager.UpdateRoundPlayerProperties();
                    Hide();
                    break;
            }
        }
    }
}
