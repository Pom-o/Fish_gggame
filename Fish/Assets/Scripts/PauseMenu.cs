using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour

{
    //[SerializeField] GameObject pauseMenu;
    public GameObject letterForThisBottle;
    // public GameObject readUI;





public void SetLetterActive(){
letterForThisBottle.SetActive(true);
}
    



    // Update is called once per frame
    void Update()
    {
        
    }
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.tag == "Player")
    //     {
    //         Debug.Log("lalalala");
    //         readUI.SetActive(true);
    //         Pause();
            

    //     }
    // }
}
