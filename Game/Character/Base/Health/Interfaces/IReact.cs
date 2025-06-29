using Game.Core.Factories.Interfaces;

namespace Game.GamePlay.Character.Base.Health.Interfaces
{
    public interface IReact
    {
        void Register(IShared character);
        void React();
    }
}