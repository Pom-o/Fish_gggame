using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float speed;
    private Rigidbody2D enemyRb;
    private GameObject player;
    //private float spawnRange = 9;
    //spawn should be around the player


    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector2 lookDirection = (player.transform.position - transform.position);
        enemyRb.AddForce(lookDirection.normalized * speed);

    }
    /*
    private Vector2 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(player.transform.position.x - spawnRange, player.transform.position.x);
        float spawnPosY = Random.Range(player.transform.position.y - spawnRange, player.transform.position.y + spawnRange);
        Vector2 randomPos = new Vector2(spawnPosX, spawnPosY);
        return randomPos;
    }
    */

}
