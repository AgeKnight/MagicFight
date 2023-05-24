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
    Vector3 thisPosition;
    Vector3 targetPosition;
    [HideInInspector]
    public float AnabiosisTime = 0;
    public enum EnemyType
    {
        poland,
        chasingMonster,
        spider,
    }
    [System.Serializable]
    public struct onDamage
    {
        public int damage;
        public float AllAnabiosisTime;
    }
    public Slider HPBar;
    public EnemyType enemyType;
    public float totalHP;
    public onDamage damage;
    public float speed;
    public float moveDistance;
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
        HPBar.value = hp / totalHP;
        if (canMove)
        {
            Move();
            transform.gameObject.tag = "Enemy";
        }
        else
        {
            Anabiosis();
        }
    }
    void Anabiosis()
    {
        transform.gameObject.tag = "NotEnemy";
        AnabiosisTime += Time.deltaTime;
        if (AnabiosisTime > damage.AllAnabiosisTime)
        {
            HPBar.gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
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
        Destroy(this.gameObject);
    }
    public void OnDamage(float damage, float hurtDistance, bool isUseKnife)
    {
        hp -= damage;
        Hurt(hurtDistance);
        if (hp <= 0)
        {
            if (isDied || !isUseKnife)
            {
                Die();
            }
            hp = 0;
            HPBar.gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            canMove = false;
        }       
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().OnDamage(damage.damage);
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
