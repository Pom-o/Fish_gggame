using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] prefabs;

    public float startDelay = 5f;
    public float spawnInterval = 1.5f;
    [SerializeField] float leftBound = -7.2f;
    [SerializeField] float rightBound = 7.2f;
    [SerializeField] float bottomBound = -4f;
    [SerializeField] float topBound = 2.2f;


    void Start()
    {
        InvokeRepeating("SpawnRandom", startDelay, spawnInterval);
    }

    void SpawnRandom() {
        int index = Random.Range(0, prefabs.Length);
        Vector3 spawnPos = new Vector3(7, Random.Range(bottomBound, topBound), 0);
        Instantiate(prefabs[index],
            spawnPos,
            prefabs[index].transform.rotation);
    }
}
