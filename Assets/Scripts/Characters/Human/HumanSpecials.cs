using CustomLogic;
using GameManagers;
using System.Collections.Generic;
using System.Linq;

namespace Characters
{
    class HumanSpecials
    {
        public static string[] AnySpecials = new string[] {"Potato", "Escape", "Dance", "Distract", "Smell", "Supply", "SmokeBomb", "Carry", "Switchback", "Confuse"};
        public static string[] AHSSSpecials = new string[] { "AHSSTwinShot" };
        public static string[] BladeSpecials = new string[] { "DownStrike", "Spin1", "Spin2", "Spin3", "BladeThrow" };
        public static string[] AmmoWeaponSpecials = new string[] { "SpinKennySpecial" };
        public static string[] ShifterSpecials = new string[] { "Eren", "Annie" };
        public static readonly string DefaultSpecial = "Potato";

        public static List<string> GetSpecialNames(string loadout, bool includeShifters)
        {
            var names = new List<string>();
            foreach (string special in AnySpecials)
                AddSpecialName(names, special);
            if (loadout == HumanLoadout.Blade)
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
            {
                AddSpecialName(names, "Stock");
            }
            if(loadout == HumanLoadout.Thunderspears || loadout == HumanLoadout.APG || loadout == HumanLoadout.AHSS)
                foreach (string special in AmmoWeaponSpecials)
                    AddSpecialName(names, special);
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
            "SpinKennySpecial" => new SpinKennySpecial(owner),
            _ => null
        };

        public static string GetSpecialIcon(string special) =>
            special.Replace(" ", "") + "SpecialIcon";
    }
}
