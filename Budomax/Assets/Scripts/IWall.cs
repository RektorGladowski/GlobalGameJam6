using UnityEngine;

namespace State
{
    public interface IWall
    {
        Vector2 Position { get; }

        int MaxHealth { get; }

        int Health { get; }
    }
}
