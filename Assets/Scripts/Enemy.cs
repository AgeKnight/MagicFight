using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected float hp;
    float AnabiosisTime = 0;
    int CountAnabiosis = 0;
    bool canMove = true;
    bool isAttack = false;
    bool canReturn = false;
    float attackTime = 0;
    Vector2 thisPosition;
    Vector2 targetPosition;
    Vector2 hurtPosition;
    [HideInInspector]
    public Slider HPBar;
    public float totalHP;
    public float speed;
    public float moveDistance;
    public float AllAnabiosisTime;
    public int AllCountAnabiosis;
    public int damage;
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
                if (attackTime >= 1f)
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
        if (AnabiosisTime >= AllAnabiosisTime)
        {
            CountAnabiosis += 1;         
            hp += 10;
            AnabiosisTime = 0;
            canMove = true;
        }

    }
    public virtual void Move()
    {
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
    }
    public virtual void OnDamage(float damage, float hurtDistance)
    {
        if (!GameManager.Instance.isEsc || !GameManager.Instance.isDie)
        {
            hp -= damage;
            Hurt(hurtDistance);
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
    }
    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !isAttack)
        {
            other.gameObject.GetComponent<Player>().OnDamage(damage);
            isAttack = true;
        }
    }
    protected void Hurt(float hurtDistance)
    {
        hurtPosition.y = transform.position.y;
        hurtPosition.x = transform.position.x + hurtDistance * GameManager.Instance.CharatorNum;
        transform.position = Vector3.MoveTowards(transform.position, hurtPosition, 100 * speed * Time.deltaTime);
    }
}
