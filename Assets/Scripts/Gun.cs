using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public float cooldown;
    public int ammo;
    public int capacity;
	public int shootRange = 100; //shooting range of gun
    public float aimSpeed;

    GameObject meshObject;
    GameObject hand;
    Animator animator;
    public Vector3 aimedPosition = new Vector3(0f, -0.1f, 0.3f);
    Vector3 unaimedPosition;

    AudioSource shot;
    AudioSource dry;
    AudioSource reload;

    private float cooldownTimer;
    private float centeredness;
    
    // Start is called before the first frame update
    void Start()
    {
        meshObject = transform.GetChild(0).gameObject;
        animator = meshObject.GetComponent<Animator>();
        hand = transform.parent.gameObject;
        unaimedPosition = hand.transform.localPosition;

        AudioSource[] audioSources = meshObject.GetComponents<AudioSource>();
        shot = audioSources[0];
        reload = audioSources[1];
        dry = audioSources[2];
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldownTimer > 0f) {
            cooldownTimer -= Time.deltaTime;
        }

        if(aiming) {
            centeredness = Mathf.Clamp(centeredness + Time.deltaTime * aimSpeed, 0f, 1f);
        } else {
            centeredness = Mathf.Clamp(centeredness - Time.deltaTime * aimSpeed, 0f, 1f);
        }

        hand.transform.localPosition = Vector3.Lerp(unaimedPosition, aimedPosition, centeredness);
    }

    public override void Use() {
        if(cooldownTimer <= 0f && ammo > 0) {
            animator.SetTrigger("Shoot");
            shot.Play();
            cooldownTimer = cooldown;
            ammo -= 1;

            RaycastHit[] raycastHits =  Physics.RaycastAll(transform.position, transform.forward);
            foreach(RaycastHit rch in raycastHits) {
                if(rch.collider.gameObject.tag == "EnemyBody") {
					
					//Decrease health
					Enemy enemy = rch.collider.gameObject.transform.root.GetComponent<Enemy>();
                    enemy.TakeBulletDamage(damage, rch.point, transform.forward);

                    break;
                }
            }
            

        } else if(ammo <= 0) {
            animator.SetTrigger("Dry");
            dry.Play();
        }
    }

    public override void Refill() {
        ammo = capacity;
        reload.Play();
        animator.SetTrigger("Reload");
        animator.ResetTrigger("Dry");
    }

    public override void StartAiming() {
        aiming = true;
        centeredness = 0f;
    }

    public override void StopAiming() {
        aiming = false;
    }

    public override float GetCharge() {
        return centeredness;
    }
}
