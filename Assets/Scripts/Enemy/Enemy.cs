using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    float hp;
    bool isDied = false;
    bool canMove = true;
    bool canReturn = true;
    int OneTurn = 1;
    [HideInInspector]
    public float AnabiosisTime = 0;
    public enum EnemyType
    {
        poland,
        chasingMonster,
    }
    [System.Serializable]
    public struct onDamage
    {
        public int damage;
        public float AllAnabiosisTime;
        public GameObject[] items;
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
    }
    void Update()
    {
        HPBar.value = hp / totalHP;
        if (canMove)
        {
            Move();
            ReturnIf();
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
    void ReturnIf()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(1, -1), 1f, LayerMask.GetMask("Floor"));
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(-1, -1), 1f, LayerMask.GetMask("Floor"));
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, new Vector2(1, 0), 1f, LayerMask.GetMask("Floor"));
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, new Vector2(-1, 0), 1f, LayerMask.GetMask("Floor"));
        if (!hit.collider || !hit2.collider || hit3.collider || hit4.collider)
        {
            canReturn = !canReturn;
        }
    }

    void Move()
    {
        switch (enemyType)
        {
            case EnemyType.poland:
                if (canReturn)
                {
                    transform.Translate(speed * Time.deltaTime, 0, 0);
                }
                else
                {
                    transform.Translate(-1 * speed * Time.deltaTime, 0, 0);
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
        DrogItem();
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
        if (Player.Instance.transform.position.x - transform.position.x < 0)
        {
            EnemyDirection = 1;
        }
        else if (Player.Instance.transform.position.x - transform.position.x > 0)
        {
            EnemyDirection = -1;
        }
        hurtPosition.y = transform.position.y;
        hurtPosition.x = transform.position.x + hurtDistance * EnemyDirection;
        transform.position = Vector3.MoveTowards(transform.position, hurtPosition, 100 * speed * Time.deltaTime);
    }
    void DrogItem()
    {
        int i = Random.Range(0, damage.items.Length);
        Instantiate(damage.items[i], transform.position, Quaternion.identity);
    }
}
