using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public GameObject creature;
    public float spawnTime = 30f;

    private Rigidbody projectile;
    private Transform player;
    private Vector3 target;


    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        projectile = GetComponent<Rigidbody>();
        // target = new Vector3(player.position.x, player.position.y);

        projectile.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        Path();
    }


    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    void Mutate()
    {
        SpawnCreature();
        DestroyProjectile();

    }

    void Path()
    {
        if ((Vector3.Distance(transform.position, player.position) > 3.5f) && (transform.position.y > 0.5f))
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            projectile.useGravity = true;
            Invoke("Mutate", spawnTime);
        }
    }

    void SpawnCreature()
    {
        Instantiate(creature, transform.position, Quaternion.identity);
        // Debug.Log("Object created");

    }

}


