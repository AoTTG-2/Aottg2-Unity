﻿using UI;
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
        public static string Blades = "Blades";
        public static string AHSS = "AHSS";
        public static string Thunderspears = "Thunderspears";
        public static string APG = "APG";
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
        public static string PasswordHash = "PH";
    }
}
