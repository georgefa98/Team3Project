using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Mob
{
    
    public GameObject splatter;
    public float enemyVision = 10f;

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
        float distance = Vector3.Distance(player.position, transform.position);
        animator.SetFloat("Speed", agent.velocity.magnitude);
        /*if(Vector3.Distance(transform.position, player.position) > 2f)
            agent.SetDestination(player.position);
        else
            agent.SetDestination(transform.position);*/
        if (distance <= enemyVision)
        {
            agent.SetDestination(player.position);
        }

		
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

    public void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Player player = coll.gameObject.GetComponent<Player>();
            player.TakeDamage(10);
        }
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyVision);
    }
}
