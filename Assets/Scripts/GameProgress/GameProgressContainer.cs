using System;
using UnityEngine;
using Settings;
using Utility;

namespace GameProgress
{
    class GameProgressContainer: SaveableSettingsContainer
    {
        protected override string FolderPath { get { return FolderPaths.GameProgress; } }
        protected override string FileName { get { return "GameProgress"; } }
        protected override bool Encrypted => true;

        public AchievementContainer Achievement = new AchievementContainer();
        public QuestContainer Quest = new QuestContainer();
        public GameStatContainer GameStat = new GameStatContainer();

        public override void Save()
        {
            Quest.CollectRewards();
            base.Save();
        }
    }
}
