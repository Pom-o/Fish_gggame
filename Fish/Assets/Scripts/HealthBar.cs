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

    private float AvoidOverflow() { 
        toxicBar.value = Mathf.Clamp(toxicBar.value, 0, toxicBar.maxValue);
        healthBar.value = Mathf.Clamp(healthBar.value, 0, LimitedMaxHealth());
        return healthBar.value;
    }

    public float DecreaseMaxHealthByToxic(float decreased)
    {
        healthBar.value -= decreased;
        toxicBar.value += decreased;
        return AvoidOverflow();
    }

    public float DecreaseHealth(float decreased)
    {
        healthBar.value -= decreased;
        return AvoidOverflow();
    }


    public float DecreaseMaxHealthByPlastic(float decreased)
    {
        healthBar.value -= decreased;
        return AvoidOverflow();
    }

    public float SetHealth(float health)
    {
        healthBar.value = health;
        return AvoidOverflow();
    }
}
