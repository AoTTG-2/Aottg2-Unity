namespace CustomLogic
{
    [CLType]
    class CustomLogicPoint : CustomLogicClassInstanceBuiltin
    {
        private float _x;
        private float _y;
        
        public CustomLogicPoint() : base("Point")
        {
        }
        
        [CLProperty]
        public float X
        {
            get => _x;
            set => _x = value;
        }
        
        [CLProperty]
        public float Y
        {
            get => _y;
            set => _y = value;
        }
    }
}