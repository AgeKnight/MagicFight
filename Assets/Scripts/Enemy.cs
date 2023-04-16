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
    [HideInInspector]
    public Slider HPBar;
    public float totalHP;
    public float speed;
    public float AllMovetime;
    public float AllAnabiosisTime;
    public int AllCountAnabiosis;
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
}
