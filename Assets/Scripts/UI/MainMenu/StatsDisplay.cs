using GameProgress;
using GameProgress.Codegen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UI
{
    class StatsDisplay
    {
        private readonly LabelFactory createLabel;
        private readonly DividerFactory createDivider;

        public StatsDisplay(LabelFactory createLabel, DividerFactory createDivider)
        {
            this.createLabel = createLabel;
            this.createDivider = createDivider;
        }

        public void CreateLabels() => DisplayDamage();

        private void DisplayDamage()
        {
            var maps = StatsManager.Maps;
            createLabel("Damage", FontStyle.Bold);

            var overall = (highest: 0u, total: 0ul, kills: 0u);

            // Couldn't these two dicts be merged...? That's assuming no weapon has the same name as a special.
            var byWeapon = new Dictionary<string, (uint highest, ulong total, uint kills)>();
            var bySpecial = new Dictionary<string, Dictionary<string, (uint highest, ulong total, uint kills)>>();

            foreach (var modes in GetValues<MapsT, ModesT>(maps))
                foreach (var enemies in GetValues<ModesT, EnemiesT>(modes))
                    foreach (var weapons in GetValues<EnemiesT, WeaponsT>(enemies))
                        foreach (var (weaponName, specials) in GetValuesWithName<WeaponsT, SpecialsT>(weapons))
                            foreach (var (specialName, metrics) in GetValuesWithName<SpecialsT, MetricsT>(specials))
                            {
                                overall.highest = Math.Max(overall.highest, metrics.HighestDamage);
                                overall.total += metrics.TotalDamage;
                                overall.kills += metrics.Kills;

                                UpdateMap(byWeapon, weaponName, metrics);
                                if (specialName != nameof(specials.None))
                                    UpdateMap(bySpecial, weaponName, specialName, metrics);
                            }

            createLabel($"Highest Overall Damage: {overall.highest}", FontStyle.Normal);
            createLabel($"Total Overall Damage: {overall.total}", FontStyle.Normal);
            createLabel($"Overall Kills: {overall.kills}", FontStyle.Normal);

            foreach (var weaponPairs in byWeapon)
            {
                var weaponName = weaponPairs.Key;
                var weaponMetrics = weaponPairs.Value;

                createDivider();
                createLabel($"Highest {weaponName} Damage: {weaponMetrics.highest}", FontStyle.Normal);
                createLabel($"Total {weaponName} Damage: {weaponMetrics.total}", FontStyle.Normal);
                createLabel($"{weaponName} Kills: {weaponMetrics.kills}", FontStyle.Normal);
                foreach (var specialPairs in bySpecial[weaponName])
                {
                    var specialName = specialPairs.Key;
                    var specialMetrics = specialPairs.Value;

                    createLabel($"Highest {specialName} Damage: {specialMetrics.highest}", FontStyle.Italic);
                    createLabel($"Total {specialName} Damage: {specialMetrics.total}", FontStyle.Italic);
                    createLabel($"{specialName} Kills: {specialMetrics.kills}", FontStyle.Italic);
                }
            }

            createDivider();
        }

        private static void UpdateMap(Dictionary<string, (uint highest, ulong total, uint kills)> map, string key, MetricsT metrics)
        {
            if (map.TryGetValue(key, out var existing))
                map[key] =
                    (Math.Max(metrics.HighestDamage, existing.highest),
                    metrics.TotalDamage + existing.total,
                    metrics.Kills + existing.kills);
            else
                map.Add(key, (metrics.HighestDamage, metrics.TotalDamage, metrics.Kills));
        }

        private static void UpdateMap(Dictionary<string, Dictionary<string, (uint highest, ulong total, uint kills)>> map, string outerKey, string innerKey, MetricsT metrics)
        {
            if (!map.TryGetValue(outerKey, out var inner))
                map.Add(outerKey, inner = new Dictionary<string, (uint highest, ulong total, uint kills)>());

            UpdateMap(inner, innerKey, metrics);
        }

        private IEnumerable<FieldInfo> GetFields<T, TField>(T parent) =>
            typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(f => f.FieldType == typeof(TField));

        private IEnumerable<TField> GetValues<T, TField>(T parent) =>
            GetFields<T, TField>(parent)
                .Select(f => (TField)f.GetValue(parent))
                .Where(v => v != null);

        private IEnumerable<(string name, TField value)> GetValuesWithName<T, TField>(T parent) =>
            GetFields<T, TField>(parent)
                   .Select(f => (name: GetBackingFieldName(f), value: (TField)f.GetValue(parent)))
                   .Where(n => n.value != null);

        private static string GetBackingFieldName(FieldInfo f)
        {
            var end = f.Name.Length - 16;
            return f.Name[1..end];
        }

        public delegate void LabelFactory(string text, FontStyle style);
        public delegate void DividerFactory();
    }
}
