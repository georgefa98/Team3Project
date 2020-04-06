using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public LayerMask layerMask;
    public float damage=2f, radius=1f;

    // Update is called once per frame
    void Update()
    {
       
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (hits.Length > 0)
        {
            hits[0].gameObject.GetComponent<HealthScript>().ApplyDamage(damage);
            //print("We hit the " + hits[0].gameObject.tag); // debug
            gameObject.SetActive(false);
        }
    }
}
