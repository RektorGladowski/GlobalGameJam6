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
    const float FIRE_RANGE = 3f;
    const int DAMAGE = 1;
    public AudioManager am;

    public GameObject bullet;
    public GameObject turretBullet;
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

    void Start()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Health = 5;
        home = HouseManager.instance.GetComponent<IHome>();// ?? HomeObject.GetComponent<IHome>();
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
        if (turret == null) { return; }
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
        am.playAudio("RecieveDamage", 0.3f);

        if (Health <= 0)
        {
            // TODO: Destroy
            am.playAudio("UnitDeath", 0.3f);
            Destroy(gameObject);
        }
    }

    GameObject NearbyEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Position, FIRE_RANGE * 10);

        GameObject bestEnemy = null;
        float bestDistance = Mathf.Infinity;

        foreach (Collider2D col in colliders)
        {
            var enemy = col.GetComponent<IEnemy>();
            if (enemy == null) { continue; }

            float distance = ((Vector2)col.transform.position - Position).magnitude;

            if (bestEnemy == null || distance < bestDistance)
            {
                bestEnemy = col.gameObject;//col.GetComponent<IDamageable>();
                bestDistance = distance;
            }
        }

        return bestEnemy;
    }

    // State transitions
    void Patrol_FixedUpdate()
    {
        // Check hunger
        //if (satiation == 0)
        //{
        //    fsm.ChangeState(WarriorStates.WannaEat);
        //    return;
        //}

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


        // Reserve a spot
        if (bestTurret != null && reservedTurret != bestTurret)
        {
            if (reservedTurret != null) { CancelReservation(); }
            Reserve(bestTurret);
        }

        if (reservedTurret == null)
        {
            targetEnemy = NearbyEnemy();

            if (targetEnemy == null) { return; }

            Debug.Log("TARGET " + targetEnemy.transform.position);
            Move(targetEnemy.transform.position);

            float distance = ((Vector2)targetEnemy.transform.position - Position).magnitude;
            if (distance > USE_RADIUS) { return; }

            var dmg = targetEnemy.GetComponent<IEnemy>();
            dmg?.Damage(DAMAGE);

            return;
        }

        // Walk
        Move(reservedTurret.Position);

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
        am.playAudio("Eating", 0.3f);
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

            // Choose target
            if (targetEnemy == null)
            {
                targetEnemy = NearbyEnemy();
            }
            else
            {
                float distance = ((Vector2)targetEnemy.transform.position - Position).magnitude;
                if (distance > FIRE_RANGE)
                {
                    targetEnemy = NearbyEnemy();
                }
            }

            // Fire
            // TODO: Hide warrior, rotate turret
            am.playAudio("TurretShot", 0.3f);
            var dmg = targetEnemy.GetComponent<IDamageable>();
            dmg?.Damage(DAMAGE);

            yield return new WaitForSeconds(ACTION_TIME);
        }
    }

    void Turret_Exit()
    {
        targetEnemy = null;
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
