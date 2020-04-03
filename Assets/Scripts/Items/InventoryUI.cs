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
    Texture2D texture;
    GameObject[] slots;
    GameObject infoPanel;

    public GrabbedItem grabbedItem;
    public bool grabbingItem;
    public Item selected;
    public int selectedIndex;

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

    // Update is called once per frame
    void Update()
    {
        
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

        texture = Resources.Load("Images/UI/wood_icon") as Texture2D;
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

        grabbingItem = false;

        mainPanel.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Horizontal, 0f);
        mainPanel.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Vertical, 0f);
    }

    public void Grab(int index, Item item) {
        grabbedItem.CurrentItem = item;
        grabbedItem.Show();
        selected = item;
        selectedIndex = index;
    }

    public void Move(int toIndex, Item item) {
        grabbedItem.Hide();
        slots[selectedIndex].GetComponent<ItemSlot>().CurrentItem = item;
        slots[toIndex].GetComponent<ItemSlot>().CurrentItem = selected;
        
        inventoryController.Move(selectedIndex, toIndex);
        
        selected = null;
        selectedIndex = 0;
    }
}
