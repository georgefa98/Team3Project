using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
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
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, player.Energy/100f * 200f);
        bar.color = Color.Lerp(new Color(0.6f, 1f, 0.7f, 0.5f), new Color(0.6f, 1f, 0.7f, 0.5f), player.Energy/100f);
    }
}
