namespace UI
{
    class CategoryPanel : BasePanel
    {
        protected override float GetWidth()
        {
            return Parent.GetPanelWidth();
        }

        protected override float GetHeight()
        {
            return Parent.GetPanelHeight();
        }
    }
}
