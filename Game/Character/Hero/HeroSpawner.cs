using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Extensions.Enums.Types;
using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using Game.GamePlay.Character.Hero.Interfaces;
using Game.GamePlay.WaveSystem;
using UnityEngine;

namespace Game.GamePlay.Character.Hero
{
    public class HeroSpawner : IHeroSpawner
    {
        private readonly SpawnPoint[] _spawnPoints;

        // TODO Change factories to smth better! 
        private readonly HeroBootstrap.MeleeFactory _meleeFactory;
        private readonly HeroBootstrap.RangedFactory _rangedFactory;
        private readonly IUnitFactory _unitFactory;
        private readonly IAssetProvider _assetProvider;
        private readonly IParentProvider _parentProvider;
        private readonly IStoppableService _stoppableService;
        public List<HeroBootstrap> SpawnedHeroes { get; }

        public HeroSpawner(
            SpawnPoint[] spawnPoints,
            HeroBootstrap.MeleeFactory meleeFactory,
            HeroBootstrap.RangedFactory rangedFactory,
            IUnitFactory unitFactory,
            IAssetProvider assetProvider,
            IParentProvider parentProvider,
            IStoppableService stoppableService
        )
        {
            _spawnPoints = spawnPoints;
            _meleeFactory = meleeFactory;
            _rangedFactory = rangedFactory;
            _unitFactory = unitFactory;
            _assetProvider = assetProvider;
            _parentProvider = parentProvider;
            _stoppableService = stoppableService;
            SpawnedHeroes = new List<HeroBootstrap>();
        }

        public async UniTask Spawn()
        {
            // var heroDatas = _loadoutData.SelectedHeroIds
            // .Select(id => _unitFactory.GetHeroById(id))
            // .Where(data => data != null)
            // .ToList();

            // for (int i = 0; i < heroDatas.Count && i < _spawnPoints.Length; i++)
            // {
            // await SpawnHero(heroDatas[i], _spawnPoints[i]);
            // }
            // await SpawnHero("Swordsman", _spawnPoints[0]);
        }

        public async UniTask SpawnHero(string heroName, SpawnPoint spawnPoint)
        {
            var heroData = _unitFactory.GetHero(heroName);
            var heroPrefab = await _assetProvider.Load<GameObject>(heroData.PrefabReference);

            if(heroPrefab != null)
            {
                // TODO NO! THAT'S A BULLSHIT CODE! WHY FACTORIES WORK LIKE THAT?
                var hero = heroData.IsRanged ? _rangedFactory.Create(heroData, heroPrefab) : _meleeFactory.Create(heroData, heroPrefab);

                SpawnedHeroes.Add(hero);
                var heroTransform = _parentProvider.GetParent(RegistrarTypes.Hero);

                if(heroTransform != null)
                {
                    hero.transform.SetParent(heroTransform);
                }

                if(spawnPoint == null) return;
                hero.transform.position = spawnPoint.transform.position;
                hero.OnObjectSpawn();
                _stoppableService.Register(hero);
            }
        }
    }
}