using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookable : MonoBehaviour
{
    public float damage = 5.0f;

    // Escape by pressing hookEscapeHitted in hookEscapeInterval seconds
    public float hookEscapeInterval = 5.0f;
    public int hookEscapePressTarget = 5;

    public float PressStrengthWeakeningRate() {
        return hookEscapePressTarget / hookEscapeInterval;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
