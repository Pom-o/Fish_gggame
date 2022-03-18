using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovable : MonoBehaviour
{
    public float speed = 3;
    //movement
    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
