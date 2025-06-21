﻿namespace Characters
{
    class TitanSounds
    {
        public static string Footstep1 = "Footstep1";
        public static string Footstep2 = "Footstep2";
        public static string Footstep3 = "Footstep3";
        public static string Swing1 = "Swing1";
        public static string Swing2 = "Swing2";
        public static string Swing3 = "Swing3";
        public static string Hurt1 = "Hurt1";
        public static string Hurt2 = "Hurt2";
        public static string Hurt3 = "Hurt3";
        public static string Hurt4 = "Hurt4";
        public static string Hurt5 = "Hurt5";
        public static string Hurt6 = "Hurt6";
        public static string Hurt7 = "Hurt7";
        public static string Hurt8 = "Hurt8";
        public static string Grunt1 = "Grunt1";
        public static string Grunt2 = "Grunt2";
        public static string Grunt3 = "Grunt3";
        public static string Grunt4 = "Grunt4";
        public static string Grunt5 = "Grunt5";
        public static string Grunt6 = "Grunt6";
        public static string Grunt7 = "Grunt7";
        public static string Grunt8 = "Grunt8";
        public static string Grunt9 = "Grunt9";
        public static string Grunt10 = "Grunt10";
        public static string Hit = "Hit";
        public static string Roar1 = "Roar1";
        public static string Roar2 = "Roar2";
        public static string DeathFall = "DeathFall";
        public static string DeathNoFall = "DeathNoFall";
        public static string Bite1 = "Bite1";
        public static string Bite2 = "Bite2";
        public static string Laugh1 = "Laugh1";
        public static string Laugh2 = "Laugh2";
        public static string Huff1 = "Huff1";
        public static string Huff2 = "Huff2";
        public static string Huff3 = "Huff3";
        public static string Huff4 = "Huff4";
        public static string Huff5 = "Huff5";
        public static string Huff6 = "Huff6";
        public static string TitanJump = "TitanJump";
        public static string RockPickup = "RockPickup";
        public static string RockThrow1 = "RockThrow1";
        public static string RockThrow2 = "RockThrow2";


        private static string[] Footsteps = new string[] { Footstep1, Footstep2, Footstep3 };

        public static string GetRandom(params string[] sounds)
        {
            return sounds[UnityEngine.Random.Range(0, sounds.Length)];
        }

        public static string GetRandomFromList(string[] sounds)
        {
            return sounds[UnityEngine.Random.Range(0, sounds.Length)];
        }

        public static string GetRandomFootstep()
        {
            return GetRandomFromList(Footsteps);
        }

        public static string GetRandomHurt()
        {
            return GetRandom(Hurt1, Hurt2, Hurt3, Hurt4, Hurt5, Hurt6, Hurt7, Hurt8);
        }

        public static string GetRandomGrabGrunt()
        {
            return GetRandom(Grunt1, Grunt2, Grunt3, Grunt4, Grunt5, Grunt6, Grunt7, Grunt8, Grunt9, Grunt10);
        }

        public static string GetRandomLaugh()
        {
            return GetRandom(Laugh1, Laugh2);
        }

        public static string GetRandomBite()
        {
            return GetRandom(Bite1, Bite2);
        }
    }
}
