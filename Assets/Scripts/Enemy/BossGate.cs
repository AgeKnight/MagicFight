using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGate : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            BossHouse.Instance.EnterBossHouse();
        }
    }
}
