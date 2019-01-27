using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using State;

public class EnemyMachine : MonoBehaviour, IDamageable
{
    const float ACTION_TIME = .2f;
    const float MOVEMENT_FORCE = 1f;
    const float HEALTH_WEIGHT = 0.5f;
    const float DISTANCE_WEIGHT = 0.5f;
    const float TICK_INTERVAL = 1f;
    const float FIRE_RANGE = 2f;
    const int DAMAGE = 1;

    public GameObject HomeObject;
    IHome home; // TODO: Get from some singleton
    Rigidbody2D rb;
    StateMachine<EnemyStates> fsm;
    public int Health { get; set; }
    IDamageable target;

    void Awake()
    {
        Health = 5;
        home = HomeObject.GetComponent<IHome>();
        fsm = StateMachine<EnemyStates>.Initialize(this);
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        fsm.ChangeState(EnemyStates.SearchWall);
    }

    Vector2 Position { get => gameObject.transform.position; }

    void Move(Vector2 target)
    {
        Vector2 direction = target - Position;
        if (direction.magnitude < FIRE_RANGE / 2f) { return; }

        direction.Normalize();
        rb.AddForce(direction * MOVEMENT_FORCE);
    }

    // State transitions
    void SearchWall_FixedUpdate()
    {
        var warrior = NearbyWarrior();
        if (warrior != null)
        {
            target = warrior;
            fsm.ChangeState(EnemyStates.Fight);
            return;
        }

        // Find best wall
        IWall bestWall = null;
        float bestDistance = Mathf.Infinity;

        foreach (IWall wall in home.Walls)
        {
            float distance = (wall.Position - Position).magnitude;

            if (bestWall == null || (wall.Health > 0 && distance < bestDistance))
            {
                bestWall = wall;
                bestDistance = distance;
            }
        }

        if (bestWall?.Health == 0) { return; }

        // Walk
        Move(bestWall.Position);

        // Check if withing reach
        if (bestDistance > FIRE_RANGE) { return; }

        target = (IDamageable)bestWall;

        fsm.ChangeState(EnemyStates.DestroyWall);
    }

    IEnumerator DestroyWall_Enter()
    {

        while (target?.Health > 0)
        {
            // Look for alternative targets
            var warrior = NearbyWarrior();
            if (warrior != null)
            {
                target = warrior;
                fsm.ChangeState(EnemyStates.Fight);
                yield break;
            }

            // Wait and damage
            yield return new WaitForSeconds(ACTION_TIME);
            target?.Damage(DAMAGE); // TODO: Add projectiles
        }

        target = null;

        fsm.ChangeState(EnemyStates.SearchWall);
    }

    IEnumerator Fight_Enter()
    {
        // TODO: Stop attacking when out of range
        while (target?.Health > 0)
        {
            // Wait and damage
            yield return new WaitForSeconds(ACTION_TIME);
            target?.Damage(DAMAGE); // TODO: Add projectiles
        }

        target = null;

        fsm.ChangeState(EnemyStates.SearchWall);
    }

    IDamageable NearbyWarrior()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Position, FIRE_RANGE);

        IDamageable bestWarrior = null;
        float bestDistance = Mathf.Infinity;

        foreach (Collider2D col in colliders)
        {
            var warrior = col.GetComponent<WarriorMachine>();
            if (warrior == null) { continue; }

            float distance = ((Vector2)col.transform.position - Position).magnitude;

            if (bestWarrior == null || distance < bestDistance)
            {
                bestWarrior = warrior.GetComponent<IDamageable>();
                bestDistance = distance;
            }
        }

        return bestWarrior;
    }

    public void Damage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            // TODO: Destroy
        }
    }

}

public enum EnemyStates
{
    Fight,
    SearchWall,
    DestroyWall,
}
