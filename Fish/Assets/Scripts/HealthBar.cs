using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Slider toxicBar;
    public float suppressByPlastic = 0;

    public void SetMaxHealth(float health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        toxicBar.maxValue = health;
        toxicBar.value = 0;
    }

    float LimitedMaxHealth() {
        return healthBar.maxValue - toxicBar.value;
    }

    public void DecreaseMaxHealthByToxic(float decreased)
    {
        healthBar.value -= decreased;
        toxicBar.value += decreased;
    }

    public void DecreaseMaxHealthByPlastic(float decreased)
    {
        healthBar.value -= decreased;
    }

    public void SetHealth(float health)
    {
        healthBar.value = Mathf.Min(health, LimitedMaxHealth());
    }

}
