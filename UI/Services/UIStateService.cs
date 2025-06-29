using Cysharp.Threading.Tasks;
using Extensions.Console.Interfaces;
using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using Game.Core.StateMachines;
using Game.GamePlay._Constants;
using Game.GamePlay.Character.Base;
using UI.Enums;
using UI.Interfaces;
using UI.ViewModels;
using UI.Views;
using UI.Views.Currency;
using UnityEngine;
using Zenject;

namespace UI.Services
{
    public class UIStateService : IUIStateService
    {
        private readonly IUIFactory _uiFactory;
        private readonly IUIRegistrar _uiRegistrar;
        private readonly IUIService _uiService;
        private readonly IUnitFactory _unitFactory;
        private readonly IDebugLogger _logger;
        private readonly DiContainer _container;
        private readonly IUIParentProvider _parentProvider;

        public UIStateService(
            IUIFactory uiFactory,
            IUIRegistrar uiRegistrar,
            IUIService uiService,
            IUnitFactory unitFactory,
            IDebugLogger logger,
            DiContainer container,
            IUIParentProvider parentProvider,
            StateMachine stateMachine
        )
        {
            _uiFactory = uiFactory;
            _uiRegistrar = uiRegistrar;
            _uiService = uiService;
            _unitFactory = unitFactory;
            _logger = logger;
            _container = container;

            _parentProvider = parentProvider;
            stateMachine.OnStateChange += OnStateChange;
        }

        private void OnStateChange(IState state)
        {
            if(state is GameEndState)
            {
                _uiService.Show<GameEndPanelView>();
            }
        }

        public async UniTask InitializeAllPanels()
        {
            await InitializeHUD();
            await RegisterGameEndPanel();
        }

        public async UniTask InitializeHUD()
        {
            if(!_uiRegistrar.IsRegistered<CastleHealthView>())
            {
                var castleUnit = _unitFactory.GetUnit(StaticTexts.CASTLE);
                if(castleUnit == null) _logger.LogError($"Castle Unit with name {StaticTexts.CASTLE} not found");
                await RegisterUIElement<CastleHealthView, CastleHealthViewModel>(UIParentType.HUD, castleUnit);
            }

            if(!_uiRegistrar.IsRegistered<ResourcesHUDView>())
            {
                await RegisterUIElement<ResourcesHUDView, ResourcesHUDViewModel>(UIParentType.Header);
            }

            _uiService.Show<CastleHealthView>();
            _uiService.Show<ResourcesHUDView>();
        }

        private async UniTask RegisterGameEndPanel()
        {
            if(!_uiRegistrar.IsRegistered<GameEndPanelView>())
            {
                await RegisterUIElement<GameEndPanelView, GameEndPanelViewModel>(UIParentType.Main);
                _uiService.Hide<GameEndPanelView>();
            }
        }

        private async UniTask RegisterUIElement<TView, TViewModel>(UIParentType parentType, IShared unit = null)
            where TView : MonoBehaviour, IView<TViewModel>
            where TViewModel : IViewModel =>
            _uiRegistrar.Register(unit == null
                                      ? await _uiFactory.CreateUIElement<TView, TViewModel>(_container.Resolve<IViewModelProvider<TViewModel>>(),
                                                                                            under: _parentProvider.GetParent(parentType))
                                      : await _uiFactory.CreateUIElement<TView, TViewModel>(_container.Resolve<IViewModelProvider<TViewModel>>(),
                                                                                            unit,
                                                                                            _parentProvider.GetParent(parentType)));
    }
}