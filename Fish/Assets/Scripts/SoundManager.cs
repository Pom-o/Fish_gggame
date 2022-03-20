using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public AudioSource efxSource;
    public AudioSource musicSource;

    void Awake () {
        if (instance == null){
            instance = this;
        } else if(instance != this){
            Destroy (gameObject);

        }
        DontDestroyOnLoad (gameObject);
    }

    public void playSingle(AudioClip clip){
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void RandomizesFx(params AudioClip[] clips){
        int randomIndex = Random.Range(0, clips.Length);
        efxSource.clip = clips [randomIndex];
        efxSource.Play();
    }
}
