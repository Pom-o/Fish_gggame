using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeTransition : MonoBehaviour
{
    [SerializeField] private string nextScene = "";
    [SerializeField] private bool disableFadeInAnimation;

    // Start is called before the first frame update
    private void Start()
    {
        if(disableFadeInAnimation){
            Animator animator = gameObject.GetComponent<Animator>();
            animator.Play("FadeIn", 0, 1);
        }
    }

    void FadeOutFinished ()
    {
        SceneManager.LoadScene(nextScene);
    }
}
