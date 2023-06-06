using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    bool canAttack = true;
    protected float hp;
    float attackTime = 0;
    bool isDied = false;
    public Slider HPBar;
    public GameObject[] allAttack;
    public float totalHP;
    public float allAttackTime;
    void Awake()
    {
        hp = totalHP;
    }
    protected virtual void Update()
    {
        if (!UsageCase.isLocked)
        {
            return;
        }
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
        if (HPBar.value * 100 >= 50)
        {
            CreateFire();
        }
        else
        {
            CreateLight();
            CreateFire();
        }
    }
    void CreateFire()
    {
        float x = Random.Range(BossHouse.Instance.AttackRange[0].transform.position.x, BossHouse.Instance.AttackRange[1].transform.position.x);
        float y = Player.Instance.transform.position.y;
        Vector3 FirePos = new Vector3(x, y, 0);
        if (Vector3.Distance(FirePos, Player.Instance.transform.position) < 7)
        {
            CreateFire();
        }
        else
        {
            BossAttack temp = Instantiate(allAttack[0], FirePos, Quaternion.identity).gameObject.GetComponent<BossAttack>();
            if (FirePos.x - Player.Instance.transform.position.x > 0)
            {
                temp.Direction = -1;
            }
            canAttack = false;
        }
    }
    void CreateLight()
    {
        float z = Random.Range(0, 359);
        float x = 0;
        float y = 0;
        if (z >= 0 && z < 90)
        {
            z = 0;
            x = Random.Range(BossHouse.Instance.AttackRange[0].transform.position.x, BossHouse.Instance.AttackRange[1].transform.position.x);
            y = BossHouse.Instance.AttackRange[1].transform.position.y + 3;
        }
        if (z >= 90 && z < 180)
        {
            z = 90;
            x = BossHouse.Instance.AttackRange[1].transform.position.x - 3;
            y = Random.Range(BossHouse.Instance.AttackRange[0].transform.position.y, BossHouse.Instance.AttackRange[1].transform.position.y);
        }
        if (z >= 180 && z < 270)
        {
            z = 180;
            x = Random.Range(BossHouse.Instance.AttackRange[0].transform.position.x, BossHouse.Instance.AttackRange[1].transform.position.x);
            y = BossHouse.Instance.AttackRange[0].transform.position.y - 3;
        }
        if (z >= 270 && z < 360)
        {
            z = 270;
            x = BossHouse.Instance.AttackRange[0].transform.position.x + 3;
            y = Random.Range(BossHouse.Instance.AttackRange[0].transform.position.y, BossHouse.Instance.AttackRange[1].transform.position.y);
        }
        Vector3 LightPos = new Vector3(x, y, 0);
        Instantiate(allAttack[1], LightPos, Quaternion.Euler(0, 0, z));
        canAttack = false;
    }
    protected virtual void Die()
    {
        isDied = true;
        UsageCase.progress=2;
        UsageCase.isLocked = false;
        GameManager.Instance.isWin = true;
        Destroy(this.gameObject);
    }
    public void OnDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            HPBar.gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            Die();
        }
    }
}
