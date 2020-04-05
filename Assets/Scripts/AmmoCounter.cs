using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{

    Text text;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.hand.childCount > 0) {
            GameObject obj = player.hand.GetChild(0).gameObject;
            Tool tool = obj.GetComponent<Tool>();

            if(tool.GetType() == typeof(Gun)) {
                Gun gun = (Gun)tool;
                text.text = "" + gun.ammo;

            } else {
                text.text = "";
            }
        }
    }
}
