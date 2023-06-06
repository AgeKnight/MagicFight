using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    float attackTime = 0;
    public float allAttackTime = 0.5f;
    [HideInInspector]
    public int Direction = 1;
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
    public float speed;
    public float damage;
    void Update()
    {
        Move();
    }
    void Move()
    {
        switch (attack.attackType)
        {
            case AttackType.Fire:
                transform.Translate(Direction * speed * Time.deltaTime, 0, 0);            
                break;
            case AttackType.Light:
                attackTime+=Time.deltaTime;
                if(attackTime>=allAttackTime)
                {
                    Die();
                }
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.Instance.OnDamage(damage);
            if (attack.attackType == AttackType.Fire)
            {
                Die();
            }
        }
        if ((other.gameObject.tag == "Floor" && attack.attackType == AttackType.Fire) || ((other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet") && attack.canAttack))
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
