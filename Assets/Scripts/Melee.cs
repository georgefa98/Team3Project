﻿using System.Collections;
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
    private GameObject hitBox;
    public bool attacking;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        hitBox = transform.GetChild(1).gameObject;
    }

    void Update() {
        if(aiming) {
            power = Mathf.Clamp(power + Time.deltaTime * chargeSpeed, 0f, 1f);
            anim.SetFloat("Power", power);
        }

        if(attacking) {
            Collider[] colls = Physics.OverlapBox(transform.position, Vector3.one, Quaternion.identity);
            foreach(Collider coll in colls) {

                if(coll.gameObject.tag == "EnemyBody") {
                    Enemy enemy = coll.transform.parent.parent.parent.GetComponent<Enemy>();
                    enemy.TakeDamage(1f);
                }
            }
        }
    }

    public override void Use() {
        anim.SetTrigger("Attack");

        StartCoroutine(EnableHitbox());

        /* play swing audio */
        AudioSource swingAudioSource = Instantiate(swingAudio).GetComponent<AudioSource>();
        swingAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, power);
        swingAudioSource.volume = Mathf.Lerp(minVolume, maxVolume, power);

        power = 0;
    }

    public IEnumerator EnableHitbox() {

        attacking = true;

        yield return new WaitForSeconds(0.2f);

        attacking = false;

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
