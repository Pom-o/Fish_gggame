using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] prefabs;

    public float startDelay = 5f;
    public float spawnInterval = 3f;
    float leftBound = -5.2f;
    float rightBound = 7.2f;
    float bottomBound = -4f;
    float topBound = 2.2f;


    void Start()
    {
        InvokeRepeating("SpawnRandom", startDelay, spawnInterval);
    }

    void SpawnRandom() {
        int index = Random.Range(0, prefabs.Length - 1);
        Vector3 spawnPos = new Vector3(8, Random.Range(bottomBound, topBound), 0);
        Instantiate(prefabs[index],
            spawnPos,
            prefabs[index].transform.rotation);
    }
}
