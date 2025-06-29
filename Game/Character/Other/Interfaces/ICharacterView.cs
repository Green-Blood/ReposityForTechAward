using Game.Core.Interfaces;

namespace Game.GamePlay.Character.Other.Interfaces
{
    public interface ICharacterView : ITransformView, IAnimatorView, IAgentView, IPhysicsView, IReactionObserver
    {
      
    }
}