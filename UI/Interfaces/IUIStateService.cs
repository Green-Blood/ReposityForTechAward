using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace UI.Interfaces
{
    public interface IUIStateService
    {
        UniTask InitializeHUD();
        UniTask InitializeAllPanels();
    }
}
