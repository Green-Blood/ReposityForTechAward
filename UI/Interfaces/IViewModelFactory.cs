namespace UI.Interfaces
{
    public interface IViewModelFactory
    {
        TViewModel Create<TViewModel>(params object[] args) where TViewModel : IViewModel;
    }
}