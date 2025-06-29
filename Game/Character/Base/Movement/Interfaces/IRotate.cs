using UnityEngine;

namespace Game.GamePlay.Character.Base.Movement.Interfaces
{
    public interface IRotate
    {
        void RotateTowards(Vector3 targetPosition, float rotationSpeed);
        void ResetRotation();
    }
}