using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;

public class RoomBehaviour : MonoBehaviour, IRoom
{
    void Awake()
    {
        Workers = 0;
        MaxWorkers = 3;
        //type = Building.Kitchen;
    }

    public List<GameObject> WallObjects;

    // For the editor
    public RoomTypeSelection type;

    public RoomTypeSelection Type { get => type; set { type = value; } }

    //public IEnumerable<IWall> Walls => throw new System.NotImplementedException();

    public int MaxWorkers { get; set; }

    public int Workers { get; set; }

    public Vector2 Position => gameObject.transform.position;

    public void Enter()
    {
        if (Workers == MaxWorkers) { return; }
        Workers += 1;
    }

    public void Leave()
    {
        if (Workers == 0) { return; }
        Workers -= 1;
    }
}
