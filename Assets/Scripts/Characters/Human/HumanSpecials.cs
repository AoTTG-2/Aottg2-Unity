using GameManagers;
using System.Collections.Generic;

namespace Characters
{
    class HumanSpecials
    {
        public static string[] AnySpecials = new string[] {"Potato", "Escape", "Dance", "Distract", "Smell", "Supply", "SmokeBomb", "Carry", "Switchback"};
        public static string[] AHSSSpecials = new string[] { "AHSSTwinShot" };
        public static string[] BladeSpecials = new string[] { "DownStrike", "Spin1", "Spin2", "Spin3", "BladeThrow" };
        public static string[] ShifterSpecials = new string[] { "Eren", "Annie" };

        public static readonly string DefaultSpecial = "Potato";

        public static HashSet<string> GetSpecialNames(string loadout, bool includeShifters)
        {
            HashSet<string> names = new HashSet<string>();
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
            _ => null
        };

        public static string GetSpecialIcon(string special) =>
            special.Replace(" ", "") + "SpecialIcon";
    }
}
