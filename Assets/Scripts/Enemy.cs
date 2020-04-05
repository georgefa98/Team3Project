using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Mob
{
    
    public GameObject splatter;

    NavMeshAgent agent;
    Transform player;
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();

        vulnerable = true;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetFloat("Speed", agent.velocity.magnitude);
        if(Vector3.Distance(transform.position, player.position) > 2f)
            agent.SetDestination(player.position);
        else
            agent.SetDestination(transform.position);
		
		if (health <= 0)
			Die();
    }

    public void TakeBulletDamage(float damage, Vector3 hitPoint, Vector3 direction) {
        Instantiate(splatter, hitPoint, Quaternion.FromToRotation(Vector3.forward, direction));

        this.TakeDamage(damage);
    }
	
	public override IEnumerator Die()
	{
		//Death animation here
		
        yield return new WaitForSeconds(2f);

		Destroy(gameObject);
	}
}
