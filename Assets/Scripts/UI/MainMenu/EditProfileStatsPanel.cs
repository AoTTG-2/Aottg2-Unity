using Settings;
using System;
using UnityEngine.UI;
using GameProgress;
using UnityEngine;

namespace UI
{
    class EditProfileStatsPanel: CategoryPanel
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
            CreateStatLabel(DoublePanelLeft, style, "Hours played", (stat.PlayTime.Value / 3600f).ToString("0.00"));
            CreateStatLabel(DoublePanelLeft, style, "Highest speed", ((int)stat.HighestSpeed.Value).ToString());
            CreateHorizontalDivider(DoublePanelLeft);
            CreateTitleLabel(DoublePanelLeft, style, "Achievements");
            CreateStatLabel(DoublePanelLeft, style, "Bronze", count.FinishedBronze + "/" + count.TotalBronze);
            CreateStatLabel(DoublePanelLeft, style, "Silver", count.FinishedSilver + "/" + count.TotalSilver);
            CreateStatLabel(DoublePanelLeft, style, "Gold", count.FinishedGold + "/" + count.TotalGold);
            CreateHorizontalDivider(DoublePanelLeft);
            CreateTitleLabel(DoublePanelLeft, style, "Damage");
            CreateStatLabel(DoublePanelLeft, style, "Highest overall", stat.DamageHighestOverall.Value.ToString());
            CreateStatLabel(DoublePanelLeft, style, "Highest blade", stat.DamageHighestBlade.Value.ToString());
            CreateStatLabel(DoublePanelLeft, style, "Highest ahss", stat.DamageHighestAHSS.Value.ToString());
            CreateStatLabel(DoublePanelLeft, style, "Highest thunderspear", stat.DamageHighestThunderspear.Value.ToString());
            CreateStatLabel(DoublePanelLeft, style, "Highest apg", stat.DamageHighestAPG.Value.ToString());
            CreateStatLabel(DoublePanelLeft, style, "Total overall", stat.DamageTotalOverall.Value.ToString());
            CreateStatLabel(DoublePanelLeft, style, "Total blade", stat.DamageTotalBlade.Value.ToString());
            CreateStatLabel(DoublePanelLeft, style, "Total ahss", stat.DamageTotalAHSS.Value.ToString());
            CreateStatLabel(DoublePanelLeft, style, "Total thunderspear", stat.DamageTotalThunderspear.Value.ToString());
            CreateStatLabel(DoublePanelLeft, style, "Total apg", stat.DamageTotalAPG.Value.ToString());
            CreateTitleLabel(DoublePanelRight, style, "Titans Killed");
            CreateStatLabel(DoublePanelRight, style, "Total", stat.TitansKilledTotal.Value.ToString());
            CreateStatLabel(DoublePanelRight, style, "Blade", stat.TitansKilledBlade.Value.ToString());
            CreateStatLabel(DoublePanelRight, style, "AHSS", stat.TitansKilledAHSS.Value.ToString());
            CreateStatLabel(DoublePanelRight, style, "APG", stat.TitansKilledAPG.Value.ToString());
            CreateStatLabel(DoublePanelRight, style, "Thunder spear", stat.TitansKilledThunderspear.Value.ToString());
            CreateStatLabel(DoublePanelRight, style, "Other", stat.TitansKilledOther.Value.ToString());
            CreateHorizontalDivider(DoublePanelRight);
            CreateTitleLabel(DoublePanelRight, style, "Humans Killed");
            CreateStatLabel(DoublePanelRight, style, "Total", stat.HumansKilledTotal.Value.ToString());
            CreateStatLabel(DoublePanelRight, style, "Blade", stat.HumansKilledBlade.Value.ToString());
            CreateStatLabel(DoublePanelRight, style, "AHSS", stat.HumansKilledAHSS.Value.ToString());
            CreateStatLabel(DoublePanelRight, style, "APG", stat.HumansKilledAPG.Value.ToString());
            CreateStatLabel(DoublePanelRight, style, "Thunder spear", stat.HumansKilledThunderspear.Value.ToString());
            CreateStatLabel(DoublePanelRight, style, "Titan", stat.HumansKilledTitan.Value.ToString());
            CreateStatLabel(DoublePanelRight, style, "Other", stat.TitansKilledOther.Value.ToString());
        }

        protected void CreateStatLabel(Transform panel, ElementStyle style, string title, string value)
        {
            ElementFactory.CreateDefaultLabel(panel, style, title + ": " + value, alignment: TextAnchor.MiddleLeft).GetComponent<Text>();
        }

        protected void CreateTitleLabel(Transform panel, ElementStyle style, string title)
        {
            ElementFactory.CreateDefaultLabel(panel, style, title, fontStyle: FontStyle.Bold, alignment: TextAnchor.MiddleLeft).GetComponent<Text>();
        }
    }
}
