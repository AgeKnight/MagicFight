using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    float MoveTime = 0;
    float hp;
    float AnabiosisTime = 0;
    int CountAnabiosis = 0;
    bool canMove = true;
    bool isAttack = false;
    float attackTime = 0;
    [HideInInspector]
    public Slider HPBar;
    public float totalHP;
    public float speed;
    public float AllMovetime;
    public float AllAnabiosisTime;
    public int AllCountAnabiosis;
    public int damage;
    void Awake()
    {
        hp = totalHP;
    }
    void Update()
    {
        HPBar.value = hp / totalHP;
        if (canMove)
        {
            Move();
        }
        else
        {
            CountAnabiosis += 1;
        }
        if (hp <= 0)
        {
            AnabiosisTime += Time.deltaTime;
            if (AnabiosisTime >= AllAnabiosisTime)
            {
                canMove = true;
                hp += 10;
                AnabiosisTime = 0;
            }
        }
        if(isAttack)
        {
            attackTime+=Time.deltaTime;
            if(attackTime>=1f)
            {
                attackTime=0;
                isAttack=false;
            }
        }
    }
    void Move()
    {
        if (MoveTime <= AllMovetime)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            MoveTime += Time.deltaTime;
        }
        else
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            MoveTime += Time.deltaTime;
            if (MoveTime >= AllMovetime * 2)
            {
                MoveTime = 0;
            }
        }
    }
    public void OnDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            canMove = false;
            if (AllCountAnabiosis < CountAnabiosis)
            {
                Die();
            }
        }
    }
    void Die()
    {
        Destroy(this.gameObject);
    }
    void OnCollisionStay2D(Collision2D other) 
    {
        if(other.gameObject.tag=="Player"&&!isAttack)
        {
            other.gameObject.GetComponent<Player>().OnDamage(damage);
            isAttack=true;
        }       
    }
}
