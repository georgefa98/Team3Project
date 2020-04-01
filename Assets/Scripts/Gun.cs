using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item
{
    public float cooldown;
    public int ammo;
    public int capacity;
	public int damage = 50; 	// Damage done to target
	public int shootRange = 100; //shooting range of gun

    GameObject meshObject;
    Animator animator;

    AudioSource shot;
    AudioSource dry;
    AudioSource reload;

    float cooldownTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        meshObject = transform.GetChild(0).gameObject;
        animator = meshObject.GetComponent<Animator>();

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
                    Debug.Log(rch.collider.gameObject.name);
					
                    GameObject hitMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    hitMarker.transform.position = rch.point;
                    hitMarker.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
					
					//Decrease health
					int enemy_health = rch.collider.gameObject.transform.root.GetComponent<Enemy>().getHealth();
					enemy_health = enemy_health - damage;
					rch.collider.gameObject.transform.root.GetComponent<Enemy>().setHealth(enemy_health);
					// Add blood splatter here
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
}
