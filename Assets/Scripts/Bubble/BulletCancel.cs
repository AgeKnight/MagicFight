using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCancel : MonoBehaviour
{
    public Bullet bullet;
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Enemy")
        {
            other.GetComponent<Enemy>().OnDamage(bullet.damage*3,0,true);
        } 
        if(other.gameObject.tag=="Boss")
        {
            if(other.GetComponent<Boss>()!=null)
            {
                other.GetComponent<Boss>().OnDamage(bullet.damage*3);
            }         
        }       
    }
}
