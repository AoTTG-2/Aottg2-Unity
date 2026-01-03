namespace CustomLogic
{
    [CLType(Name = "PlayerStatusEnum", Static = true, Abstract = true, Description = "Enumeration of player status types.")]
    partial class CustomLogicPlayerStatusEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicPlayerStatusEnum() { }

        [CLProperty("Player is alive.")]
        public static string Alive => "Alive";

        [CLProperty("Player is dead.")]
        public static string Dead => "Dead";

        [CLProperty("Player is spectating.")]
        public static string Spectating => "Spectating";
    }
}
