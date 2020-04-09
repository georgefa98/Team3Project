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
    private float dropPoint = 2.5f, playerYPosition = 0.5f;


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
<<<<<<< HEAD
        if ((Vector3.Distance(transform.position, player.position) > dropPoint) && (transform.position.y > playerYPosition))
=======
        if ((Vector3.Distance(transform.position, player.position) > 3.5f) && (transform.position.y > 0.5f))
>>>>>>> b2972463162f47b5e780857ff9c0950840071c04
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


