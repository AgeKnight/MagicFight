using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{
    public Bullet bullet;
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Player")
        {
            bullet.newRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }       
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Player")
        {
            bullet.newRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }       
    }
}
