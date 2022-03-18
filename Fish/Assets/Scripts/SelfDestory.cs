using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestory : MonoBehaviour
{
    [SerializeField] float timeDisappear = 5f;
    [SerializeField] bool destoryEnable = true;

    void Start()
    {
        StartCoroutine(Destroy());
    }

    void Update()
    {
    }

    public void disable() {
        destoryEnable = false;
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(timeDisappear);
        if (destoryEnable)
        {
            Destroy(this.gameObject);
        }
    }
}
