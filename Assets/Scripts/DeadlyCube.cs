using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyCube : MonoBehaviour
{
    private float timeBtwShots;

    public GameObject projectile;
    public Transform player;
    public float retreatDistance, speed, startTimeBtwShots, stoppingDistance;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBtwShots = startTimeBtwShots;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (player.transform.position.x >= -17.56)
        {
            FollowRetreat();
            ShootProjectiles();
        }
    }

    void FollowRetreat()
    {
        
        if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, Random.Range(2f, 6f)*Time.deltaTime, player.transform.position.z), speed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, player.position) < stoppingDistance && Vector3.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector3.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, Random.Range(2f, 6f)*Time.deltaTime, player.transform.position.z), speed * Time.deltaTime);
        }
    }   // Enemy responds to player, retreating a safe distance away from the player*/

    void ShootProjectiles()
    {
        if (timeBtwShots <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
    /* public void OnTriggerStay(Collider coll) {
        if(coll.gameObject.tag == "Player") {
            Player player = coll.gameObject.GetComponent<Player>();
            player.TakeDamage(10f * Time.deltaTime);
        } */
    }

