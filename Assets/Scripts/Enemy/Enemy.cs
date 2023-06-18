using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    bool canReturn = true;
    [HideInInspector]
    public SpriteRenderer sprite;
    [HideInInspector]
    public float AnabiosisTime = 0;
    [HideInInspector]
    public bool isInBubble = false;
    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public bool isDied = false;
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
        //0 一般 1 攻擊 2 受擊
        public Sprite[] sprite;
    }
    public EnemyType enemyType;
    public onDamage damage;
    public float speed;
    void Awake() 
    {
        sprite = GetComponent<SpriteRenderer>();  
    }
    void Update()
    {
        if (!UsageCase.isLocked||GameManager.Instance.isEsc)
        {
            return;
        }
        if(sprite.sprite != damage.sprite[0])
        {
            Invoke("SpriteChange",0.3f);
        }
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
    void SpriteChange()
    {
        sprite.sprite = damage.sprite[0];
    }
    void Anabiosis()
    {
        transform.gameObject.tag = "NotEnemy";
        AnabiosisTime += Time.deltaTime;
        if (AnabiosisTime > damage.AllAnabiosisTime)
        {
            this.gameObject.GetComponent<HpController>().hpBar.gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            this.gameObject.GetComponent<HpController>().hp += 10;
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
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player"&&canMove)
        {
            sprite.sprite = damage.sprite[1];
            other.gameObject.GetComponent<HpController>().OnDamage(damage.damage);
        }
    }
    public void DrogItem()
    {
        int i = Random.Range(0, damage.items.Length);
        Instantiate(damage.items[i], transform.position, Quaternion.identity);
    }
}
