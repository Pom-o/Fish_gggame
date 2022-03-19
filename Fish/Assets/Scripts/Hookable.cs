using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookable : MonoBehaviour
{
    // Escape by pressing hookEscapeHitted in hookEscapeInterval seconds
    public float hookEscapeInterval = 15.0f;
    public int hookEscapePressTarget = 3;

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
