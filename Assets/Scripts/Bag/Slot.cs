using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [HideInInspector]
    public Item slotItem;
    [HideInInspector]
    public GameObject ItemGameObject;
    [HideInInspector]
    public Image slotImage;
    [HideInInspector]
    public Text slotNum;
    [HideInInspector]
    public string SlotInfo;
    public void SetSlot(Item item,int num)
    { 
        if(item==null)
        {
            ItemGameObject.SetActive(false);
            return;
        }
        slotItem = item;
        slotNum.text = num.ToString();
        slotImage.sprite = item.ItemImg;
        SlotInfo = item.ItemInfo;
    }
    public void UseItem()
    {
        InventoryManager.Instance.InforMation(slotItem);
    }
}
