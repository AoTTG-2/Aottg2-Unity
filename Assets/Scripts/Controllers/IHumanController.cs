namespace Controllers
{
    interface IHumanController
    {
        public bool LeftGetKey();
        public bool RightGetKey();

        public bool JumpGetKey();

        public bool HookLeftGetKey();

        public bool HookRightGetKey();

        public bool HookBothGetKey();
    }
}