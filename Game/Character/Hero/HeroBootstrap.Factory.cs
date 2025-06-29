using Game.Core.StaticData;
using UnityEngine;
using Zenject;
namespace Game.GamePlay.Character.Hero
{
    public partial class HeroBootstrap
    {
        public class MeleeFactory : PlaceholderFactory<HeroStaticData, GameObject, HeroBootstrap>
        {
        }
        public class RangedFactory : PlaceholderFactory<HeroStaticData, GameObject, HeroBootstrap>
        {
        }
    }
}
