using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    bool isClear = false;
    static InventoryManager instance;
    public static InventoryManager Instance { get => instance; set => instance = value; }
    public Inventor bag;
    public GameObject slotPrefab;
    public Text text;
    List<GameObject> slotsList = new List<GameObject>();
    public Item thisItem;
    void Awake()
    {
        instance = this;
        if(!isClear)
        {
            ClearBag();
            isClear = true;
        }
        Refresh();
    }
    void ClearBag()
    {
        for(int i = 0; i < bag.itemList.Count; i++) 
        {
            if(bag.itemList[i].item!=null)
            {
                bag.itemList[i].item=null;
                bag.itemList[i].nums = 0;
            }
        }
    }
    public void Refresh()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.childCount == 0)
            {
                break;
            }
            Destroy(transform.GetChild(i).gameObject);
            slotsList.Clear();

        }
        for (int i = 0; i < bag.itemList.Count; i++)
        {
            slotsList.Add(Instantiate(slotPrefab, transform.position, Quaternion.identity));
            slotsList[i].transform.SetParent(this.gameObject.transform);
            slotsList[i].GetComponent<Slot>().SetSlot(bag.itemList[i].item,bag.itemList[i].nums);
        }
    }
    public void AddNewItem(Item item)
    {
        bool isInBag = false;
        for (int i = 0; i < bag.itemList.Count; i++)
        {
            if (bag.itemList[i].item == item)
            {
                bag.itemList[i].nums += 1;
                isInBag = true;
                break;
            }
        }
        if (!isInBag)
        {
            for (int i = 0; i < bag.itemList.Count; i++)
            {
                if (bag.itemList[i].item == null)
                {
                    bag.itemList[i].item = item;
                    bag.itemList[i].nums += 1;
                    break;
                }
            }
        }
        Refresh();
    }
    public void InforMation(Item item)
    {
        thisItem = item;
        text.text = item.ItemInfo;
    }
    void UseItem(Item item)
    {
        for (int i = 0; i < bag.itemList.Count; i++)
        {
            if (bag.itemList[i].item == item)
            { 
                Player.Instance.UseItem(item);
                bag.itemList[i].nums -= 1;
                if ( bag.itemList[i].nums == 0)
                {
                    bag.itemList[i].item = null;
                    break;
                }
            }
        }
        Refresh();
    }
    public void UseNewItem()
    {
        if (thisItem == null)
        {
            text.text = "請選擇物品!";
        }
        else
        {
            UseItem(thisItem);
        }
    }
}
