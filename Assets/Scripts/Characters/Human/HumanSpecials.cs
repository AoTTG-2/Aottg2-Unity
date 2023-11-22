using GameManagers;
using System.Collections.Generic;

namespace Characters
{
    class HumanSpecials
    {
        public static string[] AnySpecials = new string[] {"Potato", "Escape", "Dance", "Distract", "Supply", "SmokeBomb"};
        public static string[] AHSSSpecials = new string[] { "AHSSTwinShot" };
        public static string[] BladeSpecials = new string[] { "DownStrike", "Spin1", "Spin2", "Spin3", "BladeThrow" };
        public static string[] ShifterSpecials = new string[] { "Eren", "Annie", "Armored" };

        public static List<string> GetSpecialNames(string loadout, bool includeShifters)
        {
            List<string> names = new List<string>();
            foreach (string special in AnySpecials)
                names.Add(special);
            if (loadout == HumanLoadout.Blades)
            {
                foreach (string special in BladeSpecials)
                    names.Add(special);
            }
            else if (loadout == HumanLoadout.AHSS)
            {
                foreach (string special in AHSSSpecials)
                    names.Add(special);
            }
            if (includeShifters)
            {
                foreach (string special in ShifterSpecials)
                    names.Add(special);
            }
            if (loadout == HumanLoadout.Thunderspears)
                names.Add("Stock");
            names.Add("None");
            return names;
        }

        public static BaseUseable GetSpecialUseable(BaseCharacter owner, string special)
        {
            if (special == "Distract")
                return new DistractSpecial(owner);
            else if (special == "Escape")
                return new EscapeSpecial(owner);
            else if (special == "Dance")
                return new DanceSpecial(owner);
            else if (special == "Potato")
                return new PotatoSpecial(owner);
            else if (special == "DownStrike")
                return new DownStrikeSpecial(owner);
            else if (special == "Spin1")
                return new Spin1Special(owner);
            else if (special == "Spin2")
                return new Spin2Special(owner);
            else if (special == "Spin3")
                return new Spin3Special(owner);
            else if (special == "BladeThrow")
                return new BladeThrowSpecial(owner);
            else if (special == "Stock")
                return new StockSpecial(owner);
            else if (special == "None")
                return new NoneSpecial(owner);
            else if (special == "Supply")
                return new SupplySpecial(owner);
            else if (special == "SmokeBomb")
                return new SmokeBombSpecial(owner);
            else if (special == "AHSSTwinShot")
                return new AHSSTwinShot(owner);
            else if (special == "Eren")
                return new ShifterTransformSpecial(owner, "Eren");
            else if (special == "Annie")
                return new ShifterTransformSpecial(owner, "Annie");
            else if (special == "Armored")
                return new ShifterTransformSpecial(owner, "Armored");
            return null;
        }

        public static string GetSpecialIcon(string loadout, string special)
        {
            if (!GetSpecialNames(loadout, true).Contains(special))
                return "";
            string icon = special.Replace(" ", "") + "SpecialIcon";
            return icon;
        }
    }
}
