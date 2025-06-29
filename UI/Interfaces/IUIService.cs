namespace UI.Interfaces
{
    public interface IUIService
    {
        public void Show<TView>() where TView : IView;
        public void Hide<TView>() where TView : IView;
        public void HideAll();
    }
}