using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    float hp;
    float mp;
    float PressTime = 0;
    bool canJump = true;
    Rigidbody2D rigidBody;
    Vector3 eulerPlayer = new Vector3(0, 180, 0);
    Bullet bullet;
    float bulletHorizontal = 1;
    [HideInInspector]
    public GameObject Bullet;
    [HideInInspector]
    public GameObject Knife;
    [HideInInspector]
    public Transform KnifePosition;
    [HideInInspector]
    public Transform bulletPosition;
    public Slider[] slider;
    public float JumpSpeed;
    public float speed;
    public float totalHP;
    public float totalMP;
    // Start is called before the first frame update
    void Awake()
    {
        bullet = GetComponent<Bullet>();
        rigidBody = GetComponent<Rigidbody2D>();
        hp = totalHP;
        mp = totalMP;
    }

    // Update is called once per frame
    void Update()
    {
        slider[0].value = hp / totalHP;
        slider[1].value = mp / totalMP;
        Move();
        Shoot();
        EnterBattle();
        Jump();
    }
    void Move()
    {
        float horizontal = 0;
        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1;
            bulletHorizontal = -1;
            eulerPlayer = new Vector3(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
            bulletHorizontal = 1;
            eulerPlayer = new Vector3(0, 180, 0);
        }
        transform.Translate(new Vector2(horizontal * speed * Time.deltaTime, 0), Space.World);
        transform.rotation = Quaternion.Euler(eulerPlayer);

    }
    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && canJump)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpSpeed);
        }
    }
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
        if (Input.GetKey(KeyCode.J))
        {
            PressTime += Time.deltaTime;
            if (PressTime > 0.3f)
            {
                bullet.isGas = true;
                bullet.BiggiestScale = bullet.gasScale;
                StartCoroutine(bullet.Bigger());
            }
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            StopCoroutine(bullet.Bigger());
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
            Instantiate(Knife, KnifePosition.position, Knife.transform.rotation);
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
}
