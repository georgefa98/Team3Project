using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    GameObject infoPanel;
    public Item item;

    void Start() {
        infoPanel = GameObject.FindGameObjectWithTag("ItemInfo");
    }

    public void OnPointerEnter() {
        infoPanel.GetComponent<Image>().enabled = true;
        
        for(int i = 0; i < infoPanel.transform.childCount; i++) {
            infoPanel.transform.GetChild(i).gameObject.SetActive(true);
        }
        Text itemName = infoPanel.transform.GetChild(0).GetComponent<Text>();
        Text description = infoPanel.transform.GetChild(1).GetComponent<Text>();

        if(item) {
            itemName.text = item.itemName;
            description.text = item.description;
        } else {
            itemName.text = "Empty";
            description.text = "";
        }
        
    }

    public void OnPointerExit() {
        infoPanel.GetComponent<Image>().enabled = false;
        for(int i = 0; i < infoPanel.transform.childCount; i++) {
            infoPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
