using Cysharp.Threading.Tasks;
using UI.Configs;

namespace UI.Interfaces
{
    public interface IAnimationProvider
    {
        UniTask<UIAnimationData> GetBaseUIAnimationData();
    }
}