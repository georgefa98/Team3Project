using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public int nItemsWidth;
    InventoryController inventoryController;
    RectTransform mainPanel;
    GameObject itemButton;
    GameObject[] slots;
    GameObject infoPanel;

    public GrabbedItem grabbedItem;
    public Item selected;
    public int selectedIndex;
    public bool isHalf;

    bool active;

    // Start is called before the first frame update
    void Start()
    {
        inventoryController = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
        mainPanel = GetComponent<RectTransform>();
        itemButton = Resources.Load("Prefabs/UI/ItemButton") as GameObject;
        infoPanel = GameObject.FindGameObjectWithTag("ItemInfo") as GameObject;

        grabbedItem.AllowClickThrough();
    }

    public void Toggle() {
        if(!active) {
            Generate();
        } else {
            Ripdown();

            infoPanel.GetComponent<Image>().enabled = false;
            for(int i = 0; i < infoPanel.transform.childCount; i++) {
                infoPanel.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void Generate() {
        int sizeX = nItemsWidth;
        int sizeY = (int)Mathf.Ceil(inventoryController.Length/(float)sizeX);

        active = true;
        mainPanel.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Horizontal, sizeX * 100f);
        mainPanel.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Vertical, sizeY * 100f);

        slots = new GameObject[inventoryController.Length];

        for(int i = 0; i < inventoryController.Length; i++) {
                slots[i] = Instantiate(itemButton, Vector3.zero, Quaternion.identity);
                slots[i].transform.SetParent(transform, false);

                RectTransform objRect = slots[i].GetComponent<RectTransform>();
                objRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 10f + 100f * Mathf.Floor(i%sizeX), 80f);
                objRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 10f + 100f * Mathf.Floor(i/sizeX), 80f);

                ItemSlot itemSlot = slots[i].GetComponent<ItemSlot>();
                itemSlot.CurrentItem = inventoryController.GetItem(i);
                itemSlot.index = i;
        }
    }

    public void Ripdown() {
        active = false;
        for(int i = 0; i < slots.GetLength(0); i++) {
                Destroy(slots[i]);
        }

        grabbedItem.Hide();

        isHalf = false;

        mainPanel.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Horizontal, 0f);
        mainPanel.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Vertical, 0f);
    }

    public void Grab(int index, Item item) {
        if(item != null && selected != null && item.itemInfo.itemName == selected.itemInfo.itemName) {
            Combine(index, item);
        } else {
            slots[index].GetComponent<ItemSlot>().CurrentItem = selected;
            inventoryController.Insert(selected, index);

            if(item != null) {
                grabbedItem.CurrentItem = item;
                grabbedItem.Show();
                selected = item;
                selectedIndex = index;
            } else {
                grabbedItem.Hide();
                selected = null;
                selectedIndex = 0;
            }
        }
    }

    public void GrabHalf(int index, Item item) {
        if(item == null && selected == null)
            return;
            
        Item first = ScriptableObject.CreateInstance("Item") as Item;
        Item second = ScriptableObject.CreateInstance("Item") as Item;
        
        if(item != null)
            first.itemInfo = item.itemInfo;
        else
            first.itemInfo = selected.itemInfo;

        second.itemInfo = first.itemInfo;

        if(selected != null) {
            if(item != null) {
                if(selected.itemInfo.itemName == item.itemInfo.itemName) {
                    second.stackAmount = selected.stackAmount / 2;
                    first.stackAmount = (selected.stackAmount - second.stackAmount) + item.stackAmount;

                    if(first.stackAmount > item.itemInfo.maxStackAmount) {
                        second.stackAmount = second.stackAmount + (first.stackAmount - item.itemInfo.maxStackAmount);
                        first.stackAmount = item.itemInfo.maxStackAmount;
                    }

                } else {
                    return;
                }
            } else {
                first.stackAmount = selected.stackAmount/2;
                second.stackAmount = selected.stackAmount - first.stackAmount;
            }
        } else {
            first.stackAmount = item.stackAmount/2;
            second.stackAmount = item.stackAmount - first.stackAmount;
        }

        if(first.stackAmount > 0)
            slots[index].GetComponent<ItemSlot>().CurrentItem = first;
        else
            slots[index].GetComponent<ItemSlot>().CurrentItem = null;

        if(second.stackAmount > 0) {
            selected = second;
            grabbedItem.CurrentItem = second;
            grabbedItem.Show();
        } else {
            grabbedItem.Hide();
        }

        selectedIndex = index;
    }

    public void Combine(int toIndex, Item item) {
        grabbedItem.Hide();

        Item newItem = ScriptableObject.CreateInstance("Item") as Item;

        int amount = item.stackAmount + selected.stackAmount;
        int leftover = 0;

        if(amount > item.itemInfo.maxStackAmount) {
            leftover = amount - item.itemInfo.maxStackAmount;
            amount = item.itemInfo.maxStackAmount;
        }

        newItem.itemInfo = item.itemInfo;
        newItem.stackAmount = amount;
        
        slots[toIndex].GetComponent<ItemSlot>().CurrentItem = newItem;
        inventoryController.Insert(newItem, toIndex);
        
        if(leftover > 0) {
            Item newItem2 = ScriptableObject.CreateInstance("Item") as Item;
            newItem2.itemInfo = item.itemInfo;
            newItem2.stackAmount = leftover;

            slots[selectedIndex].GetComponent<ItemSlot>().CurrentItem = newItem2;
            inventoryController.Insert(newItem2, selectedIndex);
        }

        selected = null;
        selectedIndex = 0;
        isHalf = false;
    }
}
