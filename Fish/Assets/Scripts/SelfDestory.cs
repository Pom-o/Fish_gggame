using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestory : MonoBehaviour
{
    [SerializeField] float timeDisappear = 5f;

    void Start()
    {
        StartCoroutine(Destroy());
    }

    void Update()
    {
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(timeDisappear);
        Destroy(this.gameObject);
    }
}
