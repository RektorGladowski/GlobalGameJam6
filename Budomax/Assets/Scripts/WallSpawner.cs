using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject[] wallPrefabs;

    float timer = 0f;
    readonly float spawnInterval = 2f;


    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (timer > spawnInterval)
            TryToSpawnNewWall();
       
    }

    void TryToSpawnNewWall()
    {
        timer = 0f;

        if (Physics2D.OverlapCircle(transform.position, 1f) == null)
        {
            GameObject wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)], transform.position, transform.rotation) as GameObject;
        }
    }
}
