using Cysharp.Threading.Tasks;

namespace UI.Interfaces
{
    public interface IView
    {
        UniTask Show();
        UniTask Hide();
    }

    public interface IView<TViewModel> : IView where TViewModel : IViewModel
    {
        void Initialize(TViewModel viewModel);
    }
}