using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    private Transform player;
    private Vector3 target;


    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector3(player.position.x, player.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

      
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Player player = coll.gameObject.GetComponent<Player>();
            player.TakeDamage(325f * Time.deltaTime);
            // DestroyProjectile();
        }

        /* void DestroyProjectile()
        {
            Destroy(gameObject);
        } */

    }

}


