using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Extensions.Console.Interfaces;
using UI.Enums;
using UI.Interfaces;
using UnityEngine;
using Zenject;

namespace UI.Providers
{
    public class UIParentProvider : MonoBehaviour, IUIParentProvider
    {
        [SerializeField] private Transform hudParent;
        [SerializeField] private Transform mainParent;
        [SerializeField] private Transform headerParent;
        [SerializeField] private Transform footerParent;
        [SerializeField] private Transform bodyParent;

        private Lazy<Dictionary<UIParentType, Transform>> _parentMap;
        private IDebugLogger _logger;

        [Inject]
        private void Construct(IDebugLogger logger)
        {
            _logger = logger;
        }

        public async UniTask Warmup()
        {
            _parentMap = new Lazy<Dictionary<UIParentType, Transform>>(InitializeParentMap);

            await UniTask.CompletedTask;
        }


        public Transform GetParent(UIParentType parentType)
        {
            _parentMap.Value.TryGetValue(parentType, out var parent);

            if(parent == null)
            {
                _logger.LogWarning("Parent not found for RegistrarType: " + parentType + " Going to default parent");
            }

            return parent;
        }

        private Dictionary<UIParentType, Transform> InitializeParentMap()
        {
            var map = new Dictionary<UIParentType, Transform>();

            var fields = GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.FieldType == typeof(Transform));

            foreach (var field in fields)
            {
                foreach (UIParentType registrarType in Enum.GetValues(typeof(UIParentType)))
                {
                    if(field.Name.Contains(registrarType.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        map.Add(registrarType, (Transform)field.GetValue(this));
                        _logger?.Log($"Successfully registered {registrarType} with field {field.Name}");
                        break;
                    }
                }
            }

            return map;
        }

        public void CleanUp()
        {
            _parentMap.Value.Clear();
        }
    }
}