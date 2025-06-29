using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions.Enums.Types;
using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using Game.Core.Providers;
using Game.GamePlay.AttackZone;
using Game.GamePlay.AttackZone.Interfaces;
using Game.GamePlay.Castle;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class CastleObjectsSpawner : ISpawner
    {
        private readonly IAssetProvider _assetProvider;
        private readonly DiContainer _container;
        private readonly ITransformView _castleView;
        private readonly IGameFactory _gameFactory;
        private readonly Catapult.Factory _catapultFactory;
        private readonly IObjectRegistrar _projectileRegistrar;
        private readonly IStoppableService _stoppableService;
        private readonly IAttackZoneIndicatorService _attackZoneService;

        // TODO Take it from someSettings or smth
        private readonly Vector3 _offset = new(0, 3.7f);

        public CastleObjectsSpawner(
            IAssetProvider assetProvider,
            DiContainer container,
            ITransformView castleView,
            IGameFactory gameFactory,
            Catapult.Factory catapultFactory,
            IObjectRegistrar projectileRegistrar,
            IStoppableService stoppableService,
            IAttackZoneIndicatorService attackZoneService
        )
        {
            _assetProvider = assetProvider;
            _container = container;
            _castleView = castleView;
            _gameFactory = gameFactory;
            _catapultFactory = catapultFactory;
            _projectileRegistrar = projectileRegistrar;
            _stoppableService = stoppableService;
            _attackZoneService = attackZoneService;

            Initialize();
        }

        private async void Initialize()
        {
            await Spawn();
        }

        // TODO Maybe move to Factories/Installers already?
        public async UniTask Spawn()
        {
            await _projectileRegistrar.Register();

            var cursor = await _gameFactory.CreateCursor();
            _container.Bind<AttackZoneIndicatorView>().FromInstance(cursor).AsSingle();

            // Bind Zone indicators
            _attackZoneService.Initialize(cursor);

            var catapult = await _assetProvider.Create<CatapultView>(AssetAddress.CatapultPath, _castleView.TransformView.position + _offset, _castleView.TransformView);
            _container.Bind<CatapultView>().FromInstance(catapult).AsSingle();

            var catapultObject = _catapultFactory.Create();
            _stoppableService.Register(catapultObject);
            catapultObject.Initialize();
        }
    }
}