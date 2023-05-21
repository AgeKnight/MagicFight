using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    float hp;
    bool isDied = false;
    bool canMove = true;
    bool canReturn = false;
    float attackTime = 0;
    float AnabiosisTime = 0;
    Vector3 thisPosition;
    Vector3 targetPosition;
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
    public float moveDistance;
    public float AllAnabiosisTime;
    public float hurtDistance;
    void Awake()
    {
        hp = totalHP;
        if (enemyType == EnemyType.poland)
        {
            thisPosition = transform.position;
            targetPosition = new Vector2(transform.position.x + moveDistance, transform.position.y);
        }
    }
    void Update()
    {
       // HPBar.value = hp / totalHP;
        if (enemyType != EnemyType.Boss)
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
                    transform.position = Vector3.MoveTowards(transform.position, thisPosition, speed * Time.deltaTime);
                    if (transform.position.x == thisPosition.x)
                    {
                        canReturn = false;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    if (transform.position.x == targetPosition.x)
                    {
                        canReturn = true;
                    }
                }
                break;
            case EnemyType.chasingMonster:
                // if (player != null)
                // {
                //     Vector3 tempPos = targetPosition - transform.position;
                //     tempPos *= speed;
                //     rigidBody.velocity = new Vector2(tempPos.x, tempPos.y);
                // }
                break;
            default:
                break;
        }

    }
    public void Die()
    {
        if (enemyType == EnemyType.Boss)
        {
            GameManager.Instance.Win();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void OnDamage(float damage, float hurtDistance, bool isUseKnife)
    {
        hp -= damage;
        if (hp <= 0)
        {
            if (isDied || enemyType == EnemyType.Boss || !isUseKnife)
            {
                Die();
            }
            hp = 0;
            canMove = false;
        }
        if (enemyType != EnemyType.Boss)
        {
            Hurt(hurtDistance);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && enemyType != EnemyType.Boss)
        {
            other.gameObject.GetComponent<Player>().OnDamage(damage, hurtDistance, this);
        }
    }
    void Hurt(float hurtDistance)
    {
        Vector3 hurtPosition = new Vector3();
        int EnemyDirection = 0;
        if (GameManager.Instance.player.transform.position.x - transform.position.x < 0)
        {
            EnemyDirection = 1;
        }
        else if (GameManager.Instance.player.transform.position.x - transform.position.x > 0)
        {
            EnemyDirection = -1;
        }
        hurtPosition.y = transform.position.y;
        hurtPosition.x = transform.position.x + hurtDistance * EnemyDirection;
        transform.position = Vector3.MoveTowards(transform.position, hurtPosition, 100 * speed * Time.deltaTime);
    }
}
