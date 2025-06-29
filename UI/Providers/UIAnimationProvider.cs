using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Core.Interfaces;
using UI.Configs;
using UI.Interfaces;
using UnityEngine;

namespace UI.Providers
{
    public class UIAnimationProvider : IAnimationProvider
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IAddressProvider _addressProvider;
        private UIAnimationData _baseAnimationData;

        public UIAnimationProvider(IAssetProvider assetProvider, IAddressProvider addressProvider)
        {
            _assetProvider = assetProvider;
            _addressProvider = addressProvider;
        }

        public async UniTask<UIAnimationData> LoadUIAnimationData(string animationDataAddress)
        {
            if(string.IsNullOrEmpty(animationDataAddress))
            {
                Debug.LogWarning("No animation data address provided, falling back to default.");
                return _baseAnimationData;
            }

            var animationData = await _assetProvider.Load<UIAnimationData>(animationDataAddress);

            return animationData ?? _baseAnimationData;
        }

        public async UniTask<UIAnimationData> GetBaseUIAnimationData()
        {
            if(_baseAnimationData != null)
            {
                return _baseAnimationData;
            }

            _baseAnimationData = await LoadBaseUIAnimationData();
            return _baseAnimationData ?? CreateNewDefaultAnimationData();
        }

        private async UniTask<UIAnimationData> LoadBaseUIAnimationData()
        { 
            var baseAnimationDataAddress = _addressProvider.GetAddressForAnimationData("BaseUIAnimationData");
             
            var baseData = await _assetProvider.Load<UIAnimationData>(baseAnimationDataAddress);

            if(baseData == null)
            {
                Debug.LogWarning("Failed to load base UIAnimationData, no animation data found at the address.");
            }

            return baseData;
        }

        private UIAnimationData CreateNewDefaultAnimationData()
        {
            Debug.LogWarning("No base animation data found, creating new default animation data.");
            return ScriptableObject.CreateInstance<UIAnimationData>();
        }
    }
}