using Cysharp.Threading.Tasks;
using Extensions.Enums.Types;
using Game.Core.Factories.Interfaces;
using Game.Core.Interfaces;
using UI.Configs;
using UI.Interfaces;
using UnityEngine;
using Zenject;

namespace UI.Factories
{
    public class GameUIFactory : IUIFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IParentProvider _parentProvider;
        private readonly IAddressProvider _addressProvider;

        private Transform _parent;
        private readonly DiContainer _container;
        private readonly IAnimationProvider _animationProvider;

        public GameUIFactory(IAssetProvider assetProvider, IParentProvider parentProvider, IAddressProvider addressProvider, DiContainer container, IAnimationProvider animationProvider)
        {
            _assetProvider = assetProvider;
            _parentProvider = parentProvider;
            _addressProvider = addressProvider;
            _container = container;
            _animationProvider = animationProvider;
        }

        public UniTask Warmup()
        {
            _parent = _parentProvider.GetParent(RegistrarTypes.UI);
            return UniTask.CompletedTask;
        }

        public async UniTask<TView> CreateUIElement<TView, TViewModel>(
            IViewModelProvider<TViewModel> viewModelProvider,
            IShared unit = null,
            Transform under = null,
            UIAnimationData animationData = null
            )
            where TView : MonoBehaviour, IView<TViewModel>
            where TViewModel : IViewModel
        {
            var address = _addressProvider.GetAddressForUIElement<TView>();
            var loadedPrefab = await _assetProvider.Load<GameObject>(address);

            var view = under != null ? 
                _container.InstantiatePrefabForComponent<TView>(loadedPrefab, Vector3.zero, Quaternion.identity, under) :
                _container.InstantiatePrefabForComponent<TView>(loadedPrefab, Vector3.zero, Quaternion.identity, _parent);
            
            view.transform.localScale = Vector3.one;

            if(animationData == null) animationData = await _animationProvider.GetBaseUIAnimationData(); 
            var viewModel = unit == null 
                ? viewModelProvider.CreateViewModel(animationData)
                : viewModelProvider.CreateViewModelForUnit(unit, animationData);
            
            view.Initialize(viewModel);

            return view;
        }

        public void CleanUp()
        {
        }
    }
}