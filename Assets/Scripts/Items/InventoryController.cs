using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    public Inventory inventoryValues;

    private Inventory inventory;

    
    void Awake()
    {
        inventory = new Inventory();
        inventory.items = new List<Item>();
        for(int i = 0; i < inventoryValues.items.Count; i++) {
            inventory.items.Add(inventoryValues.items[i]);
        }
    }

    public int Length {
        get { return inventory.items.Count; }
        set {
            inventory.items.Capacity = value;
        }
    }

    public Item GetItem(int n) {
        return inventory.items[n];
    }

    public void Move(int a, int b) {
        Item tmp = inventory.items[a];
        inventory.items[a] = inventory.items[b];
        inventory.items[b] = tmp;
    }

}
