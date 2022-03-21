using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    public GameObject fishBar;
    public GameObject humanBar;

    public float eatDelay = 2.0f;

    public int testPlasticEated = 3;
    public float testToxicAccumulated = 20.5f;
    public float toxicTransferSpeed = 5;
    public float plasticTransferSpeed = 2;

    int plasticEated = 0;
    float toxicAccumulated = 0;
    float humanToxicAccumulated = 0;
    float humanPlasticEated = 0;

    bool isEating = false;
    // Start is called before the first frame update
    void Start()
    {
        toxicAccumulated = GlobalData.Instance != null ? GlobalData.Instance.toxicAccumulated : testToxicAccumulated;
        plasticEated = GlobalData.Instance != null ? GlobalData.Instance.plasticEated : testPlasticEated;
        Debug.Log($"plastic: {plasticEated} toxic: {toxicAccumulated}");

        ReinitBar(fishBar);
        //ReinitBar(humanBar);
        Invoke("StartEatFish", eatDelay);
    }

    // Update is called once per frame
    void Update()
    {
          var fish = fishBar.GetComponent<HealthBar>();
          var human = humanBar.GetComponent<HealthBar>();
        if (isEating) {
            if (humanToxicAccumulated < toxicAccumulated)
            {
                humanToxicAccumulated += toxicTransferSpeed * Time.deltaTime;
                fish.SetToxic(toxicAccumulated - humanToxicAccumulated);
                human.SetToxic(humanToxicAccumulated);
            }
            else
            {
                if (humanPlasticEated < plasticEated)
                {
                    humanPlasticEated += plasticTransferSpeed * Time.deltaTime;
                    fish.SetPlasticNumber((int)(plasticEated + 1 - humanPlasticEated));
                    human.SetPlasticNumber((int)humanPlasticEated);
                }
            }
        }
        
    }

    void StartEatFish() {
        isEating = true;
    }


    public void ReinitBar(GameObject healthBar)
    {
        var bar = healthBar.GetComponent<HealthBar>();
        bar.DecreaseMaxHealthByToxic(toxicAccumulated);
        for (int i = 0; i < plasticEated; i++)
        {
            bar.DecreaseMaxHealthByPlastic();
        }
        bar.SetHealth(0);
    }

}
