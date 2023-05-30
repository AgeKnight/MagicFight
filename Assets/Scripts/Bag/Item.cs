using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/NewItem")]
[Serializable]
public class Item : ScriptableObject
{
    public string Itemname;
    public int ItemNum;
    [TextArea]
    public string ItemInfo;
    public Sprite ItemImg;
    public enum ItemType
    {
        hp,
        mp
    }
    public ItemType itemType;
}