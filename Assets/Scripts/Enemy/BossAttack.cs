using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public enum AttackType
    {
        Fire,
        Light
    }
    [System.Serializable]
    public struct allAttack
    {
        public AttackType attackType;
        public bool canAttack;
    }
    public allAttack attack;
    public Boss boss;
    public float speed;
    public float damage;
    void Update() 
    {
        
    }
    void Move()
    {

    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Player")
        {
            other.GetComponent<Player>().OnDamage(damage);
        }
    }
}
