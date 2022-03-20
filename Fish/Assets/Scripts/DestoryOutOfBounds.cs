using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryOutOfBounds : MonoBehaviour
{
    float leftBound = -8f;
    float bottomBound = -6f;

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
