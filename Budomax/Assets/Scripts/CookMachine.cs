using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using State;
using Hellmade.Sound;

public class CookMachine : MonoBehaviour
{
    const float FOOD_WEIGHT = 0.5f;
    const float DISTANCE_WEIGHT = 0.5f;
    const float USE_RADIUS = 0.25f;
    const int COOKING_SKILL = 20;
    const int ACTION_TIME = 4;
    const float MOVEMENT_FORCE = 1f;
    public AudioManager am;

    public GameObject HomeObject;
    IHome home; // TODO: Get from some singleton
    Rigidbody2D rb;
    StateMachine<States> fsm;
    IRoom reservedRoom;
    int carriedFood = 0;
    IPantry visitedPantry;

    void Awake()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        home = HouseManager.instance?.GetComponent<IHome>();// ?? HomeObject.GetComponent<IHome>();
        fsm = StateMachine<States>.Initialize(this);
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        fsm.ChangeState(States.WannaCook);
    }

    private void Update()
    {
        //Debug.Log("Carried food " + carriedFood);
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
    void WannaCook_FixedUpdate()
    {
        // Find best kitchen
        IRoom bestKitchen = null;
        float bestDistance = Mathf.Infinity;

        foreach (IRoom room in home.Rooms)
        {
            if (room.Type == RoomTypeSelection.Kitchen)
            {
                float distance = (room.Position - Position).magnitude;
                bool isAvailable = room.Workers < room.MaxWorkers;

                if (bestKitchen == null || (isAvailable && distance < bestDistance))
                {
                    bestKitchen = room;
                    bestDistance = distance;
                }
            }
        }

        if (bestKitchen == null) { return; }

        // Reserve a spot
        if (reservedRoom != bestKitchen)
        {
            if (reservedRoom != null) { CancelReservation(); }
            Reserve(bestKitchen);
        }

        // Walk
        Move(bestKitchen.Position);

        // Check if withing reach
        if (bestDistance > USE_RADIUS) { return; }

        fsm.ChangeState(States.Kitchen);
    }

    IEnumerator Kitchen_Enter()
    {
        carriedFood += COOKING_SKILL;

        am.playAudio("Eating_alt", 0.3f);
        yield return new WaitForSeconds(ACTION_TIME);


        fsm.ChangeState(States.WannaStock);
    }

    void WannaStock_FixedUpdate()
    {
        IPantry bestPantry = null;
        float bestDistance = 0f;

        foreach (IWall wall in home.Walls)
        {
            if (wall is IPantry pantry)
            {
                float distance = (wall.Position - Position).magnitude;
                bool isAvailable = pantry.Food < pantry.MaxFood;

                if (bestPantry == null || (isAvailable && distance < bestDistance))
                {
                    bestPantry = pantry;
                    bestDistance = distance;
                }
            }
        }

        // Go towards pantry
        if (bestPantry == null) { return; }

        Move(bestPantry.Position);

        // Check if withing reach
        if (bestDistance > USE_RADIUS) { return; }

        visitedPantry = bestPantry;

        fsm.ChangeState(States.Pantry);
    }

    IEnumerator Pantry_Enter()
    {
        int space = visitedPantry.MaxFood - visitedPantry.Food;
        int filled = Mathf.Min(space, carriedFood);
        am.playAudio("RestockFeeder", 0.3f);
        yield return new WaitForSeconds(ACTION_TIME);

        if (space == 0)
        {
            fsm.ChangeState(States.WannaStock);
            yield break;
        }

        visitedPantry.Fill(filled);
        carriedFood -= filled;


        fsm.ChangeState(States.WannaCook);
    }

    void Pantry_Exit()
    {
        visitedPantry = null;
    }
}

public enum States
{
    WannaCook,
    Kitchen,
    WannaStock,
    Pantry,
}
