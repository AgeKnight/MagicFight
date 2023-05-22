using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBattle : MonoBehaviour
{

    [HideInInspector]
    public Enemy enemy;
    [HideInInspector]
    public Bullet bullet;
    [HideInInspector]
    public Boss boss;
    public float damage;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy"&& enemy == null)
        {
            enemy = other.GetComponent<Enemy>();
        }
        if ((other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet") && bullet == null)
        {
            bullet = other.GetComponent<Bullet>();
        }
        if (other.gameObject.tag == "Boss" )
        {
            boss = other.GetComponent<Boss>();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemy = null;
        }
        if (other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet")
        {
            bullet = null;
        }
        if (other.gameObject.tag == "Boss" )
        {
            boss = null;
        }
    }
}
