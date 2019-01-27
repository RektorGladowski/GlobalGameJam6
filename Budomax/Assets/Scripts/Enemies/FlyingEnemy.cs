using UnityEngine;

public class FlyingEnemy : MonoBehaviour//, IEnemy
{
    private RaycastHit2D[] results = new RaycastHit2D[50];

    public new Collider2D collider { get; private set; }

    public float Health { get { return health; } }
    public float health;

    public float Cooldown { get { return cooldown; } }

    public float cooldown;
    public int reults;

    public void Attack()
    {

    }

    /*
    public void FindTile()
    {
        int reults = collider.Raycast(results);
    }

    private void GetDirectionToHome 
    */
}
