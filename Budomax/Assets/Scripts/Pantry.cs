﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;

public class Pantry : MonoBehaviour, IPantry
{
    void Start()
    {
        Food = 0;
    }

    public int MaxHealth => throw new System.NotImplementedException();

    public int Health => throw new System.NotImplementedException();

    public Vector2 Position => gameObject.transform.position;

    public int MaxFood => 20;

    public int Food { get; private set; }

    public void Eat(int amount)
    {
        Food -= Mathf.Min(Food, amount);
    }

    public void Fill(int amount)
    {
        Food += Mathf.Min(MaxFood - Food, amount);
    }
}
