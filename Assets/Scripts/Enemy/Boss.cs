using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    bool canAttack = true;
    float attackTime = 0;
    public GameObject[] allAttack;
    public float allAttackTime = 1f;
    void Update()
    {
        if (!UsageCase.isLocked||GameManager.Instance.isEsc)
        {
            return;
        }
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
        canAttack = false;
        if (this.gameObject.GetComponent<HpController>().hpBar.value * 100 >= 50)
        {
            //CreateLight();
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
        if (Vector3.Distance(FirePos, Player.Instance.transform.position) < 15)
        {
            CreateFire();
        }
        else
        {
            GameObject temp = Instantiate(allAttack[0], FirePos, Quaternion.Euler(0,0,90));
            if (FirePos.x - Player.Instance.transform.position.x < 0)
            {
                temp.transform.rotation = Quaternion.Euler(0,0,-90);
            }
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
            y = BossHouse.Instance.AttackRange[1].transform.position.y;
        }
        if (z >= 90 && z < 180)
        {
            z = 90;
            x = BossHouse.Instance.AttackRange[1].transform.position.x;
            y = Random.Range(BossHouse.Instance.AttackRange[0].transform.position.y, BossHouse.Instance.AttackRange[1].transform.position.y);
        }
        if (z >= 180 && z < 270)
        {
            z = 180;
            x = Random.Range(BossHouse.Instance.AttackRange[0].transform.position.x, BossHouse.Instance.AttackRange[1].transform.position.x);
            y = BossHouse.Instance.AttackRange[0].transform.position.y;
        }
        if (z >= 270 && z < 360)
        {
            z = 270;
            x = BossHouse.Instance.AttackRange[0].transform.position.x;
            y = Random.Range(BossHouse.Instance.AttackRange[0].transform.position.y, BossHouse.Instance.AttackRange[1].transform.position.y);
        }
        Vector3 LightPos = new Vector3(x, y, 0);
        Instantiate(allAttack[1], LightPos, Quaternion.Euler(0, 0, z));
    }
}
