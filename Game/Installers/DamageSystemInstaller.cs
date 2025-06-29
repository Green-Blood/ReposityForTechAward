using System;
using System.Linq;
using Extensions.Enums.Types;
using Game.GamePlay.Character.Base.DamageCalculators.Interfaces;
using Game.GamePlay.Character.Base.Health;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class DamageSystemInstaller : MonoInstaller
    {
        public override void InstallBindings() 
        {
            var damageCalculatorTypes = typeof(IDamageCalculator).Assembly.GetTypes()
                .Where(t => typeof(IDamageCalculator).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToDictionary(t => (DamageType)Enum.Parse(typeof(DamageType), t.Name.Replace("DamageCalculator", "")));
            
            
            foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
            {
                if (damageCalculatorTypes.TryGetValue(damageType, out var calculatorType))
                {
                    // Bind the calculator to its DamageType
                    Container.Bind<IDamageCalculator>()
                        .WithId(damageType)
                        .To(calculatorType)
                        .AsTransient();
                }
                else
                {
                    // Handle the case where no corresponding calculator is found
                    Debug.LogWarning($"No IDamageCalculator implementation found for DamageType: {damageType}");
                }
            }
            
            Container.BindInterfacesAndSelfTo<DamageService>().AsSingle();
        }
    }
}