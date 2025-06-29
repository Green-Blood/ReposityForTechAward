using System.Collections.Generic;
using Game.GamePlay.Character.Base.Health.Interfaces;
using JetBrains.Annotations;

namespace Game.GamePlay.Character.Other.Interfaces
{
    public interface IReactionObserver
    {
        [CanBeNull] List<IReact> Reactions { get; set; }
    }
}