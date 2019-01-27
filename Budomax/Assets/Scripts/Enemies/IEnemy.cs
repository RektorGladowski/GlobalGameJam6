using UnityEngine;

public interface IEnemy
{
    Collider2D collider { get; }
    float Health { get; }
    float Cooldown { get; }
    void Attack();
    void FindTile();
}
