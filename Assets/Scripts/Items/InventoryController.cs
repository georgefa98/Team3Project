using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void InventoryChangeHandler();

public class InventoryController : MonoBehaviour
{

    public string inventoryName;
    public Inventory inventoryValues;
    private Inventory inventory;
    
    public InventoryChangeHandler inventoryChangeDelegate;

    void Awake()
    {
        inventory = ScriptableObject.CreateInstance("Inventory") as Inventory;
        inventory.items = new List<Item>();

        for(int i = 0; i < inventoryValues.items.Count; i++) {
            inventory.items.Add(inventoryValues.items[i]);
        }

    }

    public int Length {
        get {
            if(inventory != null && inventory.items != null) {
                return inventory.items.Count; 
            } else
                return 0;
        }
    }

    public Item GetItem(int n) {
        return inventory.items[n];
    }

    public void Move(int a, int b) {
        Item tmp = inventory.items[a];
        inventory.items[a] = inventory.items[b];
        inventory.items[b] = tmp;

        if(inventoryChangeDelegate != null)
            inventoryChangeDelegate();
    }

    public void Insert(Item item, int n) {
        inventory.items[n] = item;
        if(inventoryChangeDelegate != null)
            inventoryChangeDelegate();
    }

    public void Remove(int n) {
        inventory.items[n] = null;
        if(inventoryChangeDelegate != null)
            inventoryChangeDelegate();
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
        if(inventoryChangeDelegate != null)
            inventoryChangeDelegate();
    }

}
