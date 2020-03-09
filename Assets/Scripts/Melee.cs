using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Item
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Use() {
        anim.SetTrigger("Attack");
    }

}
