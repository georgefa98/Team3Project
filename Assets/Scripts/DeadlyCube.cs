using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyCube : MonoBehaviour
{
    public void OnTriggerStay(Collider coll) {
        if(coll.gameObject.tag == "Player") {
            Player player = coll.gameObject.GetComponent<Player>();
            player.TakeDamage(10f * Time.deltaTime);
        }
    }
}
