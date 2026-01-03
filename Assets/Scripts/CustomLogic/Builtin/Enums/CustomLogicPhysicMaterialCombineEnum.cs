namespace CustomLogic
{
    [CLType(Name = "PhysicMaterialCombineEnum", Static = true, Abstract = true, Description = "Enumeration of physics material combine modes for friction and bounce.")]
    partial class CustomLogicPhysicMaterialCombineEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicPhysicMaterialCombineEnum() { }

        [CLProperty("Minimum combine mode.")]
        public static int Minimum => 0;

        [CLProperty("Multiply combine mode.")]
        public static int Multiply => 1;

        [CLProperty("Maximum combine mode.")]
        public static int Maximum => 2;

        [CLProperty("Average combine mode.")]
        public static int Average => 3;
    }
}
