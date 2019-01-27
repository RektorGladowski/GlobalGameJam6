using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;

public class Turret : MonoBehaviour, ITurret
{
    void Start()
    {
        isEmpty = false;
        health = MaxHealth;
    }

    [SerializeField]
    bool isEmpty;
    [SerializeField]
    int health;

    public int MaxHealth => 20;

    public int Health { get => health; set { health = value; } }

    public Vector2 Position => gameObject.transform.position;

    public bool IsEmpty { get => isEmpty; private set { isEmpty = value; } }

    public void Enter()
    {
        IsEmpty = false;
    }

    public void Leave()
    {
        IsEmpty = true;
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
