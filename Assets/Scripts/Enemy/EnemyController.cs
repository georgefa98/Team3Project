using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    public GameObject attackPoint;
    public float attackDistance = 1.8f, chaseAfterAttackDistance = 2f, chaseDistance = 7f, patrolForThisTime = 15f, patrolRadiusMin = 20f,
        patrolRadiusMax = 60f, runSpeed = 4f, walkSpeed = 0.5f, waitBeforeAttack = 2f; 

    private EnemyAnimator enemyAnim;
    private EnemyAudio enemyAudio;
    private EnemyState enemyState;
    private NavMeshAgent navAgent;
    private Transform target;
    private float attackTimer, patrolTimer, currentChaseDistance;

    void Awake()
    {
        enemyAnim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

       enemyAudio = GetComponentInChildren<EnemyAudio>();

    }

    // Use this for initialization
    void Start()
    {

        enemyState = EnemyState.PATROL;

        patrolTimer = patrolForThisTime;

        // when the enemy first gets to the player
        // attack right away
        attackTimer = waitBeforeAttack;

        // memorize the value of chase distance
        // so that we can put it back
        currentChaseDistance = chaseDistance;

    }

    // Update is called once per frame
    void Update()
    {

        if (enemyState == EnemyState.PATROL)
        {
            Patrol();
        }

        if (enemyState == EnemyState.CHASE)
        {
            Chase();
        }

        if (enemyState == EnemyState.ATTACK)
        {
            Attack();
        }

    }

    void Patrol()
    {

        // tell nav agent that he can move
        //navAgent.isStopped = false;
        navAgent.speed = walkSpeed;

        // add to the patrol timer
        patrolTimer += Time.deltaTime;

        if (patrolTimer > patrolForThisTime)
        {

            SetNewRandomDestination();

            patrolTimer = 0f;

        }

        if (navAgent.velocity.sqrMagnitude > 0)
        {

            enemyAnim.Walk(true);

        }
        else
        {

            enemyAnim.Walk(false);

        }

        // test the distance between the player and the enemy
        if (Vector3.Distance(transform.position, target.position) <= chaseDistance)
        {

            enemyAnim.Walk(false);

            enemyState = EnemyState.CHASE;

            // play spotted audio
             enemyAudio.PlayScreamSound();

        }


    } // patrol

    void Chase()
    {

        // enable the agent to move again
        navAgent.isStopped = false;
        navAgent.speed = runSpeed;

        // set the player's position as the destination
        // because we are chasing(running towards) the player
        navAgent.SetDestination(target.position);

        if (navAgent.velocity.sqrMagnitude > 0)
        {

            enemyAnim.Run(true);

        }
        else
        {

            enemyAnim.Run(false);

        }

        // if the distance between enemy and player is less than attack distance
        if (Vector3.Distance(transform.position, target.position) <= attackDistance)
        {

            // stop the animations
            enemyAnim.Run(false);
            enemyAnim.Walk(false);
            enemyState = EnemyState.ATTACK;

            // reset the chase distance to previous
            if (chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }

        }
        else if (Vector3.Distance(transform.position, target.position) > chaseDistance)
        {
            // player run away from enemy

            // stop running
            enemyAnim.Run(false);

            enemyState = EnemyState.PATROL;

            // reset the patrol timer so that the function
            // can calculate the new patrol destination right away
            patrolTimer = patrolForThisTime;

            // reset the chase distance to previous
            if (chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }


        } // else

    } // chase

    void Attack()
    {

        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attackTimer += Time.deltaTime;

        if (attackTimer > waitBeforeAttack)
        {

            enemyAnim.Attack();

            attackTimer = 0f;

            // play attack sound
            enemyAudio.PlayAttackSound();

        }

        if (Vector3.Distance(transform.position, target.position) >
           attackDistance + chaseAfterAttackDistance)
        {

            enemyState = EnemyState.CHASE;

        }


    } // attack

    void SetNewRandomDestination()
    {

        float rand_Radius = Random.Range(patrolRadiusMin, patrolRadiusMax);

        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);

        navAgent.SetDestination(navHit.position);

    }

    void Turn_On_AttackPoint()
    {
        attackPoint.SetActive(true);
    }

    void Turn_Off_AttackPoint()
    {
        if (attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(false);
        }
    }

    public EnemyState Enemy_State
    {
        get; set;
    }

} // class


































