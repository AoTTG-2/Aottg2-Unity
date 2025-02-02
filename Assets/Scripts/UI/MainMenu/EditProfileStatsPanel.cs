using System;
using GameProgress;
using UnityEngine;
using Utility;

namespace UI
{
    class EditProfileStatsPanel : CategoryPanel
    {
        protected override bool DoublePanel => true;
        protected override bool DoublePanelDivider => true;
        protected override bool ScrollBar => true;
        protected override float VerticalSpacing => 10f;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            GameStatContainer stat = GameProgressManager.GameProgress.GameStat;
            AchievementCount count = GameProgressManager.GameProgress.Achievement.GetAchievementCount();
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: ThemePanel);

            CreateTitleLabel(DoublePanelLeft, style, "General");
            CreateStatLabel(DoublePanelLeft, style, "Level", stat.Level.Value.ToString());
            CreateStatLabel(DoublePanelLeft, style, "Exp", stat.Exp.Value.ToString() + "/" + GameProgressManager.GetExpToNext().ToString());
            CreateStatLabel(DoublePanelLeft, style, "Playtime", Format.GetReadableTimespan(TimeSpan.FromSeconds(stat.PlayTime.Value)));
            CreateStatLabel(DoublePanelLeft, style, "Highest speed", ((int)stat.HighestSpeed.Value).ToString());
            CreateHorizontalDivider(DoublePanelLeft);
            CreateTitleLabel(DoublePanelLeft, style, "Achievements");
            CreateStatLabel(DoublePanelLeft, style, "Bronze", count.FinishedBronze + "/" + count.TotalBronze);
            CreateStatLabel(DoublePanelLeft, style, "Silver", count.FinishedSilver + "/" + count.TotalSilver);
            CreateStatLabel(DoublePanelLeft, style, "Gold", count.FinishedGold + "/" + count.TotalGold);
            CreateHorizontalDivider(DoublePanelLeft);

            var display = new StatsDisplay(
                (text, fontStyle) => ElementFactory.CreateDefaultLabel(DoublePanelRight, style, text, fontStyle: fontStyle, alignment: TextAnchor.MiddleLeft),
                () => CreateHorizontalDivider(DoublePanelRight));

            display.CreateLabels();
        }

        protected void CreateStatLabel(Transform panel, ElementStyle style, string title, string value) =>
            ElementFactory.CreateDefaultLabel(panel, style, title + ": " + value, alignment: TextAnchor.MiddleLeft);

        protected void CreateTitleLabel(Transform panel, ElementStyle style, string title) =>
            ElementFactory.CreateDefaultLabel(panel, style, title, fontStyle: FontStyle.Bold, alignment: TextAnchor.MiddleLeft);
    }
}
