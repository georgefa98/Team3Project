using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    protected float health;
    protected bool alive;
    protected bool vulnerable;
    
    public float Health {
        get { return health; }
    }

    public void TakeDamage(float damage) {
        health = Mathf.Clamp(health - damage, -1f, 100);
        if(health <= 0) {
            StartCoroutine(Die());
        }
    }

    public virtual IEnumerator Die() {
        alive = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}
