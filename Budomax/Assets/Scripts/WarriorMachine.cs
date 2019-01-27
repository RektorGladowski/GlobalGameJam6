using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using State;

public class WarriorMachine : MonoBehaviour, IDamageable
{
    const float USE_RADIUS = 0.25f;
    const int ACTION_TIME = 4;
    const float MOVEMENT_FORCE = 1f;
    const float FOOD_WEIGHT = 0.5f;
    const float DISTANCE_WEIGHT = 0.5f;
    const float TICK_INTERVAL = 1f;

    public GameObject HomeObject;
    IHome home; // TODO: Get from some singleton
    Rigidbody2D rb;
    StateMachine<WarriorStates> fsm;
    ITurret reservedTurret;
    int satiation = 0;
    int maxSatiation = 100;
    IPantry visitedPantry;
    ITurret visitedTurret;
    float timeElapsed;
    GameObject targetEnemy;
    public int Health { get; private set; }

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

        fsm.ChangeState(WarriorStates.Patrol);
    }

    private void Update()
    {
        // Debug.Log("Satiation" + satiation);

        timeElapsed += Time.fixedDeltaTime;

        if (timeElapsed < TICK_INTERVAL) { return; }

        timeElapsed = 0f;

        // Hunger
        if (satiation > 0)
        {
            satiation -= 1;
        }
    }

    Vector2 Position { get => gameObject.transform.position; }

    void Move(Vector2 target)
    {
        Vector2 direction = target - Position;
        if (direction.magnitude < USE_RADIUS / 2f) { return; }

        direction.Normalize();
        rb.AddForce(direction * MOVEMENT_FORCE);
    }

    void Reserve(ITurret turret)
    {
        turret.Enter();
        reservedTurret = turret;
    }

    void CancelReservation()
    {
        reservedTurret?.Leave();
        reservedTurret = null;
    }

    public void Damage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            // TODO: Destroy
        }
    }

    // State transitions
    void Patrol_FixedUpdate()
    {
        // Check hunger
        if (satiation == 0)
        {
            fsm.ChangeState(WarriorStates.WannaEat);
            return;
        }

        // Find best turret
        ITurret bestTurret = null;
        float bestDistance = Mathf.Infinity;

        foreach (IWall wall in home.Walls)
        {
            if (wall is ITurret turret)
            {
                float distance = (wall.Position - Position).magnitude;

                if ((bestTurret == null && turret.IsEmpty) || (turret.IsEmpty && distance < bestDistance))
                {
                    bestTurret = turret;
                    bestDistance = distance;
                }
            }
        }

        if (bestTurret == null) { return; }

        // Reserve a spot
        if (reservedTurret != bestTurret)
        {
            if (reservedTurret!= null) { CancelReservation(); }
            Reserve(bestTurret);
        }

        // Walk
        Move(bestTurret.Position);

        // Check if within reach
        if (bestDistance > USE_RADIUS) { return; }

        fsm.ChangeState(WarriorStates.Turret);
    }

    void WannaEat_FixedUpdate()
    {
        IPantry bestPantry = null;
        float bestScore = 0f;

        foreach (IWall wall in home.Walls)
        {
            if (wall is IPantry pantry)
            {
                int food = pantry.Food;
                float distance = (wall.Position - Position).magnitude;
                float score = food * FOOD_WEIGHT + distance * DISTANCE_WEIGHT;

                if (pantry == null || score > bestScore)
                {
                    bestPantry = pantry;
                    bestScore = score;
                }
            }
        }

        // Go towards pantry
        if (bestPantry == null) { return; }

        Move(bestPantry.Position);

        // Check if withing reach
        float bestDistance = (bestPantry.Position - Position).magnitude;
        if (bestDistance > USE_RADIUS) { return; }

        visitedPantry = bestPantry;

        fsm.ChangeState(WarriorStates.Pantry);
    }

    IEnumerator Pantry_Enter()
    {
        yield return new WaitForSeconds(ACTION_TIME);

        // Eat
        if (visitedPantry.Food == 0)
        {
            fsm.ChangeState(WarriorStates.WannaEat);
            yield break;
        }

        int meal = (int)Mathf.Min(visitedPantry.Food, maxSatiation - satiation);
        visitedPantry.Eat(meal);
        satiation += meal;

        fsm.ChangeState(WarriorStates.Patrol);
    }

    void Pantry_Exit()
    {
        visitedPantry = null;
    }

    //void Mount_FixedUpdate()
    //{
    //    // Check hunger
    //    if (satiation == 0)
    //    {
    //        fsm.ChangeState(WarriorStates.WannaEat);
    //        return;
    //    }

    //    // Look for nearest available turret
    //    ITurret bestTurret = null;
    //    float bestDistance = 0f;

    //    foreach (IWall wall in home.Walls)
    //    {
    //        if (wall is ITurret turret)
    //        {
    //            float distance = (wall.Position - Position).magnitude;

    //            if ((bestTurret == null && turret.IsEmpty) || (turret.IsEmpty && distance < bestDistance))
    //            {
    //                bestTurret = turret;
    //                bestDistance = distance;
    //            }
    //        }
    //    }

    //    // Go towards pantry
    //    if (bestTurret == null) { return; }

    //    Move(bestTurret.Position);

    //    // Check if within reach
    //    if (bestDistance <= USE_RADIUS) {
    //        visitedTurret = bestTurret;
    //        fsm.ChangeState(WarriorStates.Turret);
    //        return; 
    //    }

    //    // TODO: Otherwise go to random wall
    //    // ...
    //}

    IEnumerator Turret_Enter()
    {
        yield return new WaitForSeconds(ACTION_TIME);

        while(true)
        {
            // Check hunger
            if (satiation == 0)
            {
                fsm.ChangeState(WarriorStates.WannaEat);
                yield break;
            }

            // TODO: Look for nearby enemies
            // ...
        }
    }

    void Turret_Leave()
    {
        CancelReservation();
    }
}

public enum WarriorStates
{
    Patrol,
    Fight,
    WannaEat,
    Pantry,
    Mount,
    Turret,
}
