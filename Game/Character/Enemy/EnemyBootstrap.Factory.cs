using Game.Core.StaticData;
using UnityEngine;
using Zenject;

namespace Game.GamePlay.Character.Enemy
{
    public partial class EnemyBootstrap
    {
        public class MeleeFactory : PlaceholderFactory<EnemyStaticData, GameObject, EnemyBootstrap>
        {
        }
        public class RangedFactory : PlaceholderFactory<EnemyStaticData, GameObject, EnemyBootstrap>
        {
        }
    }
}