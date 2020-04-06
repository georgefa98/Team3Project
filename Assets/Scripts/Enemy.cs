using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    NavMeshAgent agent;
    Transform player;
    Animator animator;
	private static float health = 100f;
	public float attack_power = 10f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
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
	
	public void setHealth(float newHealth)
	{
		health = newHealth;
	}
	
	public float getHealth()
	{
		return health;
	}
	
	void Die()
	{
		//Death animation here
		
		Destroy(gameObject);
	}
}
