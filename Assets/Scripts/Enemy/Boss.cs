using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    bool canAttack = true;
    float hp;
    float attackTime = 0;
    bool isDied = false;
    public Slider HPBar;
    public Player player;
    public GameObject[] allAttack;
    public Transform[] AttackRange;
    public float totalHP;
    public float allAttackTime;
    void Awake()
    {
        hp = totalHP;
    }
    void Update()
    {
        HPBar.value = hp / totalHP;
        if (!canAttack)
        {
            attackTime += Time.deltaTime;
            if (attackTime >= allAttackTime)
            {
                attackTime = 0;
                canAttack = true;
            }
        }
        else
        {
            Attack();
        }
    }
    void Attack()
    {
        if (player!=null)
        {
            CreateFire();
        }
    }
    void CreateFire()
    {
        float x = Random.Range(AttackRange[0].position.x, AttackRange[1].position.x);
        float y = player.transform.position.y;
        Vector3 FirePos = new Vector3(x, y, 0);
        if (Vector3.Distance(FirePos, player.transform.position) < 7)
        {
            CreateFire();
        }
        else
        {
            Instantiate(allAttack[0], FirePos, Quaternion.identity);
            canAttack = false;
        }
    }
    void Die()
    {
        isDied = true;
        GameManager.Instance.Win();
        Destroy(this.gameObject);
    }
    public void OnDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }
}
