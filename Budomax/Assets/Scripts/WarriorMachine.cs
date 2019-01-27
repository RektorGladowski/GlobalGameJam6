using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using State;

public class WarriorMachine : MonoBehaviour, IDamageable
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
    StateMachine<WarriorStates> fsm;
    public int Health { get; set; }
    IDamageable target;

    void Awake()
    {
        Health = 5;
        home = HomeObject.GetComponent<IHome>();
        fsm = StateMachine<WarriorStates>.Initialize(this);
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        fsm.ChangeState(WarriorStates.Idle);
    }

    Vector2 Position { get => gameObject.transform.position; }

    void Move(Vector2 target)
    {
        Vector2 direction = target - Position;
        if (direction.magnitude < FIRE_RANGE / 2f) { return; }

        direction.Normalize();
        rb.AddForce(direction * MOVEMENT_FORCE);
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

public enum WarriorStates
{
    Idle,
}
