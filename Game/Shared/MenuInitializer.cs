using System;
using Cysharp.Threading.Tasks;
using Extensions.Enums.Types;
using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using Game.Core.StateMachines;
using Game.GamePlay.Character.Base.Health.Interfaces;
using Game.GamePlay.Character.Hero.Interfaces;
using Game.GamePlay.WaveSystem.Interfaces;
using Game.Object_Pooler.Interfaces;
using UI.Interfaces;
using Zenject;

namespace Game.Shared
{
    public class MenuInitializer : IInitializable, IDisposable
    {
        private readonly StateMachine _stateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IObjectPooler _objectPooler;
        private readonly IUnitFactory _unitFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly ILevelService _levelService;
        private readonly IHeroSpawner _heroSpawner;
        private readonly IWaveSpawner _waveSpawner;
        private readonly IDamageService _damageService;
        private readonly IParentProvider _parentProvider;
        private readonly IObjectRegistrar _enemyRegistrar;
        private readonly IObjectRegistrar _abilityVFXRegistrar;
        private readonly IObjectRegistrar _uiRegistrar;
        private readonly IUIStateService _uiStateService;
        private readonly IUIFactory _uiFactory;
        private readonly IUIParentProvider _uiParentProvider;
        private readonly IStoppableService _stoppableService;

        public MenuInitializer(
            StateMachine stateMachine,
            IGameFactory gameFactory,
            IObjectPooler objectPooler,
            IUnitFactory unitFactory,
            IStaticDataService staticDataService,
            ILevelService levelService,
            IHeroSpawner heroSpawner,
            IWaveSpawner waveSpawner,
            IDamageService damageService,
            IParentProvider parentProvider,
            [Inject(Id = RegistrarTypes.VFX)] IObjectRegistrar abilityVFXRegistrar,
            [Inject(Id = RegistrarTypes.Enemy)] IObjectRegistrar enemyRegistrar,
            IUIRegistrar uiRegistrar,
            IUIStateService uiStateService,
            IUIFactory uiFactory,
            IUIParentProvider uiParentProvider,
            IStoppableService stoppableService
        )
        {
            _stateMachine = stateMachine;
            _gameFactory = gameFactory;
            _objectPooler = objectPooler;
            _unitFactory = unitFactory;
            _staticDataService = staticDataService;
            _levelService = levelService;
            _heroSpawner = heroSpawner;
            _waveSpawner = waveSpawner;
            _enemyRegistrar = enemyRegistrar;
            _uiRegistrar = uiRegistrar;
            _uiStateService = uiStateService;
            _uiFactory = uiFactory;
            _uiParentProvider = uiParentProvider;
            _stoppableService = stoppableService;
            _damageService = damageService;
            _parentProvider = parentProvider;
            _abilityVFXRegistrar = abilityVFXRegistrar;

            stateMachine.OnStateChange += OnStateChange;
        }

        private void OnStateChange(IState state)
        {
            switch (state)
            {
                case GameEndState:
                    _stoppableService.StopAll();
                    break;
                case GamePauseState:
                    _stoppableService.PauseAll();
                    break;
                case GameLoopState:
                    _stoppableService.ResumeAll();
                    break;
            }
        }

        public async void Initialize()
        {
            _gameFactory.CleanUp();
            _staticDataService.Load();

            await _levelService.Warmup();
            await _damageService.Warmup();
            await _parentProvider.Warmup();
            await _uiParentProvider.Warmup();
            await _waveSpawner.Warmup();

            await _objectPooler.WarmUp();
            await _gameFactory.Warmup();
            await _unitFactory.Warmup();
            await _uiFactory.Warmup();

            await _gameFactory.CreateAbilitiesVFX();

            await _heroSpawner.Spawn();
            await _abilityVFXRegistrar.Register();

            await _enemyRegistrar.Register();
            await _uiRegistrar.Register();
            await _uiStateService.InitializeAllPanels();

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            await _waveSpawner.SpawnContinually();

            _stateMachine.Enter<GameLoopState>();
        }

        public void Dispose()
        {
            _stateMachine.OnStateChange -= OnStateChange;

            _levelService.CleanUp();
            _damageService.CleanUp();
            _parentProvider.CleanUp();
            _uiParentProvider.CleanUp();
            _waveSpawner.CleanUp();

            _gameFactory.CleanUp();
            _unitFactory.CleanUp();
            _uiFactory.CleanUp();
        }
    }
}