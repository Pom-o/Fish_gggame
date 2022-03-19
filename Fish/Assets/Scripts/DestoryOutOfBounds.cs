using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryOutOfBounds : MonoBehaviour
{
    [SerializeField] float leftBound = -15;
    [SerializeField] float bottomBound = -6f;
    //[SerializeField] float rightBound = 10;
    //[SerializeField] float topBound = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < bottomBound || transform.position.x < leftBound) {
            Destroy(gameObject);
        }
        
    }
}
