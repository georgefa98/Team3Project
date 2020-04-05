using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    public void OnTriggerStay(Collider coll) {
        
        Debug.Log("Object" + coll.transform.tag);
        if(coll.transform.tag == "Enemy") {
            Enemy enemy = coll.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(10f);
            Debug.Log("Hit!");
        }
    }
}
