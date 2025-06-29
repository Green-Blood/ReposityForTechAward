namespace Game.GamePlay.Character.Base.Attack.Interfaces
{
    public interface IAttack
    {
        bool CanAttack();
        void TryAttack();
    }
}