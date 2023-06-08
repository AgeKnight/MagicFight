using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBattle : MonoBehaviour
{
    public GameObject BattleEmpty;
    public float damage;
    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Enemy"||other.gameObject.tag == "NotEnemy"|| other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet"||other.gameObject.tag == "Boss" )&&BattleEmpty==null)
        {
            BattleEmpty = other.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy"||other.gameObject.tag == "NotEnemy"|| other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet"||other.gameObject.tag == "Boss" )
        {
            BattleEmpty = null;
        }
    }
}
