using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour {
    public float scrollSpeed = 5;
    public GameObject cam; 
    private float length, startpos;
    


    void Start () {
         startpos = transform.position.x;
         transform.position = new Vector3(1, 0, 0);
         Vector3 newPosition = transform.position;
         newPosition.x = 1;
         transform.position = newPosition; 
         length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    void Update () {
	transform.position -= new Vector3(1 * scrollSpeed * Time.deltaTime, 0, 0);

        if (cam.transform.position.x > startpos + length) startpos += length;
        else if (cam.transform.position.x < startpos - length) startpos -=length;
    }
}
