using Settings;
using System.Collections.Generic;
using SimpleJSONFixed;
using System;
using UnityEngine;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using GameProgress;

namespace Characters
{
    class HumanStats
    {
        // stats
        public int Speed = 80;
        public int Gas = 80;
        public int Ammunition = 80;
        public int Acceleration = 80;
        public Dictionary<string, BasePerk> Perks = new Dictionary<string, BasePerk>();
        public static int MaxPerkPoints = 3;

        // in-game
        public float CurrentGas = -1f;
        public float MaxGas = -1f;
        public float GasUsage = 0.2f;
        public float HorseSpeed = 50f;
        public float RunSpeed;
        protected Human _human;

        public HumanStats(Human human)
        {
            _human = human;
            Perks.Add("AdvancedAlloy", new AdvancedAlloyPerk());
            Perks.Add("DurableBlades", new DurableBladesPerk());
            Perks.Add("RefillTime", new RefillTimePerk());
            Perks.Add("VerticalDash", new VerticalDashPerk());
            Perks.Add("OmniDash", new OmniDashPerk());
            ResetGas();
            UpdateStats();
        }

        public void DisablePerks()
        {
            foreach (var perk in Perks.Values)
            {
                perk.CurrPoints = 0;
            }
        }

        public int GetPerkPoints()
        {
            int points = 0;
            foreach (string key in Perks.Keys)
            {
                points += Perks[key].CurrPoints;
            }
            return points;
        }

        public void UpdateStats()
        {
            if (_human != null)
                _human.Cache.Rigidbody.mass = 0.5f - ((float)Acceleration * 2f - 150f) * 0.001f;
            RunSpeed = (0.4f * (float)Speed) - 16f;
        }

        public void ResetGas()
        {
            CurrentGas = MaxGas = ((float)Gas * 2f) - 35f;
        }

        public void UseDashGas()
        {
            UseGas(4f);
        }

        public void UseFrameGas()
        {
            UseGas(GasUsage * Time.deltaTime);
        }

        public void UseHookGas()
        {
            UseGas(GasUsage);
        }

        public void UseTSGas()
        {
            UseGas(100f * CharacterData.HumanWeaponInfo["Thunderspear"]["StunGasPenalty"].AsFloat);
        }

        private void UseGas(float amount)
        {
            CurrentGas -= amount;
            CurrentGas = Mathf.Max(CurrentGas, 0f);
        }

        public static HumanStats Deserialize(HumanStats stats, string str)
        {
            try
            {
                if (str != string.Empty)
                {
                    var json = JSON.Parse(str);
                    stats.Speed = json["Speed"].AsInt;
                    stats.Gas = json["Gas"].AsInt;
                    stats.Ammunition = json["Ammunition"].AsInt;
                    stats.Acceleration = json["Acceleration"].AsInt;
                    foreach (string key in json["Perks"].Keys)
                    {
                        if (stats.Perks.ContainsKey(key))
                            stats.Perks[key].CurrPoints = json["Perks"][key].AsInt;
                    }
                    if (stats.Validate())
                    {
                        stats.ResetGas();
                        stats.UpdateStats();
                        return stats;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("Exception while loading human stats: " + e.Message);
            }
            return new HumanStats(stats._human);
        }

        public string Serialize()
        {
            if (!Validate())
            {
                Speed = 80;
                Gas = 80;
                Ammunition = 80;
                Acceleration = 80;
                Perks.Clear();
            }
            var json = new JSONObject();
            json["Speed"] = Speed.ToString();
            json["Gas"] = Gas.ToString();
            json["Ammunition"] = Ammunition.ToString();
            json["Acceleration"] = Acceleration.ToString();
            var perks = new JSONObject();
            foreach (string key in Perks.Keys)
            {
                if (Perks[key].CurrPoints > 0)
                    perks[key] = Perks[key].CurrPoints.ToString();
            }
            json["Perks"] = perks;
            return json.ToString();
        }

        public bool Validate()
        {
            if (Speed + Gas + Ammunition + Acceleration > 320) return false;
            if (Speed < 50 || Speed > 100) return false;
            if (Gas < 50 || Gas > 100) return false;
            if (Ammunition < 50 || Ammunition > 100) return false;
            if (Acceleration < 50 || Acceleration > 100) return false;
            foreach (var perk in Perks.Values)
            {
                if (!perk.Validate(Perks))
                    return false;
            }
            if (GetPerkPoints() > MaxPerkPoints)
                return false;
            return true;
        }
    }
}
