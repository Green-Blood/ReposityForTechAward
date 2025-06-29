using System;

namespace Systems.StatSystem.Interfaces
{
    public interface IStatValueChange
    {
        event EventHandler OnValueChange;
    }
}