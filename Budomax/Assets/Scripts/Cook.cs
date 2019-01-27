using UnityEngine;
using System.Collections;
using State;

public class Cook : MonoBehaviour, IBot
{
    const float TICK_INTERVAL = 1f;
    const float FOOD_WEIGHT = 0.5f;
    const float DISTANCE_WEIGHT = 0.5f;
    const float USE_RADIUS = 0.25f;
    const int COOKING_SKILL = 20;
    const float MOVEMENT_FORCE = 2f;

    public GameObject HomeObject;
    float timeElapsed = 0f;

    IHome home;
    Rigidbody2D rb;
    Vector2 target;
    int maxSatiation = 100;
    int satiation;
    int carriedFood = 0;

    // Use this for initialization
    void Start()
    {
        satiation = maxSatiation;
        target = gameObject.transform.position;
        rb = gameObject.GetComponent<Rigidbody2D>();
        home = HomeObject.GetComponent<Home>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Satiation " + satiation + " / " + maxSatiation + " Carried food " + carriedFood);

        Animate(target);

        timeElapsed += Time.fixedDeltaTime;

        if (timeElapsed < TICK_INTERVAL) { return; }

        timeElapsed = 0f;

        // Hunger
        if (satiation > 0)
        {
            satiation -= 1;
        }

        // Behaviour
        if (satiation == 0)
        {
            //Debug.Log("Searching food.");
            // Find pantry
            IPantry bestPantry = null;
            float bestScore = 0f;

            foreach (IWall wall in home.Walls)
            {
                if (wall is IPantry pantry)
                {
                    int food = pantry.Food;
                    float distance = (wall.Position - Position).magnitude;
                    float score = ScorePantry(food, distance);

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

            // Eat
            if (bestPantry.Food == 0) { return; }

            int meal = (int)Mathf.Min(bestPantry.Food, maxSatiation - satiation);
            bestPantry.Eat(meal);
            satiation += meal;
        }
        else if (carriedFood == 0)
        {
            //Debug.Log("Searching kitchen.");
            // Find kitchen
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

            // Go towards kitchen
            if (bestKitchen == null) { return; }

            Move(bestKitchen.Position);

            // Use kitchen
            if (bestDistance > USE_RADIUS) { return; }

            carriedFood += COOKING_SKILL;
        }
        else
        {
            //Debug.Log("Searching pantry.");
            // Find pantry
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
                        bestDistance  = distance;
                    }
                }
            }

            // Go towards pantry
            if (bestPantry == null) { return; }

            Move(bestPantry.Position);

            // Fill pantry
            if (bestDistance > USE_RADIUS) { return; }

            int space = bestPantry.MaxFood - bestPantry.Food;
            int filled = Mathf.Min(space, carriedFood);

            bestPantry.Fill(filled);
            carriedFood -= filled;
        }
    }

    public void Animate(Vector2 target)
    {
        Vector2 direction = target - Position;

        if (direction.magnitude < USE_RADIUS / 2f) { return; }

        direction.Normalize();
        rb.AddForce(direction * MOVEMENT_FORCE);
    }

    float ScorePantry(int food, float distance)
    {
        return food * FOOD_WEIGHT + distance * DISTANCE_WEIGHT;
    }

    public Vector2 Position
    {
        get
        {
            return gameObject.transform.position;
        }
    }

    public void Move(Vector2 target)
    {
        this.target = target;
        Debug.DrawLine(Position, target, Color.white, TICK_INTERVAL);
    }
}
