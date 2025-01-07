﻿using CustomLogic;
using GameManagers;
using System.Collections.Generic;
using System.Linq;

namespace Characters
{
    class HumanSpecials
    {
        public static string[] AnySpecials = new string[] {"Potato", "Escape", "Dance", "Distract", "Smell", "Supply", "SmokeBomb", "Carry", "Switchback", "Confuse", "Thunderspears"};
        public static string[] AHSSSpecials = new string[] { "AHSSTwinShot" };
        public static string[] BladeSpecials = new string[] { "DownStrike", "Spin1", "Spin2", "Spin3", "BladeThrow" };
        public static string[] ShifterSpecials = new string[] { "Eren", "Annie" };
        public static readonly string DefaultSpecial = "Potato";

        public static List<string> GetSpecialNames(string loadout, bool includeShifters)
        {
            var names = new List<string>();
            foreach (string special in AnySpecials)
                AddSpecialName(names, special);
            if (loadout == HumanLoadout.Blades)
            {
                foreach (string special in BladeSpecials)
                    AddSpecialName(names, special);
            }
            else if (loadout == HumanLoadout.AHSS)
            {
                foreach (string special in AHSSSpecials)
                    AddSpecialName(names, special);
            }
            if (includeShifters)
            {
                foreach (string special in ShifterSpecials)
                    AddSpecialName(names, special);
            }
            if (loadout == HumanLoadout.Thunderspears)
                AddSpecialName(names, "Stock");
            AddSpecialName(names, "None");
            return names;
        }

        private static void AddSpecialName(List<string> specials, string special)
        {
            specials.Add(special);
            return;

            /*
            var allowed = CustomLogicManager.Evaluator.AllowedSpecials;
            var disallowed = CustomLogicManager.Evaluator.DisallowedSpecials;
            if (allowed.Count == 0 && disallowed.Count == 0)
            {
                specials.Add(special);
            }
            else if (allowed.Count > 0 && disallowed.Count == 0)
            {
                if (allowed.Contains(special))
                    specials.Add(special);
            }
            else if (allowed.Count == 0 && disallowed.Count > 0)
            {
                if (!disallowed.Contains(special))
                    specials.Add(special);
            }
            else if (allowed.Count > 0 && disallowed.Count > 0)
            {
                if (allowed.Contains(special) && !disallowed.Contains(special))
                    specials.Add(special);
            }
            */
        }
        public static BaseUseable GetSpecialUseable(BaseCharacter owner, string special) => special switch
        {
            "Distract" => new DistractSpecial(owner),
            "Escape" => new EscapeSpecial(owner),
            "Dance" => new DanceSpecial(owner),
            "Smell" => new SmellSpecial(owner),
            "Potato" => new PotatoSpecial(owner),
            "DownStrike" => new DownStrikeSpecial(owner),
            "Spin1" => new Spin1Special(owner),
            "Spin2" => new Spin2Special(owner),
            "Spin3" => new Spin3Special(owner),
            "BladeThrow" => new BladeThrowSpecial(owner),
            "Stock" => new StockSpecial(owner),
            "None" => new NoneSpecial(owner),
            "Supply" => new SupplySpecial(owner),
            "SmokeBomb" => new SmokeBombSpecial(owner),
            "Carry" => new CarrySpecial(owner),
            "AHSSTwinShot" => new AHSSTwinShot(owner),
            "Eren" => new ShifterTransformSpecial(owner, "Eren"),
            "Annie" => new ShifterTransformSpecial(owner, "Annie"),
            "Armored" => new ShifterTransformSpecial(owner, "Armored"),
            "Switchback" => new SwitchbackSpecial(owner),
            "Confuse" => new ConfuseSpecial(owner),
            "Thunderspears" => new ThunderspearWeapon(owner, 2, 2, CharacterData.HumanWeaponInfo["Thunderspear"]["CD"].AsFloat, 1.5f, CharacterData.HumanWeaponInfo["Thunderspear"]["Speed"].AsFloat, (CharacterData.HumanWeaponInfo["Thunderspear"]["Range"].AsFloat / CharacterData.HumanWeaponInfo["Thunderspear"]["Speed"].AsFloat), CharacterData.HumanWeaponInfo["Thunderspear"]["Delay"].AsFloat, CharacterData.HumanWeaponInfo["Thunderspear"]),
            _ => null
        };

        public static string GetSpecialIcon(string special) =>
            special.Replace(" ", "") + "SpecialIcon";
    }
}
