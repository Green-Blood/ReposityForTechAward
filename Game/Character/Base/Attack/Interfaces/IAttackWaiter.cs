namespace Game.GamePlay.Character.Base.Attack.Interfaces
{
    public interface IAttackWaiter
    {
        public bool IsAttackCooldownFinished();
        public void UpdateAttackCooldown();
    }
}