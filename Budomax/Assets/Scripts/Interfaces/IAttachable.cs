using System.Collections.Generic;
using UnityEngine;

public interface IAttachable
{
    Rigidbody2D rigidbody2 { get; }
    PolygonCollider2D collider { get; }

    bool TryAttaching();
    void Drop();
}
