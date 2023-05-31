using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/NewInventory")]
[Serializable]
public class Inventor : ScriptableObject
{
    public List<InventoryItem> itemList = new List<InventoryItem>();
}

[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int nums;
}
