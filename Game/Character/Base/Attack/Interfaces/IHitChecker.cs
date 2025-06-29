using UnityEngine;

namespace Game.GamePlay.Character.Base.Attack.Interfaces
{
    public interface IHitChecker
    {
        void Hit(out Collider[] colliders);
    }
}