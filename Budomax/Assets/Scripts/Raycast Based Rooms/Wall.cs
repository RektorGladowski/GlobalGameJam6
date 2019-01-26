using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IDamageable
{
    public StructureConfig wallConfig;

   
    public List<Wall> neighbours = new List<Wall>();
    Rigidbody2D wallRb;
    Collider2D wallCollider;
    float currentHP;



    void Awake()
    {
        // Get Components
        wallRb = GetComponent<Rigidbody2D>();
        wallCollider = GetComponent<Collider2D>();

        // Process config info
        wallRb.gravityScale = wallConfig.structureGravityMultiplier;
        wallRb.bodyType = wallConfig.defaultRBType;
        wallCollider.isTrigger = wallConfig.triggerByDefault;
        currentHP = wallConfig.structureHP;
    }  



    public void DropWall()
    {
        wallCollider.isTrigger = true;
        wallRb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void PlaceWall(List<GameObject> neighbourWalls)
    {
        for (int nId = 0; nId < neighbourWalls.Count; nId++)
        {
            Wall tempWall = neighbourWalls[nId].GetComponentInParent<Wall>();
            neighbours.Add(tempWall);
            tempWall.neighbours.Add(this);
        }

        wallCollider.isTrigger = false;
        wallRb.bodyType = RigidbodyType2D.Static;

        GetComponent<WallRoomChecker>()?.CheckForPossibleRooms();
    }



    public void TakeDamage(float damage)
    {
        if (wallConfig.damageable)
        {
            Debug.Log("Wall damaged");
            currentHP -= damage;

            if (currentHP < 0f)
                DropWall();
        }
    }
}
