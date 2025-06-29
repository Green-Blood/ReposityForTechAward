using Unity.Mathematics;
using UnityEngine;

namespace Game.GamePlay.Character.Base.Movement.Interfaces
{
    public interface IMovable
    {
        bool CanMove { get; }
        public Vector3 InitialPosition { get; }
        void MoveTo(float3 destination);
        void AllowMoving();
        void ForbidMoving();
        void ResetPosition();
    }
}