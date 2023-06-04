using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHand : Boss
{
    bool canMove = true;
    float bossDamaged = 0;
    float tempHp = 0;
    public float damage;
    public float speed;
    public float beDamaged;
    protected override void Update()
    {
        HPBar.value = hp / totalHP;
        Move();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && canMove)
        {
            other.gameObject.GetComponent<Player>().OnDamage(damage);
        }
    }
    void Move()
    {
        if (canMove)
        {

        }
        else
        {
            if(bossDamaged - base.hp >= beDamaged)
            {
                canMove = true;
                hp=totalHP;
                HPBar.gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            } 
        }
    }
    protected override void Die()
    {
        canMove = false;
        bossDamaged = base.hp;
    }
}
