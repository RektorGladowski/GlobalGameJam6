using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigidbody2d { get { _rigidbody2d = GetComponent<Rigidbody2D>(); return _rigidbody2d; } }
    private Rigidbody2D _rigidbody2d;

    public float bulletSpeed = 3f;
    public float bulletCollideDistance = 0.2f;

    public void Move(Vector2 direction)
    {
        rigidbody2d.velocity = direction * bulletSpeed;
    }

    public void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bulletCollideDistance);

        for (int i = 0; i < colliders.Length; i++)
        {
            WarriorMachine warrior = colliders[i].GetComponent<WarriorMachine>();
            if(warrior != null) warrior.Damage(1);

            Obstacle obstacle = colliders[i].GetComponent<Obstacle>();
            if (obstacle != null) obstacle.Damage(1);

            if (colliders[i].GetComponent<IEnemy>() == null)
            {
                Destroy(gameObject);
            }
        }
    }

}
