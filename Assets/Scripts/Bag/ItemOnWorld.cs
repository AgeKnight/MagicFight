using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    bool isCollider = false;
    void Update() 
    {
        transform.position = Vector3.MoveTowards(transform.position,Player.Instance.transform.position,10*Time.deltaTime);                     
    }
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
