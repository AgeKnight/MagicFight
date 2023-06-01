using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDie : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.isDie = true;
            other.gameObject.GetComponent<Player>().Die();
        }
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().Die();
        }
    }
}
