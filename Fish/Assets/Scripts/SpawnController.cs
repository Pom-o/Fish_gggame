using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyPrefab;
    private int enemiesToSpawn;

    void Start()
    {

    }

    // Update is called once per frame
    void SpawnEnemyWave()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemyPrefab, new Vector2(0, 0), enemyPrefab.transform.rotation);
        }
    }

    void GenerateSpawnPosition()
    {

    }
}
