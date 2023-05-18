using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    float hp;
    bool isDied = false;
    bool canMove = true;
    bool isAttack = false;
    bool canReturn = false;
    float attackTime = 0;
    float AnabiosisTime = 0;
    Vector2 thisPosition;
    Vector2 targetPosition;
    Vector2 hurtPosition;
    public enum EnemyType
    {
        poland,
        chasingMonster,
        spider,
        Boss
    }
    public Slider HPBar;
    public EnemyType enemyType;
    public float totalHP;
    public float speed;
    public int damage;
    public int allAtackTime;
    public float moveDistance;
    public float AllAnabiosisTime;
    void Awake()
    {
        hp = totalHP;
        thisPosition = transform.position;
        targetPosition = new Vector2(transform.position.x + moveDistance, transform.position.y);
    }
    void Update()
    {
        HPBar.value = hp / totalHP;
        if (!GameManager.Instance.isEsc || !GameManager.Instance.isDie)
        {
            if (canMove)
            {
                transform.gameObject.tag = "Enemy";
                Move();
            }
            else
            {
                Anabiosis();
            }
            if (isAttack)
            {
                attackTime += Time.deltaTime;
                if (attackTime >= allAtackTime)
                {
                    attackTime = 0;
                    isAttack = false;
                }
            }
        }
    }
    void Anabiosis()
    {
        transform.gameObject.tag = "NotEnemy";
        AnabiosisTime += Time.deltaTime;
        if (AnabiosisTime > AllAnabiosisTime)
        {
            hp += 10;
            canMove = true;
            isDied = true;
        }
    }
    public void Move()
    {
        switch (enemyType)
        {
            case EnemyType.poland:
                if (canReturn)
                {
                    transform.position = Vector2.MoveTowards(transform.position, thisPosition, speed * Time.deltaTime);
                    if (transform.position.x == thisPosition.x && transform.position.y == thisPosition.y)
                    {
                        canReturn = false;
                    }
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    if (transform.position.x == targetPosition.x && transform.position.y == targetPosition.y)
                    {
                        canReturn = true;
                    }
                }
                break;
            default:
                break;
        }

    }
    public void Die()
    {
        switch (enemyType)
        {
            case EnemyType.Boss:
                GameManager.Instance.isWin = true;
                GameManager.Instance.Win();
                break;
            default:
                break;
        }
        Destroy(this.gameObject);
    }
    public void OnDamage(float damage, float hurtDistance,bool isUseKnife)
    {
        if (!GameManager.Instance.isEsc || !GameManager.Instance.isDie)
        {
            hp -= damage;
            Hurt(hurtDistance);
            if (hp <= 0)
            {
                if (isDied||enemyType==EnemyType.Boss||!isUseKnife)
                {
                    Die();
                }
                hp = 0;
                canMove = false;
            }
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !isAttack)
        {
            other.gameObject.GetComponent<Player>().OnDamage(damage);
            isAttack = true;
        }
    }
    void Hurt(float hurtDistance)
    {
        hurtPosition.y = transform.position.y;
        hurtPosition.x = transform.position.x + hurtDistance * GameManager.Instance.CharatorNum;
        transform.position = Vector3.MoveTowards(transform.position, hurtPosition, 100 * speed * Time.deltaTime);
    }
}
