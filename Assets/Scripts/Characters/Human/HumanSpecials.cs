using GameManagers;
using System.Collections.Generic;

namespace Characters
{
    class HumanSpecials
    {
        public static string[] AnySpecials = new string[] {"Potato", "Escape", "Dance", "Distract"};
        public static string[] BladeSpecials = new string[] { "DownStrike", "Spin1", "Spin2", "BladeThrow" };
        public static string[] ShifterSpecials = new string[] { "ErenTransform", "AnnieTransform", "ArmoredTransform" };

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
            if (loadout == HumanLoadout.Thunderspears)
            {
                names.Insert(0, "Stock");
            }
            if (includeShifters)
            {
                foreach (string special in ShifterSpecials)
                    names.Add(special);
            }
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
            else if (special == "BladeThrow")
                return new BladeThrowSpecial(owner);
            else if (special == "Stock")
                return new StockSpecial(owner);
            else if (special == "ErenTransform")
                return new ShifterTransformSpecial(owner, "Eren");
            else if (special == "AnnieTransform")
                return new ShifterTransformSpecial(owner, "Annie");
            else if (special == "ArmoredTransform")
                return new ShifterTransformSpecial(owner, "Armored");
            return null;
        }

        public static string GetSpecialIcon(string loadout, string special)
        {
            if (special == "ArmoredTransform")
                special = "ErenTransform";
            if (!GetSpecialNames(loadout, true).Contains(special))
                return "";
            string icon = special.Replace(" ", "") + "SpecialIcon";
            return icon;
        }
    }
}
