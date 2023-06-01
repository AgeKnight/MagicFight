using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 需要存檔的:hp、mp、bulletHorizontal
    /// </summary>
    #region "Internal"
    int scaleX = 1;
    float hp;
    float mp;
    float BattleEnterTime;
    float PressTime;
    float dodgeTime;
    bool isDodge = false;
    bool isUseKnife = false;
    bool isInvincible = false;
    Bullet bullet;
    SpriteRenderer sprite;
    Animator animator;
    Vector3 scale;
    #endregion
    #region "Hide"
    [HideInInspector]
    public Rigidbody2D rigidBody;
    [HideInInspector]   
    public float bulletHorizontal = -1;
    [HideInInspector]
    public bool canJump = true;
    #endregion
    [System.Serializable]
    public struct Weapon
    {
        public GameObject Bullet;
        public GameObject Knife;
        public Transform KnifePosition;
        public Transform bulletPosition;
        public EnterBattle enterBattle;
    }
    #region  "Public"
    static Player instance;
    public static Player Instance { get => instance; set => instance = value; }
    public Slider[] slider;
    public Weapon weapon;
    public float JumpSpeed;
    public float speed;
    public float totalHP;
    public float totalMP;
    public float allDodge;
    public float hurtDistance;
    #endregion
    void Awake()
    {
        instance = this;
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
        slider[1].value = mp / totalMP;
        if (isDodge || isInvincible)
        {
            dodgeTime += Time.deltaTime;
            if (dodgeTime >= allDodge)
            {
                dodgeTime = 0;
                isDodge = false;
                isInvincible = false;
            }
        }
        animator.SetBool("isInvincible", isInvincible);
        animator.SetBool("isDodge", isDodge);
        if (isUseKnife)
        {
            BattleEnterTime += Time.deltaTime;
            if (BattleEnterTime >= 0.5f)
            {
                BattleEnterTime = 0;
                isUseKnife = false;
            }
        }
        
        Shoot();
        EnterBattle();
        Jump();
        IsInvincible();

    }
    void FixedUpdate() 
    {
        Move();
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
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
            bulletHorizontal = 1;
            scaleX = -1;
        }
        Vector3 v3Dis = new Vector3(horizontal * speed * Time.deltaTime, 0, 0);
        transform.Translate(v3Dis, Space.Self);
        transform.localScale = new Vector3(scale.x * scaleX, scale.y, scale.z);
    }
    /// <summary>
    /// 跳躍
    /// </summary>
    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && canJump)
        {
            float temp = transform.position.y;
            canJump = false;
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
            bullet = Instantiate(weapon.Bullet, weapon.bulletPosition.position, Quaternion.identity).GetComponent<Bullet>();
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
                bullet.gas.BiggiestScale = bullet.gas.gasScale;
                bullet.nowCorotine = StartCoroutine(bullet.Bigger());
                bullet.flyTime = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            bullet.flyTime = 0;
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
            Instantiate(weapon.Knife, weapon.KnifePosition.position, Quaternion.identity);
            if (weapon.enterBattle.enemy != null)
            {
                weapon.enterBattle.enemy.OnDamage(weapon.enterBattle.damage, hurtDistance, isUseKnife);
            }
            if (weapon.enterBattle.bullet != null)
            {
                weapon.enterBattle.bullet.Die();
            }
            if (weapon.enterBattle.boss != null)
            {
                weapon.enterBattle.boss.OnDamage(weapon.enterBattle.damage);
            }
            isUseKnife = true;
        }
    }
    /// <summary>
    /// 閃避
    /// </summary>
    void IsInvincible()
    {
        if (Input.GetKeyDown(KeyCode.S) && !isInvincible)
        {
            isInvincible = true;
        }
    }
    #endregion
    public void OnDamage(float damage)
    {
        if (!isDodge && !isInvincible)
        {
            hp -= damage;
            isDodge = true;
            slider[0].value = hp / totalHP;
        }
        if (hp <= 0)
        {
            hp = 0;
            slider[0].value = hp / totalHP;
            slider[0].gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            Die();
        }

    }
    public void Die()
    {
        Destroy(gameObject);
    }
    public void UseItem(Item item)
    {
        if(item.itemType == Item.ItemType.hp)
        {
            hp += item.itemEffect; 
        }
        if(item.itemType == Item.ItemType.mp)
        {
            mp += item.itemEffect; 
        }
    }
}
