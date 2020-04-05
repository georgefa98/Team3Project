using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public float maxHealth = 100f;
    protected float health;
    protected bool alive;
    protected bool vulnerable;
    
    public float Health {
        get { return health; }
    }

    void Start() {
        health = maxHealth;
    }

    public void TakeDamage(float damage) {
        if (vulnerable)
        {
            health = Mathf.Clamp(health - damage, -1f, maxHealth);
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }


    public virtual IEnumerator Die() {
        alive = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}
