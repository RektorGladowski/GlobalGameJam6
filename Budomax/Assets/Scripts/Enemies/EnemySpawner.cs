using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] prefabFlyingType;
    public GameObject[] prefabWalkingType;

    public float prewarmTime = 10f;
    public float currentTime = 1f;
    public float lastSpawnTime = 0f;

    private float baseMonsters = 30;
    private float cooldown = 30;

    private bool canSpawn = false;

    private void Start()
    {
        canSpawn = true;
        currentTime = Time.time;

        if(TutorialManager.instance != null)
        {
            if (TutorialManager.instance.IsRunning())
            {
                TutorialManager.EnemySpawnerReadyToGo += OnEnemySPawnerGo;
            }
            else
            {
                OnEnemySPawnerGo();
            }
        }
    }

    private void OnEnemySPawnerGo()
    {
        if (Time.time - currentTime > prewarmTime)
        {
            canSpawn = true;
        }
    }

    private void Update()
    {
        // calculate equation
        cooldown = baseMonsters - 5 * (Time.time / baseMonsters);
        cooldown = Mathf.Max(cooldown, 5);

        Debug.Log(cooldown);

        if (Time.time - lastSpawnTime > cooldown && canSpawn)
        {
            SpawnEnemy();
        }

    }


    private void SpawnEnemy()
    {
        lastSpawnTime = Time.time;
        Instantiate(GetRandomEnemyPrefab(), GetRandomSpawnPoint(), Quaternion.identity, transform);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        var randomPoint = UnityEngine.Random.onUnitSphere * HouseManager.instance.GetHouseMaxDistance();
        randomPoint.y = Mathf.Abs(randomPoint.y); // force top.y = Mathf.Abs(P.y);
        return randomPoint + HouseManager.instance.GetHouseCenterPoint();
    }


    private GameObject GetRandomEnemyPrefab()
    {
        EnemyType randomEnemyType = (EnemyType)UnityEngine.Random.Range(0, 2);
        switch (randomEnemyType)
        {
            default: return prefabFlyingType[UnityEngine.Random.Range(0, prefabFlyingType.Length)];
        }
    }
}
