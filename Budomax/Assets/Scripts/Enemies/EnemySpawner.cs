using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] prefabFlyingType;
    public GameObject[] prefabWalkingType;


    private void SpawnEnemy()
    {
        Instantiate(GetRandomEnemyPrefab(), GetRandomSpawnPoint(), Quaternion.identity, transform);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        var randomPoint = Random.onUnitSphere * HouseManager.instance.GetHouseMaxDistance();
        randomPoint.y = Mathf.Abs(randomPoint.y); // force top.y = Mathf.Abs(P.y);
        return randomPoint + HouseManager.instance.GetHouseCenterPoint();
    }


    private GameObject GetRandomEnemyPrefab()
    {
        EnemyType randomEnemyType = (EnemyType) Random.Range(0, 2);
        switch (randomEnemyType)
        {
            case EnemyType.Flying:
                return prefabFlyingType[Random.Range(0, prefabFlyingType.Length)];
            case EnemyType.Walking:
                return prefabFlyingType[Random.Range(0, prefabWalkingType.Length)];
            default:
                return prefabFlyingType[Random.Range(0, prefabFlyingType.Length)];
        }
    }
}
