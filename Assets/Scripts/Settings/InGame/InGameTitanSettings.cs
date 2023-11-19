namespace Settings
{
    class InGameTitanSettings : BaseSettingsContainer
    {
        public BoolSetting TitanSpawnEnabled = new BoolSetting(false);
        public FloatSetting TitanSpawnNormal = new FloatSetting(20f, minValue: 0f, maxValue: 100f);
        public FloatSetting TitanSpawnAbnormal = new FloatSetting(20f, minValue: 0f, maxValue: 100f);
        public FloatSetting TitanSpawnJumper = new FloatSetting(20f, minValue: 0f, maxValue: 100f);
        public FloatSetting TitanSpawnCrawler = new FloatSetting(20f, minValue: 0f, maxValue: 100f);
        public FloatSetting TitanSpawnPunk = new FloatSetting(20f, minValue: 0f, maxValue: 100f);
        public BoolSetting TitanSizeEnabled = new BoolSetting(false);
        public FloatSetting TitanSizeMin = new FloatSetting(1f, minValue: 0.1f, maxValue: 100f);
        public FloatSetting TitanSizeMax = new FloatSetting(3f, minValue: 0.1f, maxValue: 100f);
        public IntSetting TitanHealthMode = new IntSetting(0, minValue: 0);
        public IntSetting TitanHealthMin = new IntSetting(100, minValue: 0);
        public IntSetting TitanHealthMax = new IntSetting(200, minValue: 0);
        public BoolSetting TitanArmorEnabled = new BoolSetting(false);
        public BoolSetting TitanArmorCrawlerEnabled = new BoolSetting(false);
        public IntSetting TitanArmor = new IntSetting(1000, minValue: 0);
        public BoolSetting TitanStandardModels = new BoolSetting(false);
    }
}
