using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public bool canThreeDelete = false;
    float horizontal = 0;
    [HideInInspector]
    public float Horizontal { get => horizontal; set => horizontal = value; }
    public float speed;
    public float allFlyTime = 0.1f;
    float flyTime = 0;
    public float allDestroyTime;
    float destoyTime;
    Rigidbody2D newRigidbody;
    [HideInInspector]
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        newRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (destoyTime >= allDestroyTime)
        {
            Die();
        }
        else
        {
            destoyTime += Time.deltaTime;
        }
        Move();
    }
    void Move()
    {
        if (flyTime >= allFlyTime)
        {
            transform.Translate(0, 0, 0);
            newRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            transform.Translate(horizontal * speed * Time.deltaTime, 0, 0);
            flyTime += Time.deltaTime;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet" && this.gameObject.tag == "Bullet")
        {
            other.gameObject.tag = "MeetBullet";
            this.gameObject.tag = "MeetBullet";
            flyTime = allFlyTime;
        }
        else if (other.gameObject.tag == "MeetBullet"&&this.gameObject.tag == "Bullet")
        {
            this.gameObject.tag = "CanThreeDelete";
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "CanThreeDelete":
                {
                    flyTime = allFlyTime;
                    this.gameObject.tag = "CanThreeDelete";
                    gameManager.bullets.Add(this);
                    canThreeDelete = true;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }
}
