using UnityEngine;

public interface IAttachable
{
    Rigidbody2D rigidbody2 { get; }
    PolygonCollider2D collider { get; }
    bool isAttached { get; }

    bool TryAttaching();
    void Drop();
}
