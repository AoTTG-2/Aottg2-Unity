namespace CustomLogic
{
    [CLType(Name = "TeamEnum", Static = true, Abstract = true, Description = "Enumeration of team types for characters and players.")]
    partial class CustomLogicTeamEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicTeamEnum() { }

        [CLProperty("No team assigned.")]
        public static string None => "None";

        [CLProperty("Blue team.")]
        public static string Blue => "Blue";

        [CLProperty("Red team.")]
        public static string Red => "Red";

        [CLProperty("Titan team.")]
        public static string Titan => "Titan";

        [CLProperty("Human team.")]
        public static string Human => "Human";
    }
}
