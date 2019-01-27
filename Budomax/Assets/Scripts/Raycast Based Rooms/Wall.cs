using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IDamageable
{
    public StructureConfig wallConfig;

    public List<Wall> neighbours = new List<Wall>();
    public Rigidbody2D rigidbody2 { get; private set; }
    public new PolygonCollider2D collider { get; private set; }

    public int Health => (int)currentHP;

    float currentHP;

    void Awake()
    {
        // Get Components
        rigidbody2 = GetComponent<Rigidbody2D>();
        collider = GetComponent<PolygonCollider2D>();

        // Process config info
        rigidbody2.gravityScale = wallConfig.structureGravityMultiplier;
        rigidbody2.bodyType = wallConfig.defaultRBType;
        collider.isTrigger = wallConfig.triggerByDefault;
        currentHP = wallConfig.structureHP;
    }  

    public void Drop()
    {
        collider.isTrigger = true;
        rigidbody2.bodyType = RigidbodyType2D.Dynamic;
    }

    public bool TryAttaching()
    {
        return false;
    }

    public bool Attach(List<Collider2D> neighbourWalls)
    {
        for (int nId = 0; nId < neighbourWalls.Count; nId++)
        {
            Wall tempWall = neighbourWalls[nId].GetComponentInParent<Wall>();
            neighbours.Add(tempWall);
            tempWall.neighbours.Add(this);
        }

        collider.isTrigger = false;
        rigidbody2.bodyType = RigidbodyType2D.Static;

        GetComponent<WallRoomChecker>()?.CheckForPossibleRooms();

        return true;
    }



    public void Damage(int damage)
    {
        if (wallConfig.damageable)
        {
            Debug.Log("Wall damaged");
            currentHP -= damage;

            if (currentHP < 0f)
                Drop();
        }
    }

}
