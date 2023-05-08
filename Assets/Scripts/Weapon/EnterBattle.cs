using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBattle : MonoBehaviour
{
    public float damage;
    //[HideInInspector]
    public Enemy enemy;
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
           enemy = other.GetComponent<Enemy>();
        }       
    }
    void OnTriggerStay2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
           enemy = other.GetComponent<Enemy>();
        }       
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
           enemy = null;
        } 
    }
}
