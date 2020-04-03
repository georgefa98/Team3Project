using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    public Inventory inventoryValues;

    private Inventory inventory;
    
    void Awake()
    {
        inventory = ScriptableObject.CreateInstance("Inventory") as Inventory;
        inventory.items = new List<Item>();

        for(int i = 0; i < inventoryValues.items.Count; i++) {
            inventory.items.Add(inventoryValues.items[i]);
        }
    }

    public int Length {
        get { return inventory.items.Count; }
    }

    public Item GetItem(int n) {
        return inventory.items[n];
    }

    public void Move(int a, int b) {
        Item tmp = inventory.items[a];
        inventory.items[a] = inventory.items[b];
        inventory.items[b] = tmp;
    }

    public void Insert(Item item, int n) {
        inventory.items[n] = item;
    }

    public void Remove(int n) {
        inventory.items[n] = null;
    }

    public void ChangeCapacity(int capacity) {
        if(capacity < 0)
            return;
        
        if(capacity < Length)
            inventory.items.RemoveRange(capacity, Length - capacity);

        if(capacity > Length) {
            for(int i = 0; i < capacity - Length; i++) {
                inventory.items.Insert(Length, null);
            }
        }
    }

}
