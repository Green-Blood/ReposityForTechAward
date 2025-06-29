using System;

namespace Systems.StatSystem.Interfaces
{
    public interface IStatCurrentValueChange
    {
        event EventHandler OnCurrentValueChange;
    }
}