using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{

    public static EnemySpawnerController Instance;
    public List<EnemyController> EnemyTypes;
    public AudioSource EnemyExplosion;
    int LastIX = 0;

    public float StartSpawnRate = 5;
    public float SpawnRate;
    public float LastSpawn;

    void Start()
    {
        EnemySpawnerController.Instance = this;
    }

    public void Reset()
    {
        SpawnRate = StartSpawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.Player.lives <= 0) return;
        if ((LastSpawn + SpawnRate) < Time.time)
        {
            int ix = LastIX;
            while (ix == LastIX)
            {
                ix = Random.Range(0, EnemyTypes.Count);
            }
            LastIX = ix;
            EnemyController toClone = EnemyTypes[ix];
            EnemyController newEnemy = UnityEngine.Object.Instantiate<EnemyController>(toClone);
            newEnemy.gameObject.SetActive(true);
            newEnemy.InitFlightPlan();
            LastSpawn = Time.time;
        }

        float modifier = Mathf.Log10(1F + ((float)GameController.Instance.kills/((float)(StartSpawnRate)*10)))*((float)StartSpawnRate);
        SpawnRate = Mathf.Max(0.25F, (StartSpawnRate - modifier));
    }
}
