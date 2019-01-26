using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;

public class Wall : MonoBehaviour, IWall
{
    public int MaxHealth => throw new System.NotImplementedException();

    public int Health => throw new System.NotImplementedException();

    public Vector2 Position => gameObject.transform.position;
}
