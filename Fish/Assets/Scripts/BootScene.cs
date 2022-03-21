using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootScene : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator = null;

    void Start()
    {
        
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(10);
        fadeAnimator.SetTrigger("FadeOut");
    }
}