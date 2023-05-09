using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region "Internal"
    float hp;
    float mp;
    float BattleEnterTime;
    float PressTime = 0;
    float bulletHorizontal = -1;
    float dodgeTime = 0;
    bool isUseKnife = false;
    Rigidbody2D rigidBody;
    Bullet Bubbles;
    Bullet bullet;
    SpriteRenderer sprite;
    Animator animator;
    Vector3 scale;
    #endregion
    #region "Hide"
    [HideInInspector]
    public int scaleX = 1;
    [HideInInspector]
    public int canJump = 0;
    [HideInInspector]
    public GameObject Bullet;
    [HideInInspector]
    public GameObject Knife;
    [HideInInspector]
    public Transform KnifePosition;
    [HideInInspector]
    public Transform bulletPosition;
    [HideInInspector]
    public Slider[] slider;
    [HideInInspector]
    public EnterBattle enterBattle;
    [HideInInspector]
    public bool isDodge = false;
    #endregion
    #region  "Public"
    public float JumpSpeed;
    public float speed;
    public float totalHP;
    public float totalMP;
    public float allDodge;
    public float hurtDistance;
    #endregion
    void Awake()
    {
        bullet = GetComponent<Bullet>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        scale = transform.localScale;
        hp = totalHP;
        mp = totalMP;
    }
    void Update()
    {
        slider[0].value = hp / totalHP;
        slider[1].value = mp / totalMP;
        if (!GameManager.Instance.isEsc || !GameManager.Instance.isDie)
        {
            if (isDodge)
            {
                dodgeTime += Time.deltaTime;
                if (dodgeTime >= allDodge)
                {
                    dodgeTime = 0;
                    isDodge = false;
                    animator.SetBool("isDodgeL", false);
                    animator.SetBool("isDodgeR", false);
                }
            }
            if (isUseKnife)
            {
                BattleEnterTime += Time.deltaTime;
                if (BattleEnterTime >= 0.5f)
                {
                    BattleEnterTime = 0;
                    isUseKnife = false;
                }
            }
            Move();
            Shoot();
            EnterBattle();
            Jump();
            Blow();
            Dodge();
        }
    }
    #region "角色移動與跳躍"
    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        float horizontal = 0;
        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1;
            bulletHorizontal = -1;
            scaleX = 1;
            GameManager.Instance.CharatorNum = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
            bulletHorizontal = 1;
            scaleX = -1;
            GameManager.Instance.CharatorNum = 1;
        }
        transform.Translate(new Vector2(horizontal * speed * Time.deltaTime, 0), Space.World);
        transform.localScale = new Vector3(scale.x * scaleX, scale.y, scale.z);
    }
    /// <summary>
    /// 跳躍
    /// </summary>
    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && canJump < 2)
        {
            canJump += 1;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpSpeed);
        }
    }
    #endregion
    #region "Attack"
    /// <summary>
    /// 按著j不動可以集氣 放開按鍵才能發射泡泡
    /// 設定按著j不動超過0.3秒就是集氣發射，小於0.3秒是一般大小的泡泡
    /// </summary>
    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.J) && mp > 0)
        {
            bullet = Instantiate(Bullet, bulletPosition.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.Horizontal = bulletHorizontal;
            mp -= bullet.useMP;
            if (mp < 0)
            {
                mp = 0;
            }
        }
        if (Input.GetKey(KeyCode.J) && mp > 0)
        {
            PressTime += Time.deltaTime;
            if (PressTime > 0.3f)
            {
                bullet.isGas = true;
                bullet.BiggiestScale = bullet.gasScale;
                bullet.nowCorotine = StartCoroutine(bullet.Bigger());
            }
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            bullet.nowCorotine = null;
            if (bullet.isGas)
            {
                mp -= bullet.useMP * 2;
                if (mp < 0)
                {
                    mp = 0;
                }
            }
            PressTime = 0;
            bullet.canShoot = true;         
        }
    }
    void EnterBattle()
    {
        if (Input.GetKeyDown(KeyCode.K) && !isUseKnife)
        {
            Instantiate(Knife,KnifePosition.position,Quaternion.identity);
            if (enterBattle.enemy != null)
            {
                enterBattle.enemy.OnDamage(enterBattle.damage, hurtDistance);
            }
            if (enterBattle.bullet != null)
            {
                enterBattle.bullet.Die();
            }
            isUseKnife = true;
        }
    }
    /// <summary>
    /// 按L吹氣 當泡泡離開trigger範圍後無法吹動
    /// </summary>
    void Blow()
    {
        if (Input.GetKey(KeyCode.L) && Bubbles != null)
        {;
            Bubbles.Horizontal = bulletHorizontal;
            Bubbles.canBlow = true;
            Bubbles.nowCorotine = StartCoroutine(Bubbles.BeBlow());
        }
        if (Input.GetKeyUp(KeyCode.L)&& Bubbles != null)
        {
            Bubbles.canBlow = false;
            Bubbles.nowCorotine = null;
            Bubbles.blowTime = 0;
        }
    }
    /// <summary>
    /// 閃避
    /// </summary>
    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.S) && !isDodge)
        {
            isDodge = true;
            if (scaleX > 0)
            {
                animator.SetBool("isDodgeL", isDodge);
            }
            if (scaleX < 0)
            {
                animator.SetBool("isDodgeR", isDodge);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag=="Floor")
        {
            canJump = 0;
        }
    }
    #endregion
    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet") && Bubbles == null)
        {
            Bubbles = other.gameObject.GetComponent<Bullet>();
            bullet.canBlow = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Bullet>() == Bubbles&& Bubbles != null)
        {
            Bubbles.canBlow = false;
            Bubbles = null;
        }
    }
    public void OnDamage(float damage)
    {
        if (!GameManager.Instance.isEsc || !GameManager.Instance.isDie)
        {
            if (isDodge)
            {
                damage = 0;
            }
            hp -= damage;
            if (hp < 0)
            {
                hp = 0;
                slider[0].value = hp / totalHP;
                Die();
            }
        }
    }
    public void Die()
    {
        GameManager.Instance.isDie = true;
        Destroy(this.gameObject);
    }
}
