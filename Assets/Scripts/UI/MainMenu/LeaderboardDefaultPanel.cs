using ApplicationManagers;
using Settings;
using SimpleJSONFixed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class LeaderboardDefaultPanel: CategoryPanel
    {
        protected override bool ScrollBar => true;
        protected override int VerticalPadding => 15;
        protected override float VerticalSpacing => 20f;
        protected override string ThemePanel => "LeaderboardPopup";

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "MainMenu";
            string sub = "LeaderboardPopup";
            ElementStyle style = new ElementStyle(themePanel: ThemePanel);
            if (PastebinLoader.Status != PastebinStatus.Loaded)
            {
                ElementFactory.CreateDefaultLabel(SinglePanel, style, "Loading leaderboard...");
                return;
            }
            try
            {
                // get categories
                StringSetting currentCategory = ((LeaderboardPopup)parent).CurrentCategory;
                StringSetting currentSubcategory = ((LeaderboardPopup)parent).CurrentSubcategory;
                if (currentCategory.Value == string.Empty)
                    currentCategory.Value = PastebinLoader.Leaderboard[0]["CategoryName"];
                JSONNode category = FindCategory(currentCategory.Value);
                if (currentSubcategory.Value == string.Empty)
                    currentSubcategory.Value = category["Subcategories"][0]["SubcategoryName"];
                JSONNode subcategory = FindSubcategory(category, currentSubcategory.Value);

                // create header
                Transform header = ElementFactory.CreateHorizontalGroup(SinglePanel, 10f, TextAnchor.MiddleLeft).transform;
                ElementStyle dropdownStyle = new ElementStyle(titleWidth: 0f, themePanel: ThemePanel);
                ElementFactory.CreateDropdownSetting(header, dropdownStyle, currentCategory, "", GetCategoryNames(), elementWidth: 180f, optionsWidth: 180f,
                    onDropdownOptionSelect: () => parent.RebuildCategoryPanel());
                string[] subcategories = GetSubcategoryNames(category);
                if (subcategories.Length > 1)
                    ElementFactory.CreateDropdownSetting(header, dropdownStyle, currentSubcategory, "", subcategories, elementWidth: 180f, optionsWidth: 180f,
                        onDropdownOptionSelect: () => parent.RebuildCategoryPanel());
                ElementFactory.CreateTooltipIcon(header, style, "Join the discord to participate in the leaderboard.");

                // get players
                bool descending = category.HasKey("Sort") ? category["Sort"].Value == "Descending" : true;
                bool hasLink = category.HasKey("HasLink") ? category["HasLink"].AsBool : false;
                int decimalPlaces = category.HasKey("DecimalPlaces") ? category["DecimalPlaces"].AsInt : 0;
                string scoreLabel = category.HasKey("ScoreLabel") ? category["ScoreLabel"].Value : "Score";
                descending = subcategory.HasKey("Sort") ? subcategory["Sort"].Value == "Descending" : descending;
                hasLink = subcategory.HasKey("HasLink") ? subcategory["HasLink"].AsBool : hasLink;
                decimalPlaces = subcategory.HasKey("DecimalPlaces") ? subcategory["DecimalPlaces"].AsInt : decimalPlaces;
                scoreLabel = subcategory.HasKey("ScoreLabel") ? category["ScoreLabel"].Value : scoreLabel;
                List<JSONNode> players = new List<JSONNode>();
                foreach (JSONNode player in subcategory["Players"])
                    players.Add(player);
                List<JSONNode> sortedPlayers;
                if (descending)
                    sortedPlayers = players.OrderByDescending(x => x["Score"].AsFloat).ToList();
                else
                    sortedPlayers = players.OrderBy(x => x["Score"].AsFloat).ToList();
                CreateHorizontalDivider(SinglePanel);

                // create subheader
                Transform subHeader = ElementFactory.CreateHorizontalGroup(SinglePanel, 0f).transform;
                ElementFactory.CreateDefaultLabel(subHeader, style, "Rank", FontStyle.Bold);
                ElementFactory.CreateDefaultLabel(subHeader, style, "Name", FontStyle.Bold);
                ElementFactory.CreateDefaultLabel(subHeader, style, scoreLabel, FontStyle.Bold);
                if (hasLink)
                    ElementFactory.CreateDefaultLabel(subHeader, style, "Watch", FontStyle.Bold);
                float width = hasLink ? GetWidth() / 4f : GetWidth() / 3f;
                foreach (Transform t in subHeader)
                    t.GetComponent<LayoutElement>().preferredWidth = width;

                // create player rows
                for (int i = 0; i < sortedPlayers.Count; i++)
                {
                    Transform row = ElementFactory.CreateHorizontalGroup(SinglePanel, 0f).transform;
                    Transform rank = ElementFactory.CreateHorizontalGroup(row, 5f, TextAnchor.MiddleCenter).transform;
                    CreateRank(rank, style, i + 1);
                    ElementFactory.CreateDefaultLabel(row, style, sortedPlayers[i]["Name"].Value);
                    ElementFactory.CreateDefaultLabel(row, style, Util.FormatFloat(sortedPlayers[i]["Score"].AsFloat, decimalPlaces));
                    if (hasLink)
                    {
                        if (sortedPlayers[i].HasKey("Link"))
                        {
                            string link = sortedPlayers[i]["Link"].Value;
                            ElementFactory.CreateLinkButton(row, style, "Link",
                                onClick: () => UIManager.CurrentMenu.ExternalLinkPopup.Show(link));
                        }
                        else
                            ElementFactory.CreateDefaultLabel(row, style, "");
                    }
                    foreach (Transform t in row)
                        t.GetComponent<LayoutElement>().preferredWidth = width;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                ElementFactory.CreateDefaultLabel(SinglePanel, style, "Failed to load leaderboard.");
                return;
            }
        }

        private void CreateRank(Transform parent, ElementStyle style, int rank)
        {
            if (rank <= 3)
            {
                RawImage img = ElementFactory.CreateRawImage(parent, style, "Icons/Quests/TrophyIcon", 32f, 32f).GetComponent<RawImage>();
                img.color = UIManager.GetThemeColor(ThemePanel, "Rank", "Trophy" + rank.ToString() + "Color");
            }
            else if (rank <= 10)
            {
                RawImage img = ElementFactory.CreateRawImage(parent, style, "Icons/Quests/Badge1Icon", 32f, 32f).GetComponent<RawImage>();
                img.color = UIManager.GetThemeColor(ThemePanel, "Rank", "BadgeColor");
            }
            ElementFactory.CreateDefaultLabel(parent, style, rank.ToString());
        }


        private void OnButtonClick(string name)
        {
        }

        private string[] GetCategoryNames()
        {
            List<string> names = new List<string>();
            foreach (JSONNode category in PastebinLoader.Leaderboard)
                names.Add(category["CategoryName"]);
            return names.ToArray();
        }

        private string[] GetSubcategoryNames(JSONNode category)
        {
            List<string> names = new List<string>();
            foreach (JSONNode subcategory in category["Subcategories"])
                names.Add(subcategory["SubcategoryName"]);
            return names.ToArray();
        }

        private JSONNode FindCategory(string name)
        {
            foreach (JSONNode category in PastebinLoader.Leaderboard)
            {
                if (category["CategoryName"] == name)
                    return category;
            }
            return null;
        }

        private JSONNode FindSubcategory(JSONNode category, string subcategoryName)
        {
            if (category["Subcategories"].Count == 1)
                return category["Subcategories"][0];
            else
            {
                foreach (JSONNode subcategory in category["Subcategories"])
                {
                    if (subcategory.HasKey("SubcategoryName") && subcategory["SubcategoryName"] == subcategoryName)
                        return subcategory;
                }
            }
            return null;
        }
    }
}
