using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region "Internal"
    float hp;
    float mp;
    float PressTime = 0;
    float bulletHorizontal = 1;
    float dodgeTime = 0;
    bool canJump = true;
    Rigidbody2D rigidBody;
    Bullet Bubbles;
    Bullet bullet;
    SpriteRenderer sprite;
    Transform realKnifePosition;
    Transform realBulletPosition;
    #endregion
    #region "Hide"
    [HideInInspector]
    public GameObject Bullet;
    [HideInInspector]
    public GameObject Knife;
    
    public Transform[] KnifePosition;
    
    public Transform[] bulletPosition;
    [HideInInspector]
    public Slider[] slider;
    [HideInInspector]
    public bool isDodge = false;
    #endregion
    #region  "Public"
    public float JumpSpeed;
    public float speed;
    public float totalHP;
    public float totalMP;
    public float allDodge;
    Animator animator;
    #endregion
    void Awake()
    {
        bullet = GetComponent<Bullet>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        realKnifePosition = KnifePosition[0].transform;
        realBulletPosition = bulletPosition[0].transform;
        hp = totalHP;
        mp = totalMP;
    }
    void Update()
    {
        slider[0].value = hp / totalHP;
        slider[1].value = mp / totalMP;
        if (isDodge)
        {
            dodgeTime += Time.deltaTime;
            if (dodgeTime >= allDodge)
            {               
                dodgeTime = 0;
                isDodge = false;    
                animator.SetBool("isDodge", isDodge);                 
            }
        }
        Move();
        Shoot();
        EnterBattle();
        Jump();
        Blow();
        Dodge();
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
            sprite.flipX = true;
            realKnifePosition = KnifePosition[1].transform;
            realBulletPosition = bulletPosition[1].transform;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
            bulletHorizontal = 1;
            sprite.flipX = false;
            realKnifePosition = KnifePosition[0].transform;
            realBulletPosition = bulletPosition[0].transform;
        }
        transform.Translate(new Vector2(horizontal * speed * Time.deltaTime, 0), Space.World);

    }
    /// <summary>
    /// 跳躍
    /// </summary>
    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && canJump)
        {
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
            bullet = Instantiate(Bullet, realBulletPosition.position, Quaternion.identity).GetComponent<Bullet>();
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
            if (bullet.nowCorotine != null)
            {
                StopCoroutine(bullet.nowCorotine);
                bullet.nowCorotine = null;
            }
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            Instantiate(Knife, realKnifePosition.position, Knife.transform.rotation);
        }
    }
    /// <summary>
    /// 按L吹氣 當泡泡離開trigger範圍後無法吹動
    /// </summary>
    void Blow()
    {
        if (Input.GetKey(KeyCode.L) && Bubbles != null)
        {
            Bubbles.Horizontal = bulletHorizontal;
            Bubbles.nowCorotine = StartCoroutine(Bubbles.BeBlow());
        }
        if (Input.GetKeyUp(KeyCode.L) && Bubbles != null)
        {
            if (Bubbles.nowCorotine != null)
            {
                StopCoroutine(Bubbles.nowCorotine);
                Bubbles.nowCorotine = null;
            }
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
            animator.SetBool("isDodge", isDodge);
        }
    }
    #endregion
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet") && Bubbles == null)
        {
            Bubbles = other.gameObject.GetComponent<Bullet>();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Bullet>() == Bubbles)
        {
            Bubbles = null;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Floor":
                {
                    canJump = true;
                    break;
                }
            case "Enemy":
                {
                    break;
                }
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Floor":
                {
                    canJump = false;
                    break;
                }
        }
    }
    public void OnDamage(float damage)
    {
        if (isDodge)
        {
            damage = 0;
        }
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            Die();
        }
    }
    void Die()
    {
        Destroy(this.gameObject);
    }
}
