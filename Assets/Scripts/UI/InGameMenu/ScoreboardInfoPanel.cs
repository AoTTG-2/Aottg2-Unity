using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using ApplicationManagers;
using GameManagers;
using System.Collections;
using Utility;
using System.Collections.Specialized;
using Photon.Pun;
using static UnityEngine.Rendering.DebugUI;
using CustomLogic;

namespace UI
{
    class ScoreboardInfoPanel: ScoreboardCategoryPanel
    {
        protected override bool DoublePanel => true;
        protected override float VerticalSpacing => 15f;
        protected override int VerticalPadding => 15;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(themePanel: ThemePanel);
            InGameSet settings = SettingsManager.InGameCurrent;
            ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, "General", fontStyle: FontStyle.Bold, alignment: TextAnchor.MiddleLeft);
            CreateLabels(style, DoublePanelLeft, "General", settings.General);
            ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, "Map Hash: " + MapManager.MapScript.MapHash.ToLower(), alignment: TextAnchor.MiddleLeft);
            ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, "Logic Hash: " + CustomLogicManager.LogicHash.ToLower(), alignment: TextAnchor.MiddleLeft);
            if (!PhotonNetwork.OfflineMode)
            {
                ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, "Room name: " + PhotonNetwork.CurrentRoom.GetStringProperty(RoomProperty.Name), alignment: TextAnchor.MiddleLeft);
                ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, "Max players: " + PhotonNetwork.CurrentRoom.MaxPlayers.ToString(), alignment: TextAnchor.MiddleLeft);
            }
            CreateHorizontalDivider(DoublePanelLeft);
            ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, "Titans", fontStyle: FontStyle.Bold, alignment: TextAnchor.MiddleLeft);
            CreateLabels(style, DoublePanelLeft, "Titans", settings.Titan);
            ElementFactory.CreateDefaultLabel(DoublePanelRight, style, "Mode", fontStyle: FontStyle.Bold, alignment: TextAnchor.MiddleLeft);
            CreateLabels(style, DoublePanelRight, "Mode", settings.Mode.Current);
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateDefaultLabel(DoublePanelRight, style, "Misc", fontStyle: FontStyle.Bold, alignment: TextAnchor.MiddleLeft);
            CreateLabels(style, DoublePanelRight, "Misc", settings.Misc);
        }

        private void CreateLabels(ElementStyle style, Transform panel, string category, OrderedDictionary settings)
        {
            if (category == "Mode")
                settings.Insert(0, "Game mode", SettingsManager.InGameCurrent.General.GameMode);
            foreach (DictionaryEntry entry in settings)
            {
                BaseSetting setting = (BaseSetting)entry.Value;
                string name = (string)entry.Key;
                string value = setting.ToString();
                if (setting.GetType() == typeof(FloatSetting))
                    value = Util.FormatFloat(((FloatSetting)setting).Value, 2);
                if (category == "General")
                {
                    if (name == "Difficulty")
                        value = ((GameDifficulty)((IntSetting)setting).Value).ToString();
                    else if (name == "Password" || name == "PrevGameMode" || name == "RoomName" || name == "MaxPlayers")
                        continue;
                }
                else if (category == "Misc")
                {
                    if (name == "PVP")
                        value = ((PVPMode)((IntSetting)setting).Value).ToString();
                    else if (name == "ClearKDROnRestart")
                        name = "Clear KDR on restart";
                    else if (name == "ThunderspearPVP")
                        name = "Thunderspear PVP";
                    else if (name == "APGPVP")
                        name = "APG PVP";
                    else if (name == "AllowAHSS")
                        name = "Allow ahss";
                    else if (name == "AllowAPG")
                        name = "Allow apg";
                }
                string label = Util.PascalToSentence(name) + ": " + value;
                ElementFactory.CreateDefaultLabel(panel, style, label, alignment: TextAnchor.MiddleLeft);
            }
        }

        private void CreateLabels(ElementStyle style, Transform panel, string category, BaseSettingsContainer container)
        {
            CreateLabels(style, panel, category, container.Settings);
        }

        private void CreateLabels(ElementStyle style, Transform panel, string category, Dictionary<string, BaseSetting> settings)
        {
            OrderedDictionary dictionary = new OrderedDictionary();
            foreach (string key in settings.Keys)
                dictionary.Add(key, settings[key]);
            CreateLabels(style, panel, category, dictionary);
        }
    }
}
