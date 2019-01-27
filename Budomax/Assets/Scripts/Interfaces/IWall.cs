using UnityEngine;

namespace State
{
    public interface IWall : IDamageable
    {
        Vector2 Position { get; }

        int MaxHealth { get; }

        //int Health { get; }
    }
}
