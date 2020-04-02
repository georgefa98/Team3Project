using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{

    public float damage;
    protected bool aiming;

    public virtual void StartAiming() {

    }

    public virtual void StopAiming() {

    }

    public virtual float GetCharge() {
        return 0f;
    }

}
