using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    GameObject infoPanel;
    InventoryUI inventoryUI;
    Image image;
    Text stackAmount;
    
    public Sprite emptyCellSprite;
    public Color emptyCellColor;

    public bool hovering;
    
    private Item item;
    public int index;

    void Awake() {
        image = transform.GetChild(0).GetComponent<Image>();
        stackAmount = transform.GetChild(1).GetComponent<Text>();
        
        emptyCellSprite = image.sprite;
        emptyCellColor = image.color;
    }

    void Start() {
        infoPanel = GameObject.FindGameObjectWithTag("ItemInfo");
        inventoryUI = transform.parent.GetComponent<InventoryUI>();
    }

    /* Because this is run in late update,
    the InventoryUI can set itemClickedSlot to false*/
    void LateUpdate() {
        if(hovering) {
            
            if(Input.GetMouseButtonDown(0)) {
                OnClick(0);
                inventoryUI.ReportSlotClicked();
            }
            else if(Input.GetMouseButtonDown(1)) {
                OnClick(1);
                inventoryUI.ReportSlotClicked();
            }
        } else {
            if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
                inventoryUI.ReportSlotNotClicked();
            }
        }
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
            itemName.text = "";
            description.text = "";
        }

        hovering = true;
    }

    public void OnPointerExit() {
        infoPanel.GetComponent<Image>().enabled = false;
        for(int i = 0; i < infoPanel.transform.childCount; i++) {
            infoPanel.transform.GetChild(i).gameObject.SetActive(false);
        }

        hovering = false;
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
                stackAmount.enabled = true;
                stackAmount.text = "" + item.stackAmount;
            } else {
                image.sprite = emptyCellSprite;
                image.color = emptyCellColor;
                stackAmount.enabled = false;
            }
        }
    }

    public void OnClick(int mouseNumber) {
        if(mouseNumber == 0)
            inventoryUI.Grab(index, item);
        else
            inventoryUI.GrabHalf(index, item);
    }
}
