using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDie : MonoBehaviour
{
    Animator aniAim;
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() && !GameManager.Instance.isInBoss)
        {
            other.gameObject.GetComponent<HpController>().Die();
        }
        if (other.gameObject.GetComponent<Enemy>()&&!other.gameObject.GetComponent<Enemy>().isInBubble)
        {
            other.gameObject.GetComponent<HpController>().Die();
        }
    }
}
