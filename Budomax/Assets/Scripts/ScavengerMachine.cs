using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using State;

public class ScavengerMachine : MonoBehaviour
{
    const float USE_RADIUS = 0.25f;
    const int ACTION_TIME = 2;
    const float MOVEMENT_FORCE = 3f;
    const float FOOD_WEIGHT = 0.5f;
    const float DISTANCE_WEIGHT = 0.5f;
    const float TICK_INTERVAL = 1f;

    public GameObject[] walls; // TODO: Use "material manager"
    public GameObject HomeObject;
    IHome home; // TODO: Get from some singleton
    Rigidbody2D rb;
    StateMachine<ScavengerStates> fsm;
    IRoom reservedRoom;
    int satiation = 0;
    int maxSatiation = 100;
    IPantry visitedPantry;
    Vector2 scavengeTarget;
    float timeElapsed;
    GameObject carriedWall;

    void Start()
    {
        home = HouseManager.instance.GetComponent<IHome>();// ?? HomeObject.GetComponent<IHome>();
        fsm = StateMachine<ScavengerStates>.Initialize(this);
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        fsm.ChangeState(ScavengerStates.WannaRest);
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

    void Reserve(IRoom room)
    {
        room.Enter();
        reservedRoom = room;
    }

    void CancelReservation()
    {
        reservedRoom?.Leave();
        reservedRoom = null;
    }

    // State transitions
    void WannaRest_FixedUpdate()
    {
        // Hack until there are working rooms
        fsm.ChangeState(ScavengerStates.WannaScavenge);
        return;

        // Find best scavenger room
        IRoom bestRoom = null;
        float bestDistance = Mathf.Infinity;

        foreach (IRoom room in home.Rooms)
        {
            if (room.Type == RoomTypeSelection.ScavengerRoom)
            {
                float distance = (room.Position - Position).magnitude;
                bool isAvailable = room.Workers < room.MaxWorkers;

                if (bestRoom == null || (isAvailable && distance < bestDistance))
                {
                    bestRoom = room;
                    bestDistance = distance;
                }
            }
        }

        if (bestRoom == null) { return; }

        // Reserve a spot
        if (reservedRoom != bestRoom)
        {
            if (reservedRoom != null) { CancelReservation(); }
            Reserve(bestRoom);
        }

        // Walk
        Move(bestRoom.Position);

        // Check if withing reach
        if (bestDistance > USE_RADIUS) { return; }

        fsm.ChangeState(ScavengerStates.Bedroom);
    }

    IEnumerator Bedroom_Enter()
    {
        yield return new WaitForSeconds(ACTION_TIME);

        if (satiation == 0)
        {
            fsm.ChangeState(ScavengerStates.WannaEat);
            yield break;
        }

        fsm.ChangeState(ScavengerStates.WannaScavenge);
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

        fsm.ChangeState(ScavengerStates.Pantry);
    }

    IEnumerator Pantry_Enter()
    {
        yield return new WaitForSeconds(ACTION_TIME);

        // Eat
        if (visitedPantry.Food == 0) {
            fsm.ChangeState(ScavengerStates.WannaEat);
            yield break;
        }

        int meal = (int)Mathf.Min(visitedPantry.Food, maxSatiation - satiation);
        visitedPantry.Eat(meal);
        satiation += meal;

        fsm.ChangeState(ScavengerStates.WannaScavenge);
    }

    void Pantry_Exit()
    {
        visitedPantry = null;
    }

    void WannaScavenge_Enter()
    {
        // Find random place for scavanging
        scavengeTarget = new Vector2(Random.Range(-20f, 20f), -20f);
    }

    void WannaScavenge_FixedUpdate()
    {
        float distance = (scavengeTarget - Position).magnitude;

        Move(scavengeTarget);
        if (distance > USE_RADIUS) { return; }

        fsm.ChangeState(ScavengerStates.Scrapyard);
    }

    IEnumerator Scrapyard_Enter()
    {
        yield return new WaitForSeconds(ACTION_TIME * 2);

        Debug.Log("ITEM FOUND");
        Spawn();

        fsm.ChangeState(ScavengerStates.WannaReturn);
    }

    void WannaReturn_Enter()
    {
        scavengeTarget = new Vector2(Random.Range(-6f, 1f), 0f); // y was 1f
    }

    void WannaReturn_FixedUpdate()
    {
        float distance = (scavengeTarget - Position).magnitude;

        Move(scavengeTarget);

        if (distance > USE_RADIUS) { return; }

        fsm.ChangeState(ScavengerStates.Return);
    }

    IEnumerator Return_Enter()
    {
        yield return new WaitForSeconds(ACTION_TIME);

        Debug.Log("ITEM LEFT");
        Drop();

        fsm.ChangeState(ScavengerStates.WannaRest);
    }

    // Spawn wall to carry
    void Spawn()
    {
        GameObject wall = Instantiate(walls[Random.Range(0, walls.Length)], transform.position, transform.rotation) as GameObject;
        wall.transform.parent = transform;

        carriedWall = wall;
    }

    void Drop()
    {
        carriedWall.transform.parent = null;
        carriedWall = null;
    }
}

public enum ScavengerStates
{
    WannaRest,
    Bedroom,
    WannaEat,
    Pantry,
    WannaScavenge,
    Scrapyard,
    WannaReturn,
    Return,
}
