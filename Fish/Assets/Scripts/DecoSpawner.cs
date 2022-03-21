using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoSpawner : MonoBehaviour
{
    public List<GameObject>platforms = new List<GameObject>();
    // Start is called before the first frame update
    public float spawnTime;
    private float countTime;
    private Vector3 spawnPosition;


    // Update is called once per frame
    void Update()
    {
        SpawnPosition();
    }


    public void SpawnPosition(){
        countTime += Time.deltaTime;
        spawnPosition = transform.position;
        spawnPosition.x = Random.Range(10f, 20f);
        spawnPosition.y = Random.Range(3.5f, 4.5f);

        if (countTime >= spawnTime)
        {
            CreatePlatform();
            countTime = 0;
        }
    }

    public void CreatePlatform(){
        int index = Random.Range(0, platforms.Count);
        Instantiate(platforms[index],spawnPosition, Quaternion.identity);

    }

}
