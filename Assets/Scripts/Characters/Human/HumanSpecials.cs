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
            else if (special == "Smell")
                return new SmellSpecial(owner);
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
            else if (special == "Carry")
                return new CarrySpecial(owner);
            else if (special == "AHSSTwinShot")
                return new AHSSTwinShot(owner);
            else if (special == "Eren")
                return new ShifterTransformSpecial(owner, "Eren");
            else if (special == "Annie")
                return new ShifterTransformSpecial(owner, "Annie");
            else if (special == "Armored")
                return new ShifterTransformSpecial(owner, "Armored");
            else if (special == "Switchback")
                return new SwitchbackSpecial(owner);
            return null;
        }

        public static readonly IReadOnlyDictionary<string, string> TooltipBySpecialName = new Dictionary<string, string>
        {
            ["Distract"] = "Causes enemies to switch their focus to you",
            ["Escape"] = "Lets you escape the grasp of a titan  —  just once",
            ["Dance"] = "Make titans around you laugh, holding them in place",
            ["Smell"] = "Sniff out the location of titans in range",
            ["Potato"] = "Eat a potato to give you a burst of running speed",
            ["DownStrike"] = "Stop in mid-air and slash down for the kill",
            ["Spin1"] = "Spin horizontally with your blades",
            ["Spin2"] = "Spin vertically with your blades",
            ["Spin3"] = "Spin1, but it shoots out a reeling hook",
            ["BladeThrow"] = "Throw your blades, turning them into a projectile",
            ["Stock"] = "Slash as if attacking with blades - does no damage",
            ["None"] = "Used for Custom Logic-made skills",
            ["Supply"] = "Drop a supply tank on the ground",
            ["SmokeBomb"] = "Shoot a flare that blinds titans. Aim for the eyes",
            ["Carry"] = "Pick up another player. They can attack while carried.",
            ["AHSSTwinShot"] = "Shoot both guns, increasing damage and range",
            ["Eren"] = "Transform into the Attack Titan",
            ["Annie"] = "Transform into the Female Titan",
            ["Armored"] = "Transform into the Armored Titan",
            ["Switchback"] = "Use near surfaces to bounce, keeping your momentum",
        };

        public static string GetSpecialIcon(string special)
        {
            string icon = special.Replace(" ", "") + "SpecialIcon";
            return icon;
        }
    }
}
