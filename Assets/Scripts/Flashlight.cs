using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item
{
    Light light;

    void Start()
    {
        light = GetComponentInChildren<Light>();
    }

    public override void Use() {
        light.enabled = !light.enabled;
    }
}
