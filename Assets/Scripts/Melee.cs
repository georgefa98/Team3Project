using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{

    public float chargeSpeed;

    Animator anim;

    private float power;
    
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update() {
        if(aiming) {
            power = Mathf.Clamp(power + Time.deltaTime * chargeSpeed, 0f, 1f);
            anim.SetFloat("Power", power);
        }
    }

    public override void Use() {
        anim.SetTrigger("Attack");
        power = 0;
    }

    public override void StartAiming() {
        aiming = true;
        anim.SetBool("Charging", true);
        power = 0f;
    }

    public override void StopAiming() {
        aiming = false;
        anim.SetBool("Charging", false);
        anim.ResetTrigger("Attack");
        power = 0f;
    }

}
