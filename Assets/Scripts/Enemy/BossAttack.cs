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
    public int Direction = 1;
    void Update()
    {
        Move();
    }
    void Move()
    {
        transform.Translate(Direction * speed * Time.deltaTime, 0, 0);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().OnDamage(damage);
            if (attack.attackType == AttackType.Fire)
            {
                Die();
            }
        }
        else if ((other.gameObject.tag == "Barrier" && attack.attackType == AttackType.Fire) || ((other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet" )&& attack.canAttack))
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
