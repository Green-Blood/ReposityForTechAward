using Extensions.ExtensionMethods.Even_More_Extensions.RX;
using Game.Core.Factories.Interfaces;
using Game.GamePlay.Character.Base.Attack.Damage;
using Game.GamePlay.Character.Base.Health.Interfaces;

namespace Game.GamePlay.Character.Base.Health
{
    public class CollisionHandler : DisposableObject, ICollisionHandler
    {
        private readonly IDamageService _damageService;
        private readonly IShared _receiver;

        public CollisionHandler(IDamageService damageService, IShared receiver)
        {
            _damageService = damageService;
            _receiver = receiver;
            _receiver.CharacterView.CollisionObserver.OnHitCollisionOccured += Handle;
        }
        public void Handle(HitData hitData, IShared attacker)
        {
            _damageService.ApplyDamage(hitData, attacker, _receiver);
        }

        public override void Dispose()
        {
            base.Dispose();
            _receiver.CharacterView.CollisionObserver.OnHitCollisionOccured -= Handle;
        }
    }
}