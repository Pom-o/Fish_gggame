using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private Animator animator = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
{
    animator.SetTrigger("FadeOut");
}
    }
}
