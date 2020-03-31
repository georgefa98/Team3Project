using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item
{
    Light spotLight;

    void Start()
    {
        spotLight = GetComponentInChildren<Light>();
    }

    public override void Use() {
        spotLight.enabled = !spotLight.enabled;
    }
}
