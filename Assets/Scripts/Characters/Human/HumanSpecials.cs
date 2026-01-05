using CustomLogic;
using GameManagers;
using System.Collections.Generic;
using System.Linq;

namespace Characters
{
    class HumanSpecials
    {
        public static string[] AnySpecials = new string[] {HumanSpecial.Potato, HumanSpecial.Escape, HumanSpecial.Dance, HumanSpecial.Distract, HumanSpecial.Smell, HumanSpecial.Supply, HumanSpecial.SmokeBomb, HumanSpecial.Carry, HumanSpecial.Switchback, HumanSpecial.Confuse};
        public static string[] AHSSSpecials = new string[] { HumanSpecial.AHSSTwinShot };
        public static string[] BladeSpecials = new string[] { HumanSpecial.DownStrike, HumanSpecial.Spin1, HumanSpecial.Spin2, HumanSpecial.Spin3, HumanSpecial.BladeThrow };
        public static string[] ShifterSpecials = new string[] { HumanSpecial.Eren, HumanSpecial.Annie };
        public static readonly string DefaultSpecial = HumanSpecial.Potato;

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
            if (loadout == HumanLoadout.Thunderspear)
                AddSpecialName(names, HumanSpecial.Stock);
            AddSpecialName(names, HumanSpecial.None);
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
            HumanSpecial.Distract => new DistractSpecial(owner),
            HumanSpecial.Escape => new EscapeSpecial(owner),
            HumanSpecial.Dance => new DanceSpecial(owner),
            HumanSpecial.Smell => new SmellSpecial(owner),
            HumanSpecial.Potato => new PotatoSpecial(owner),
            HumanSpecial.DownStrike => new DownStrikeSpecial(owner),
            HumanSpecial.Spin1 => new Spin1Special(owner),
            HumanSpecial.Spin2 => new Spin2Special(owner),
            HumanSpecial.Spin3 => new Spin3Special(owner),
            HumanSpecial.BladeThrow => new BladeThrowSpecial(owner),
            HumanSpecial.Stock => new StockSpecial(owner),
            HumanSpecial.None => new NoneSpecial(owner),
            HumanSpecial.Supply => new SupplySpecial(owner),
            HumanSpecial.SmokeBomb => new SmokeBombSpecial(owner),
            HumanSpecial.Carry => new CarrySpecial(owner),
            HumanSpecial.AHSSTwinShot => new AHSSTwinShot(owner),
            HumanSpecial.Eren => new ShifterTransformSpecial(owner, ShifterType.Eren),
            HumanSpecial.Annie => new ShifterTransformSpecial(owner, ShifterType.Annie),
            HumanSpecial.Armored => new ShifterTransformSpecial(owner, ShifterType.Armored),
            HumanSpecial.Switchback => new SwitchbackSpecial(owner),
            HumanSpecial.Confuse => new ConfuseSpecial(owner),
            _ => null
        };

        public static string GetSpecialIcon(string special) =>
            special.Replace(" ", "") + "SpecialIcon";
    }
}
