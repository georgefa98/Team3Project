using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public int nItemsWidth;

    Inventory inventory;
    RectTransform mainPanel;
    GameObject itemButton;
    Texture2D texture;
    GameObject[] slots;
    

    bool active;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        mainPanel = GetComponent<RectTransform>();
        itemButton = Resources.Load("Prefabs/UI/ItemButton") as GameObject;
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
        }
    }

    public void Generate() {

        int sizeX = nItemsWidth;
        int sizeY = (int)Mathf.Ceil(inventory.GetSize()/(float)sizeX);

        active = true;
        mainPanel.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Horizontal, sizeX * 100f);
        mainPanel.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Vertical, sizeY * 100f);

        texture = Resources.Load("Images/UI/wood_icon") as Texture2D;
        slots = new GameObject[inventory.GetSize()];

        for(int i = 0; i < inventory.GetSize(); i++) {
                slots[i] = Instantiate(itemButton, Vector3.zero, Quaternion.identity);
                slots[i].transform.SetParent(transform, false);

                RectTransform objRect = slots[i].GetComponent<RectTransform>();
                objRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 10f + 100f * Mathf.Floor(i%sizeX), 80f);
                objRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 10f + 100f * Mathf.Floor(i/sizeX), 80f);

                Image objImage = slots[i].transform.GetChild(0).GetComponent<Image>();

                if(inventory.GetItem(i)) {
                    objImage.sprite = inventory.GetItem(i).icon;
                    objImage.color = Color.white;
                }

                Button slotButton = slots[i].GetComponent<Button>();
                slotButton.onClick.AddListener(OnClickSlot);

                ItemSlot itemSlot = slots[i].GetComponent<ItemSlot>();
                itemSlot.item = inventory.GetItem(i);
                
                
        }
    }

    public void Ripdown() {
        active = false;
        for(int i = 0; i < slots.GetLength(0); i++) {
                Destroy(slots[i]);
        }

        mainPanel.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Horizontal, 0f);
        mainPanel.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Vertical, 0f);
    }

    void OnClickSlot() {
        Debug.Log("Click");
    }
}
