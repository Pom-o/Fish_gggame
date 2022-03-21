using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string entryScene = "1LoggingIn";
    public string tutorialScene = "2Tutorial";
    public string gameScene = "SampleScene";
    public string endScene = "FinalCut";

    public void StartEntry()
    {
        SceneManager.LoadScene(entryScene);
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene(tutorialScene);
    }

    public void StartGameScene()
    {
        SceneManager.LoadScene(gameScene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void EndGame()
    {
        SceneManager.LoadScene(endScene);
    }

}
