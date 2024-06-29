namespace Characters
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
        public static string Hit = "Hit";
        public static string Roar = "Roar";
        public static string Fall = "Fall";
        public static string Bite1 = "Bite1";
        public static string Bite2 = "Bite2";
        public static string Laugh1 = "Laugh1";
        public static string Laugh2 = "Laugh2";

        public static string GetRandom(params string[] sounds)
        {
            return sounds[UnityEngine.Random.Range(0, sounds.Length)];
        }

        public static string GetRandomFootstep()
        {
            return GetRandom(Footstep1, Footstep2, Footstep3);
        }

        public static string GetRandomHurt()
        {
            return GetRandom(Hurt1, Hurt2, Hurt3, Hurt4, Hurt5, Hurt6, Hurt7, Hurt8);
        }

        public static string GetRandomDie()
        {
            return GetRandom(Hurt1, Hurt2, Hurt3, Hurt4, Hurt5);
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
