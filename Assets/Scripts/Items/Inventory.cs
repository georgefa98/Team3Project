using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public Item[] items;
    
    void Awake()
    {
    }

    public int GetSize() {
        return items.Length;
    }

    public Item GetItem(int n) {
        return items[n];
    }
}
