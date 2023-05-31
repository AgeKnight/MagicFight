using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    bool isCollider = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag=="Player"&&!isCollider)
        {
            isCollider = true;
            InventoryManager.Instance.AddNewItem(thisItem);
            Destroy(gameObject);           
        }
    }
}
