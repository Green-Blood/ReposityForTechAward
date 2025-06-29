using Systems.StatSystem.StatTypes;

namespace Systems.StatSystem.Interfaces
{
    public interface IStatLinkable
    {
        int StatLinkerValue { get; }
        void AddLinker(StatLinker linker);
        void RemoveLinker(StatLinker linker);
        void ClearLinkers();
        void UpdateLinkers();
    }
}