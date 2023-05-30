using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;
    public static InventoryManager Instance { get => instance; set => instance = value; }
    public Inventor bag;
    public GameObject slotPrefab;
    public Text text;
    public List<GameObject> slotsList = new List<GameObject>();
    void Awake()
    {
        instance = this;
        Refresh();
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
            slotsList[i] = Instantiate(slotPrefab, transform.position, Quaternion.identity);
            slotsList[i].transform.SetParent(this.gameObject.transform);
            slotsList[i].GetComponent<Slot>().SetSlot(bag.itemList[i]);
        }
    }
    public void AddNewItem(Item item)
    {
        for (int i = 0; i < bag.itemList.Count; i++)
        {
            if (bag.itemList[i] == null)
            {
                bag.itemList[i] = item;
                break;
            }
        }
        Refresh();
    }
    public void InforMation(Item item)
    {
        text.text = item.ItemInfo;
    }
}
