using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour
{
    public bool isCannibal, isDestroyableBullet;
    public float health = 100f;

    [SerializeField] private float damageMultiplier;

    private EnemyAnimator enemyAnim;
    private EnemyAudio enemyAudio;
    private EnemyController enemyController;
    private NavMeshAgent navAgent;

    void Awake()
    {
        if (isDestroyableBullet)
        {    
            navAgent = GetComponent<NavMeshAgent>();
        }

        if (isCannibal)
        {
            enemyAnim = GetComponent<EnemyAnimator>();
            enemyController = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            // get enemy audio
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }

    }

    public void ApplyDamage(float damage)
    {       
        health -= damage * damageMultiplier;

        if (isCannibal)
        {
            if (enemyController.Enemy_State == EnemyState.PATROL)
            {
                enemyController.chaseDistance = 50f;
            }
        }

        if (health <= 0f)
        {
            Died();
        }

    } // apply damage

    void Died()
    {
        if (isCannibal)
        {
            //print("stupid cannibal is dead"); // debug
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 5f);

            enemyController.enabled = false;
            navAgent.enabled = false;
            enemyAnim.enabled = false;

            StartCoroutine(DeadSound());
        }

        if (isDestroyableBullet)
        {
            navAgent.velocity = Vector3.zero;

            gameObject.SetActive(false);

        }

    }  // died


    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.PlayDeadSound();
    }

}   // class









































