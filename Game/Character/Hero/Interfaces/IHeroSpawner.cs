using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Core.Interfaces;
using Game.GamePlay.WaveSystem;

namespace Game.GamePlay.Character.Hero.Interfaces
{
    public interface IHeroSpawner : ISpawner
    {
        List<HeroBootstrap> SpawnedHeroes { get; }
        UniTask SpawnHero(string heroName, SpawnPoint spawnPoint);
    }
}
