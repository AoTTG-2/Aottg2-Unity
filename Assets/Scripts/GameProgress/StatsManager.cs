using Utility;
using System.IO;
using GameProgress.Codegen;
using UnityEngine;
using Settings;
using Characters;

namespace GameProgress
{
    static class StatsManager
    {
        private static readonly string FolderPath;
        private static readonly string FilePath;

        public static readonly MapsT Maps;

        static StatsManager()
        {
            FolderPath = Path.Join(FolderPaths.Documents, "GameProgress");
            FilePath = Path.Join(FolderPath, "Stats");

            if (File.Exists(FilePath))
                Maps = Load();
            else
                Maps = new MapsT();
        }

        private static MapsT Load()
        {
            var bytes = File.ReadAllBytes(FilePath);
            bytes = new SimpleAES().Decrypt(bytes);
            return MapsT.DeserializeFromBinary(bytes);
        }

        [RuntimeInitializeOnLoadMethod]
        static void RunOnStart() => Application.quitting += Quit;

        static void Quit() => Save();

        private static void Save()
        {
            byte[] bytes = Maps.SerializeToBinary();
            bytes = new SimpleAES().Encrypt(bytes);
            Directory.CreateDirectory(FolderPath);
            File.WriteAllBytes(FilePath, bytes);
        }

        public static MetricsT GetOrCreateMetrics(BaseCharacter enemy, KillMethod method)
        {
            var settings = SettingsManager.InGameCurrent.General;
            var mapName = settings.MapName.Value;
            var modes = mapName switch
            {
                "Forest1" => Maps.Forest1 ??= new ModesT(),
                "Forest2" => Maps.Forest2 ??= new ModesT(),
                "Forest3" => Maps.Forest3 ??= new ModesT(),
                "City1" => Maps.City1 ??= new ModesT(),
                "City2" => Maps.City2 ??= new ModesT(),
                "Utgard1" => Maps.Utgard1 ??= new ModesT(),
                "Utgard2" => Maps.Utgard2 ??= new ModesT(),
                "Cave1" => Maps.Cave1 ??= new ModesT(),
                "Lake" => Maps.Lake ??= new ModesT(),
                "Plaza" => Maps.Plaza ??= new ModesT(),
                "Grandmaster" => Maps.Grandmaster ??= new ModesT(),
                "Casino" => Maps.Casino ??= new ModesT(),
                "ForestFlooded" => Maps.ForestFlooded ??= new ModesT(),
                "ForestLava" => Maps.ForestLava ??= new ModesT(),
                "Deforest1" => Maps.Deforest1 ??= new ModesT(),
                "Birth of Levi" => Maps.BirthOfLevi ??= new ModesT(),
                "Mew's 1K" => Maps.Mews1k ??= new ModesT(),
                "Rocky Mountain" => Maps.RockyMountain ??= new ModesT(),
                "Shiganshina" => Maps.Shiganshina ??= new ModesT(),
                "Trost" => Maps.Trost ??= new ModesT(),
                "Futuristic Forest" => Maps.FuturisticForest ??= new ModesT(),
                _ => Maps.Other.Find(m => m.Name == mapName)?.Modes ?? AddNamedMap(mapName),
            };

            var modeName = settings.GameMode.Value;
            var enemies = modeName switch
            {
                "AHSS PVP" => modes.AhssPvp ??= new EnemiesT(),
                "APG PVP" => modes.ApgPvp ??= new EnemiesT(),
                "Base" => modes.Base ??= new EnemiesT(),
                "Blade PVP" => modes.BladePvp ??= new EnemiesT(),
                "Cage Fight" => modes.CageFight ??= new EnemiesT(),
                "Cranked" => modes.Cranked ??= new EnemiesT(),
                "Endless" => modes.Endless ??= new EnemiesT(),
                "None" => modes.None ??= new EnemiesT(),
                "Racing" => modes.Racing ??= new EnemiesT(),
                "Survive" => modes.Survive ??= new EnemiesT(),
                "Thunderspear PVP" => modes.ThunderspearPvp ??= new EnemiesT(),
                "Titan Explode" => modes.TitanExplode ??= new EnemiesT(),
                "Waves" => modes.Waves ??= new EnemiesT(),
                _ => modes.Other.Find(m => m.Name == modeName)?.Enemies ?? AddNamedMode(modes, modeName),
            };

            var weapons = enemy switch
            {
                BasicTitan => enemies.Titan ??= new WeaponsT(),
                Human => enemies.Human ??= new WeaponsT(),
                BaseShifter => enemies.Shifter ??= new WeaponsT(),
                _ => enemies.Other ??= new WeaponsT(),
            };

            var specials = method.Weapon switch
            {
                KillWeapon.Blade => weapons.Blade ??= new SpecialsT(),
                KillWeapon.AHSS => weapons.Ahss ??= new SpecialsT(),
                KillWeapon.Thunderspear => weapons.Thunderspear ??= new SpecialsT(),
                KillWeapon.APG => weapons.Apg ??= new SpecialsT(),
                KillWeapon.Shifter => weapons.Shifter ??= new SpecialsT(),
                KillWeapon.Titan => weapons.Titan ??= new SpecialsT(),
                _ => weapons.Other ??= new SpecialsT(),
            };

            var metrics = method.Special switch
            {
                "" => specials.None ??= new MetricsT(),
                "Spin1" => specials.Spin1 ??= new MetricsT(),
                "Spin2" => specials.Spin2 ??= new MetricsT(),
                "Spin3" => specials.Spin3 ??= new MetricsT(),
                "DownStrike" => specials.Downstrike ??= new MetricsT(),
                "BladeThrow" => specials.Bladethrow ??= new MetricsT(),
                "AHSSTwinShot" => specials.Twinshot ??= new MetricsT(),
                "Eren" => specials.Eren ??= new MetricsT(),
                "Annie" => specials.Annie ??= new MetricsT(),
                _ => specials.Other ??= new MetricsT()
            };

            return metrics;
        }

        private static ModesT AddNamedMap(string mapName)
        {
            var modes = new ModesT();
            Maps.Other.Add(new NamedMapT() { Name = mapName, Modes = modes });
            return modes;
        }

        private static EnemiesT AddNamedMode(ModesT modes, string modeName)
        {
            var enemies = new EnemiesT();
            modes.Other.Add(new NamedModeT() { Name = modeName, Enemies = enemies });
            return enemies;
        }
    }
}
