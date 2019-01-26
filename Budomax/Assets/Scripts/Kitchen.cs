using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;

public class Kitchen : MonoBehaviour, IRoom
{
    public List<GameObject> WallObjects;

    public Building Type => Building.Kitchen;

    public IEnumerable<IWall> Walls => throw new System.NotImplementedException();

    public int MaxWorkers => 3;

    public int Workers => 0;

    public Vector2 Position => gameObject.transform.position;

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Leave()
    {
        throw new System.NotImplementedException();
    }
}
