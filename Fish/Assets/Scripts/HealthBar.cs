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
    public float plasticStartX = 455;
    public float plasticStartY = -50;
    public float plasticWidth = 30;
    public float toxicBarOffsetUnit = 4.8f;
    int maxPlastics = 15;
    public bool isFinalCut = false;
    private bool isGameOver = false;
    public Animator transition;

    float CalculatePlasticOffsetX(int index) {
        return -toxicBar.value * toxicBarOffsetUnit +  plasticStartX - plasticWidth * index;

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
        if (!isFinalCut && healthBar.value <= 0) { 
           StartCoroutine(LoadFinal());
           isGameOver = true; 
        }
        return healthBar.value;
    }

    private void SyncToGlobal() {
        if (!isFinalCut) { 
        GlobalData.Instance.plasticEated = suppressByPlastic;
        GlobalData.Instance.toxicAccumulated = toxicBar.value;
        }
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
        Debug.Log($"eatedPlastic: {suppressByPlastic}");
        return AfterHooks();
    }

    public void SetToxic(float toxic) {
        toxicBar.value = toxic;
        rerenderPlastics();
    }

    public void SetPlasticNumber(int plastic) {
        suppressByPlastic = plastic;
        rerenderPlastics();
    }

    public float SetHealth(float health)
    {
        healthBar.value = health;
        return AfterHooks();
    }


    IEnumerator LoadFinal(){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("FinalCut");

    }

}
