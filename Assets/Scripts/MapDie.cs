using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDie : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() && !GameManager.Instance.isInBoss)
        {
            other.gameObject.GetComponent<HpController>().Die();
        }
        if (other.gameObject.tag == "Enemy"||other.gameObject.tag == "NotEnemy")
        {
            other.gameObject.GetComponent<HpController>().Die();
        }
    }
}
