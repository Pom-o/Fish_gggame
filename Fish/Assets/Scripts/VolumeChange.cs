using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChange : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    void Start()
    {
        // if(!PlayerPrefs.HasKey("Volume")){
        //     PlayerPrefs.SetFloat("Volume", 1);
        //     Load();
        // }

        // else {
        //     Load();
        // }
        volumeSlider.value = 2;
        ChangeVolume();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        // Save();

    }

    // private void Load(){
    //     volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    // }

    // private void Save(){
    //     PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    // }
}


   