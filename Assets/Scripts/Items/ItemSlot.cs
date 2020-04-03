using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    GameObject infoPanel;
    InventoryUI inventoryUI;
    Image image;
    
    public Sprite emptyCellSprite;
    public Color emptyCellColor;
    
    private Item item;
    public int index;

    void Awake() {
        image = transform.GetChild(0).GetComponent<Image>();
        
        emptyCellSprite = image.sprite;
        emptyCellColor = image.color;
    }

    void Start() {
        infoPanel = GameObject.FindGameObjectWithTag("ItemInfo");
        inventoryUI = transform.parent.GetComponent<InventoryUI>();

        
    }

    public void OnPointerEnter() {
        infoPanel.GetComponent<Image>().enabled = true;
        
        for(int i = 0; i < infoPanel.transform.childCount; i++) {
            infoPanel.transform.GetChild(i).gameObject.SetActive(true);
        }
        Text itemName = infoPanel.transform.GetChild(0).GetComponent<Text>();
        Text description = infoPanel.transform.GetChild(1).GetComponent<Text>();

        if(item) {
            itemName.text = item.itemInfo.itemName;
            description.text = item.itemInfo.description;
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

    public Item CurrentItem {
        get {
            return item;
        }
        set {
            item = value;
            if(item != null) {
                image.sprite = item.itemInfo.icon;
                image.color = Color.white;
            } else {
                image.sprite = emptyCellSprite;
                image.color = emptyCellColor;
            }
        }
    }

    public void OnClick() {
        if(!inventoryUI.grabbingItem) {
            inventoryUI.Grab(index, item);
            inventoryUI.grabbingItem = true;
        } else {
            inventoryUI.Move(index, item);
            inventoryUI.grabbingItem = false;
        }
    }
}
