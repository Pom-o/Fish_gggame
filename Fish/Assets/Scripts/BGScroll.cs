using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour {
    public float scrollSpeed = 5;


    void Start () {
         transform.position = new Vector3(1, 0, 0);
         Vector3 newPosition = transform.position;
         newPosition.x = 1;
         transform.position = newPosition; 
    }

    void Update () {
	transform.position -= new Vector3(1 * scrollSpeed * Time.deltaTime, 0, 0);
}

}
