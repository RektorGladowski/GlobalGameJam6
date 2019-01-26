using System;
using UnityEngine;

namespace State
{
    public interface IBot
    {
        Vector2 Position { get; }

        void Move(Vector2 target);
    }
}
