using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject[] foodPrefabs;
    public GameObject[] plasticPrefabs;
    public GameObject[] fishnetRefabs;
    public GameObject[] electricShockerRefabs;
    public GameObject[] toxicAreaRefabs;
    public GameObject[] hookRefabs;

    enum Tag { Food, Plastic, Fishnet, ElectricShocker , ToxicArea, Hook};
    Dictionary<Tag, int> EnemyGenerateRate = new Dictionary<Tag, int> {
            {Tag.Food, 10 },
            {Tag.Plastic, 5 },
            {Tag.Fishnet, 3},
            {Tag.ElectricShocker, 3 },
            {Tag.ToxicArea, 3 },
            {Tag.Hook, 3 }
    };
    List<Tag> EnemyGenerateSample = new List<Tag>();
    Dictionary<Tag, GameObject[]> tag2Fabs; 

    public float startDelay = 5f;
    public float spawnInterval = 2f;
    float leftBound = -5.2f;
    float rightBound = 4.8f;
    float bottomBound = -4f;
    float topBound = 2.2f;


    // Start is called before the first frame update
    void Start()
    {
        tag2Fabs = new Dictionary<Tag, GameObject[]> {
            {Tag.Food, foodPrefabs },
            {Tag.Plastic, plasticPrefabs },
            { Tag.Fishnet, fishnetRefabs},
            {Tag.ElectricShocker, electricShockerRefabs },
            {Tag.ToxicArea, toxicAreaRefabs },
            {Tag.Hook, hookRefabs }
           };
        InitEnemiesGenerateSample();
        InvokeRepeating("SpawnRandom", startDelay, spawnInterval);
    }

    void InitEnemiesGenerateSample() { 
        foreach(var e in EnemyGenerateRate) { 
            for (int i = 0; i < e.Value; i++) {
                EnemyGenerateSample.Add(e.Key);
            }
        }
    }

    void SpawnRandom() {
        Tag tag = EnemyGenerateSample[Random.Range(0, EnemyGenerateSample.Count)];

        var prefabs = tag2Fabs[tag];
        var index = Random.Range(0, prefabs.Length);
        var prefab = prefabs[index];
        Instantiate(prefab,
            GetSpawnPos(prefab),
            prefab.transform.rotation);
    }

    Vector3 GetSpawnPos(GameObject gameObject) {
        Debug.Log($"Spawn {gameObject.tag}");
        switch(gameObject.tag) {
            case "DeadFish":
                return new Vector3(rightBound + 5, 2.7f, 0);
            case "Hook":
                return new Vector3(rightBound, 2f, 0);
            case "Fishnet":
                return new Vector3(rightBound, Random.Range(bottomBound, 1.9f), 0);
            default:
                return new Vector3(8, Random.Range(bottomBound, topBound), 0);

        }
    }
}
