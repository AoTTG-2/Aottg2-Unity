using UI;
using Settings;
using Characters;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace GameManagers
{
    class PlayerProperty
    {
        public static string Name = "Name";
        public static string Guild = "Guild";
        public static string Status = "Status";
        public static string CharacterViewId = "CharacterViewId";
        public static string Character = "Character";
        public static string CustomMapHash = "CustomMapHash";
        public static string CustomLogicHash = "CustomLogicHash";
        public static string Team = "Team";
        public static string Loadout = "Loadout";
        public static string Kills = "Kills";
        public static string Deaths = "Deaths";
        public static string HighestDamage = "HighestDamage";
        public static string TotalDamage = "TotalDamage";
        public static string SpawnPoint = "SpawnPoint";
        public static string CameraDistance = "CameraDistance";
        public static string Ping = "Ping";
        public static string SpectateID = "SpectateID";
    }

    class PlayerStatus
    {
        public static string Alive = "Alive";
        public static string Dead = "Dead";
        public static string Spectating = "Spectating";
    }

    class PlayerCharacter
    {
        public static string Human = "Human";
        public static string Titan = "Titan";
        public static string Shifter = "Shifter";
    }

    class HumanLoadout
    {
        public static string Blade = "Blade";
        public static string AHSS = "AHSS";
        public static string Thunderspear = "Thunderspear";
        public static string APG = "APG";
    }

    class ShifterLoadout
    {
        public const string Annie = "Annie";
        public const string Eren = "Eren";
        public const string Armored = "Armored";
        public const string WallColossal = "WallColossal";
    }

    class TitanLoadout
    {
        public const string Small = "Small";
        public const string Medium = "Medium";
        public const string Large = "Large";
    }

    class TitanType
    {
        public const string Normal = "Normal";
        public const string Abnormal = "Abnormal";
        public const string Jumper = "Jumper";
        public const string Crawler = "Crawler";
        public const string Thrower = "Thrower";
        public const string Punk = "Punk";
        public const string Default = "Default";
        public const string Random = "Random";
    }

    class ShifterType
    {
        public const string Titan = "Titan";
        public const string Annie = "Annie";
        public const string Eren = "Eren";
        public const string Armored = "Armored";
        public const string WallColossal = "WallColossal";
    }

    class HumanSpecial
    {
        public const string Potato = "Potato";
        public const string Escape = "Escape";
        public const string Dance = "Dance";
        public const string Distract = "Distract";
        public const string Smell = "Smell";
        public const string Supply = "Supply";
        public const string SmokeBomb = "SmokeBomb";
        public const string Carry = "Carry";
        public const string Switchback = "Switchback";
        public const string Confuse = "Confuse";

        public const string DownStrike = "DownStrike";
        public const string Spin1 = "Spin1";
        public const string Spin2 = "Spin2";
        public const string Spin3 = "Spin3";
        public const string BladeThrow = "BladeThrow";

        public const string AHSSTwinShot = "AHSSTwinShot";

        public const string Stock = "Stock";

        public const string None = "None";

        public const string Eren = "Eren";
        public const string Annie = "Annie";

        public const string Armored = "Armored";
    }

    class TeamInfo
    {
        public static string Blue = "Blue";
        public static string Red = "Red";
        public static string None = "None";
        public static string Titan = "Titan";
        public static string Human = "Human";

        public static bool SameTeam(string a, string b)
        {
            return a == b && a != None && b != None;
        }

        public static bool SameTeam(BaseCharacter a, BaseCharacter b)
        {
            return SameTeam(a.Team, b.Team) || a == b;
        }

        public static bool SameTeam(BaseCharacter a, string b)
        {
            return a.Team == b && a.Team != None && b != None;
        }

        public static Color GetTeamColorUnity(string Team)
        {
            string color = GetTeamColor(Team);
            ColorUtility.TryParseHtmlString(color, out Color c);
            return c;
        }

        public static string GetTeamColor(string team)
        {
            if (team == Blue)
                return "#3399FF";
            else if (team == Red)
                return "#9A3334";
            else if (team == Human)
                return "#ACD1E9";
            else if (team == Titan)
                return "#FFD800";
            else
                return "white";
        }
    }

    class RoomProperty
    {
        public static string Name = "Name";
        public static string Map = "Map";
        public static string GameMode = "GameMode";
        public static string Password = "Password";
        public static string PasswordSalt = "PS";
        public static string PasswordHash = "PH";
    }
}
