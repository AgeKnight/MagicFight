using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCancel : MonoBehaviour
{
    public Bullet bullet;
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Enemy"&&!bullet.isGas)
        {
            other.GetComponent<Enemy>().OnDamage(bullet.damage*3,0);
        }       
    }
}
