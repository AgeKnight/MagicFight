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
    public Rigidbody2D rigidBody;
    Vector3 thisPosition;
    Vector3 targetPosition;
    Vector3 hurtPosition;
    BoxCollider2D EnemyCollider;
    [HideInInspector]
    public Player player;
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
    public float hurtDistance;
    void Awake()
    {
        hp = totalHP;
        thisPosition = transform.position;
        targetPosition = new Vector2(transform.position.x + moveDistance, transform.position.y);
        rigidBody = GetComponent<Rigidbody2D>();
        EnemyCollider = gameObject.GetComponent<BoxCollider2D>();
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
                    transform.position = Vector3.MoveTowards(transform.position, thisPosition, speed * Time.deltaTime);
                    if (transform.position.x == thisPosition.x && transform.position.y == thisPosition.y)
                    {
                        canReturn = false;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    if (transform.position.x == targetPosition.x && transform.position.y == targetPosition.y)
                    {
                        canReturn = true;
                    }
                }
                break;
            case EnemyType.chasingMonster:
                if (player != null&&!isAttack)
                {
                    Vector3 tempPos = targetPosition - transform.position;
                    tempPos *= speed;
                    rigidBody.velocity = new Vector2(tempPos.x, tempPos.y);
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
    public void OnDamage(float damage, float hurtDistance, bool isUseKnife)
    {
        if (!GameManager.Instance.isEsc || !GameManager.Instance.isDie)
        {
            hp -= damage;
            Hurt(hurtDistance);
            if (hp <= 0)
            {
                if (isDied || enemyType == EnemyType.Boss || !isUseKnife)
                {
                    Die();
                }
                hp = 0;
                canMove = false;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !isAttack)
        {
            other.gameObject.GetComponent<Player>().OnDamage(damage, hurtDistance, this);
            isAttack = true;
            if(enemyType == EnemyType.chasingMonster)
            {
                rigidBody.gravityScale = 0;
                EnemyCollider.isTrigger = true;
                transform.position = Vector3.MoveTowards(transform.position, player.Shoose.transform.position, 30*speed * Time.deltaTime);
            }
        }
        if (other.gameObject.tag == "Floor")
        {
            if(enemyType == EnemyType.chasingMonster&&player!=null)
            {
                targetPosition = player.Head.transform.position;
                isAttack = false;
            }
        }
    }
    void Hurt(float hurtDistance)
    {
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
    public void ColliderTrigger()
    {
        rigidBody.gravityScale = 1;
        EnemyCollider.isTrigger = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject.GetComponent<Player>();
            targetPosition = player.Head.transform.position;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = null;
            targetPosition = transform.position;
        }
    }
}
