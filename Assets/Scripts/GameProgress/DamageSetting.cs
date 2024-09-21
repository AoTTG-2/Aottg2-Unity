using Characters;
using Settings;
using SimpleJSONFixed;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameProgress
{
    class DamageSetting : BaseSetting
    {
        private const string OverallKey = "Overall";
        private const string HighestKey = "Highest";
        private const string TotalKey = "Total";

        private static readonly Comparison<KeyValuePair<string, JSONNode>> Comparison;

        private JSONNode root;

        static DamageSetting()
        {
            var weaponOrder = (KillWeapon[])Enum.GetValues(typeof(KillWeapon));
            var stringOrder = weaponOrder.Select(w => w.ToString())
                .Concat(HumanSpecials.AnySpecials)
                .Concat(HumanSpecials.AHSSSpecials)
                .Concat(HumanSpecials.BladeSpecials)
                .Concat(HumanSpecials.ShifterSpecials)
                .ToArray();
            var i = 0;
            var orderByString = stringOrder.ToDictionary(k => k, v => i++);
            Comparison = (lhs, rhs) =>
            {
                var hasRhs = orderByString.TryGetValue(rhs.Key, out var rhsi);
                var hasLhs = orderByString.TryGetValue(lhs.Key, out var lhsi);
                if (hasLhs && hasRhs) return rhsi - lhsi;
                else if (hasRhs && !hasLhs) return 1;
                else if (!hasRhs && hasLhs) return -1;
                else return string.Compare(lhs.Key, rhs.Key);
            };
        }


        private (ulong highest, ulong total) this[KillMethod method]
        {
            get
            {
                if (!root.HasKey(method.WeaponKey)) return (0ul, 0ul);
                if (!root[method.WeaponKey].HasKey(method.SpecialKey)) return (0ul, 0ul);
                var byType = root[method.WeaponKey][method.SpecialKey];
                var highest = byType.HasKey(HighestKey) ? byType[HighestKey].AsULong : 0ul;
                var total = byType.HasKey(TotalKey) ? byType[TotalKey].AsULong : 0ul;
                return (highest, total);
            }

            set
            {
                if (!root.HasKey(method.WeaponKey)) root.Add(method.WeaponKey, new JSONObject());
                if (!root[method.WeaponKey].HasKey(method.SpecialKey)) root[method.WeaponKey].Add(method.SpecialKey, new JSONObject());
                var byType = root[method.WeaponKey][method.SpecialKey];
                byType[HighestKey] = value.highest;
                byType[TotalKey] = value.total;
            }
        }

        private (ulong highest, ulong total) Overall
        {
            get
            {
                if (!root.HasKey(OverallKey)) return (0ul, 0ul);
                var byType = root[OverallKey];
                var highest = byType.HasKey(HighestKey) ? byType[HighestKey].AsULong : 0ul;
                var total = byType.HasKey(TotalKey) ? byType[TotalKey].AsULong : 0ul;
                return (highest, total);
            }

            set
            {
                if (!root.HasKey(OverallKey)) root.Add(OverallKey, new JSONObject());
                var byType = root[OverallKey];
                byType[HighestKey].AsULong = value.highest;
                byType[TotalKey].AsULong = value.total;
            }
        }

        public DamageSetting() => SetDefault();

        public override void SetDefault() => root = new JSONObject();

        public override JSONNode SerializeToJsonObject() => root;

        public override void DeserializeFromJsonObject(JSONNode json) => this.root = json;

        public void Register(KillMethod method, ulong damage)
        {
            {
                var (highest, total) = Overall;
                if (damage > highest) highest = damage;
                total += damage;
                Overall = (highest, total);
            }

            {
                var (highest, total) = this[method];
                if (damage > highest) highest = damage;
                total += damage;
                this[method] = (highest, total);
            }
        }

        public IEnumerable<(string title, string value)> GetStatLabels()
        {
            var rootPairs = root.Linq.Where(kvp => kvp.Key != OverallKey).ToList();
            rootPairs.Sort(Comparison);

            yield return ($"Highest Overall", Overall.highest.ToString());
            foreach (var okvp in rootPairs)
            {
                var weapon = okvp.Key;
                foreach (var ikvp in okvp.Value)
                {
                    var special = ikvp.Key;
                    var highest = ikvp.Value[HighestKey];
                    yield return special == KillMethod.NullSpecialKey ? ($"Highest {weapon}", highest) : ($"Highest {weapon} ({special})", highest);
                }
            }

            yield return ($"Total Overall", Overall.total.ToString());
            foreach (var okvp in rootPairs)
            {
                var weapon = okvp.Key;
                foreach (var ikvp in okvp.Value)
                {
                    var special = ikvp.Key;
                    var total = ikvp.Value[TotalKey];
                    yield return special == KillMethod.NullSpecialKey ? ($"Total {weapon}", total) : ($"Total {weapon} ({special})", total);
                }
            }
        }
    }
}
