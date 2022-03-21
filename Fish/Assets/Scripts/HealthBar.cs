using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Slider toxicBar;

    public GameObject plasticPrefab;
    private List<GameObject> plastics = new List<GameObject>();
    int suppressByPlastic = 0;
    float helathLimitPerPlastic = 6.6f;
    float plasticStartX = 455;
    float plasticStartY = -50;
    int maxPlastics = 15;
    float plasticWidth = 30;


    float CalculatePlasticOffsetX(int index) {
        return -toxicBar.value * 4.8f +  plasticStartX - plasticWidth * index;

    }

    void rerenderPlastics() { 
        foreach(GameObject obj in plastics) {
            Destroy(obj);
        }
        plastics.Clear();

        for(int i = 0; i < suppressByPlastic; i++) {
            var position = new Vector3(CalculatePlasticOffsetX(i), plasticStartY, 0);
            var plastic = Instantiate(plasticPrefab, position, plasticPrefab.transform.rotation);
            plastic.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            plastics.Add(plastic);
        }
    }

    public void SetMaxHealth(float health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        toxicBar.maxValue = health;
        toxicBar.value = 0;
    }

    float LimitedMaxHealth() {
        return healthBar.maxValue - toxicBar.value - suppressByPlastic * helathLimitPerPlastic;
    }

    private float AfterHooks() { 
        toxicBar.value = Mathf.Clamp(toxicBar.value, 0, toxicBar.maxValue - suppressByPlastic * helathLimitPerPlastic);
        healthBar.value = Mathf.Clamp(healthBar.value, 0, LimitedMaxHealth());
        SyncToGlobal();
        if (healthBar.value <= 0) { 
           SceneManager.LoadScene("FinalCut");
        }
        return healthBar.value;
    }

    private void SyncToGlobal() {
        GlobalData.Instance.plasticEated = suppressByPlastic;
        GlobalData.Instance.toxicAccumulated = toxicBar.value;
    }

    public float DecreaseMaxHealthByToxic(float decreased)
    {
        healthBar.value -= decreased;
        toxicBar.value += decreased;
        rerenderPlastics();
        return AfterHooks();
    }

    public float DecreaseHealth(float decreased)
    {
        healthBar.value -= decreased;
        return AfterHooks();
    }


    public float DecreaseMaxHealthByPlastic()
    {
        if (suppressByPlastic < maxPlastics && LimitedMaxHealth() > 0)
        {
            suppressByPlastic += 1;
            rerenderPlastics();
        }
        return AfterHooks();
    }

    public float SetHealth(float health)
    {
        healthBar.value = health;
        return AfterHooks();
    }
}
