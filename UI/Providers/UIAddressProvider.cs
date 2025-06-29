using System;
using System.Collections.Generic;
using Game.Core.Interfaces;
using UI.Views;
using UI.Views.Currency;

namespace UI.Providers
{
    public class UIAddressProvider : IAddressProvider
    {
        private static readonly Dictionary<Type, string> AddressMap = new()
        {
            { typeof(CastleHealthView), "CastleHealthUI" },
            { typeof(GameEndPanelView), "GameDeadPanel" },
            { typeof(ResourcesHUDView), "ResourcesHUD" },
            // Add other UI element mappings here
        };
        private static readonly Dictionary<string, string> AnimationAddressMap = new()
        {
            { typeof(CastleHealthView).FullName ?? string.Empty, "CastleHealthAnimationData" },
            { typeof(GameEndPanelView).FullName ?? string.Empty, "GameEndPanelAnimationData" },
            { "BaseUIAnimationData", "BaseUIAnimationData" }  // Custom string keys
        };


        public string GetAddressForUIElement<TView>()
        {
            var viewType = typeof(TView);

            if(AddressMap.TryGetValue(viewType, out var address))
            {
                return address;
            }

            throw new InvalidOperationException($"No address found for UI element {viewType.Name}");
        }
        public string GetAddressForAnimationData<TView>()
        {
            var viewType = typeof(TView).FullName;

            if (viewType != null && AnimationAddressMap.TryGetValue(viewType, out var address))
            {
                return address;
            }

            throw new InvalidOperationException($"No address found for animation data {viewType}");
        }
        public string GetAddressForAnimationData(string viewName)
        {
            if (AnimationAddressMap.TryGetValue(viewName, out var address))
            {
                return address;
            }

            throw new InvalidOperationException($"No address found for animation data {viewName}");
        }
    }
}