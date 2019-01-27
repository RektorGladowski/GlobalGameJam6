using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;

public class Pantry : MonoBehaviour, IPantry, IDamageable
{
    void Start()
    {
        food = 0;
        health = MaxHealth;
    }

    bool isDestroyed;

    [SerializeField]
    int health;

    public int MaxHealth => 20;

    public int Health { get => health; set { health = value; } }

    public Vector2 Position => gameObject.transform.position;

    public int MaxFood => 100;

    [SerializeField]
    int food;

    public int Food { get => food; private set { food = value; } }

    public void Eat(int amount)
    {
        Food -= Mathf.Min(Food, amount);
    }

    public void Fill(int amount)
    {
        Food += Mathf.Min(MaxFood - Food, amount);
    }

    public void Damage(int damage)
    {
        Health -= damage;
        //if (Health <= 0)
        //{
        //    isDestroyed = true;
        //}
    }
}
