using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public interface IRoom
    {
        Vector2 Position { get; }

        Building Type { get; }

        IEnumerable<IWall> Walls { get; }

        int MaxWorkers { get; } // Capacity

        int Workers { get; }

        void Enter();

        void Leave();
    }
}

public enum Building
{
    Construction,
    Barracks,
    Kitchen,
    Wasteland,
}
