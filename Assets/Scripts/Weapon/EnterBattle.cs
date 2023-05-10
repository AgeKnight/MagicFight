using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBattle : MonoBehaviour
{

    [HideInInspector]
    public Enemy enemy;
    [HideInInspector]
    public Bullet bullet;
    public float damage;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemy = other.GetComponent<Enemy>();
        }
        if ((other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet") && bullet == null)
        {
            bullet = other.GetComponent<Bullet>();
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
    }
}
