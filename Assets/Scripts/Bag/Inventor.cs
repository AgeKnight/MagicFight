using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/NewInventory")]
[Serializable]
public class Inventor : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
}
