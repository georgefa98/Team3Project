using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{

    public float chargeSpeed;
    public GameObject swingAudio;
    public float minPitch;
    public float maxPitch;
    public float minVolume;
    public float maxVolume;

    private Animator anim;
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

        /* play swing audio */
        AudioSource swingAudioSource = Instantiate(swingAudio).GetComponent<AudioSource>();
        swingAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, power);
        swingAudioSource.volume = Mathf.Lerp(minVolume, maxVolume, power);

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

    public override float GetCharge() {
        return power;
    }

}
