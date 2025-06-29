using System.Collections.Generic;
using System.Linq;
using Game.Core.Factories.Interfaces;
using Game.GamePlay._Constants;
using Game.GamePlay.Character.Base;
using Game.GamePlay.Character.Hero.Interfaces;
using Game.GamePlay.Character.Other.Interfaces;
using UnityEngine;

namespace Game.GamePlay.Character.Other
{
    public class EnemyDistanceChecker : IDistanceChecker
    {
        private readonly ICharacterView _characterView;
        private readonly IShared _castle;
        private readonly List<IShared> _heroes = new List<IShared>();

        public EnemyDistanceChecker(ICharacterView characterView, IHeroSpawner heroSpawner, IUnitFactory unitFactory)
        {
            _characterView = characterView;
            _castle = unitFactory.GetUnit(StaticTexts.CASTLE);
            _heroes.AddRange(heroSpawner.SpawnedHeroes);
        }

        public bool IsAnybodyAround { get; private set; }

        public IShared GetCurrentClosestTargetShared()
        {
            var closestHero = _heroes
                .Where(hero => hero != null && !hero.UnitDeath.IsDead.Value)
                .OrderBy(hero => Vector3.Distance(hero.UnitObject.transform.position, _characterView.TransformView.position))
                .FirstOrDefault();

            if(closestHero != null)
            {
                IsAnybodyAround = true;
                return closestHero;
            }

            IsAnybodyAround = false;
            return _castle;
        }

        public Transform GetCurrentClosestTargetTransform()
        {
            var closestHero = _heroes
                .Where(hero => hero != null && !hero.UnitDeath.IsDead.Value)
                .OrderBy(hero => Vector3.Distance(hero.UnitObject.transform.position, _characterView.TransformView.position))
                .FirstOrDefault();

            if(closestHero != null)
            {
                IsAnybodyAround = true;
                return closestHero.UnitObject.transform;
            }

            IsAnybodyAround = false;
            return _castle.UnitObject.transform;
        }

        public void Reset()
        {
            IsAnybodyAround = false;
        }
    }
}