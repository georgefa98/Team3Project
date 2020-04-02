using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    RectTransform rectTransform;
    Image bar;
    Player player;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        bar = GetComponent<Image>(); 
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update() {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, player.Health/100f * 200f);
        bar.color = Color.Lerp(new Color(0.75f, 0f, 0f, 0.5f), new Color(1f, 1f, 1f, 0.5f), player.Health/100f);
    }
}
