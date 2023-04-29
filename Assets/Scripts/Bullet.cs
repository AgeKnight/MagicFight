using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region "Internal"
    float horizontal = 0;
    float destoyTime;
    float bigTime = 0;
    float flyTime = 0;
    Rigidbody2D newRigidbody;
    #endregion
    #region "Hide"
    [HideInInspector]
    public float Horizontal { get => horizontal; set => horizontal = value; }
    [HideInInspector]
    public bool canShoot = false;
    [HideInInspector]
    public bool isGas = false;
    [HideInInspector]
    public BulletCancel bulletCancel;
    [HideInInspector]
    public GameObject bulletCanceled;
    #endregion
    #region "public"
    public float speed;
    public float allDestroyTime;
    public float damage;
    public float useMP;
    public float BiggiestScale;
    public float allFlyTime;
    public float slowSpeed;
    public float gasScale;
    #endregion
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
        else if (flyTime >= allFlyTime)
        {
            destoyTime += Time.deltaTime;
        }
        if (canShoot)
        {
            Move();
            flyTime += Time.deltaTime;
            if (flyTime >= allFlyTime)
            {
                flyTime = allFlyTime;
            }
        }
    }
    void Move()
    {
        if (flyTime >= allFlyTime)
        {
            bigTime = BiggiestScale;
            transform.localScale = new Vector3(bigTime, bigTime, bigTime);
            transform.Translate(0, 0, 0);
        }
        else
        {
            if (isGas)
            {
                transform.Translate(horizontal * slowSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.Translate(horizontal * speed * Time.deltaTime, 0, 0);
            }
            StartCoroutine(Bigger());
        }
    }
    public IEnumerator Bigger()
    {
        while (bigTime <= BiggiestScale)
        {
            bigTime += Time.deltaTime;
            transform.localScale = new Vector3(bigTime, bigTime, bigTime);
            if (bigTime > BiggiestScale)
            {
                bigTime = BiggiestScale;
                break;
            }
            yield return new WaitForSeconds(bigTime);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet" && this.gameObject.tag == "Bullet")
        {
            flyTime = allFlyTime;
            other.gameObject.tag = "MeetBullet";
            this.gameObject.tag = "MeetBullet";
        }
        else if (other.gameObject.tag == "MeetBullet" && this.gameObject.tag == "Bullet")
        {
            flyTime = allFlyTime;
            this.gameObject.tag = "CanThreeDelete";
        }
        else if (other.gameObject.tag == "Enemy")
        {
            flyTime = allFlyTime;
            if(isGas)
            {
                damage*=3;
            }
            other.gameObject.GetComponent<Enemy>().OnDamage(damage);
            Die();
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "CanThreeDelete":
                {
                    this.gameObject.tag = "CanThreeDelete";
                    bulletCanceled.SetActive(true);
                    GameManager.Instance.bullets.Add(this);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    public void Die()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
        GameManager.Instance.bullets.Remove(this);
    }
}
