using Settings;
using SimpleJSONFixed;
using System.Collections.Generic;

namespace GameProgress
{
    class DamageSetting : BaseSetting
    {
        private const string OverallKey = "Overall";
        private const string HighestKey = "Highest";
        private const string TotalKey = "Total";

        private JSONNode root;

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
            foreach (var okvp in root)
            {
                var weapon = okvp.Key;
                if (weapon == OverallKey)
                {
                    var highest = okvp.Value[HighestKey];
                    yield return ($"Highest Overall", highest);
                }
                else
                {
                    foreach (var ikvp in okvp.Value)
                    {
                        var special = ikvp.Key;
                        var highest = ikvp.Value[HighestKey];
                        yield return special == KillMethod.NullSpecialKey ? ($"Highest {weapon}", highest) : ($"Total {weapon} ({special})", highest);
                    }
                }
            }

            foreach (var okvp in root)
            {
                var weapon = okvp.Key;
                if (weapon == OverallKey)
                {
                    var total = okvp.Value[TotalKey];
                    yield return ($"Total Overall", total);
                }
                else
                {
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
}
