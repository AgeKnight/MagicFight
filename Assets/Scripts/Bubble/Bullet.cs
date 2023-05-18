using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region "Internal"
    float horizontal = 0;
    float destoyTime;
    float bigTime = 0;
    #endregion
    #region "Hide"
    [HideInInspector]
    public float flyTime = 0;
    [HideInInspector]
    public bool canBlow = false;
    [HideInInspector]
    public float Horizontal { get => horizontal; set => horizontal = value; }
    [HideInInspector]
    public Rigidbody2D newRigidbody;
    [HideInInspector]
    public float blowTime = 0;
    [HideInInspector]
    public bool canShoot = false;
    [HideInInspector]
    public bool isGas = false;
    [HideInInspector]
    public GameObject bulletCanceled;
    public Coroutine nowCorotine;
    [HideInInspector]
    public BubbleClose bubbleClose;
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
    public float hurtDistance;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        newRigidbody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isEsc || !GameManager.Instance.isDie)
        {
            if (destoyTime >= allDestroyTime)
            {
                Die();
            }
            if (canShoot)
            {
                Move();          
            }
        }
    }
    void Move()
    {
        if (flyTime >= allFlyTime)
        {
            destoyTime += Time.deltaTime;
            bigTime = BiggiestScale;
            transform.localScale = new Vector3(bigTime, bigTime, bigTime);
            transform.Translate(0, 0, 0);
        }
        else
        {
            nowCorotine = StartCoroutine(Bigger());
            flyTime += Time.deltaTime;
            if (isGas)
            {
                transform.Translate(horizontal * slowSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.Translate(horizontal * speed * Time.deltaTime, 0, 0);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (!GameManager.Instance.isEsc || !GameManager.Instance.isDie)
        {
            if (other.gameObject.tag != "Player")
            {
                flyTime = allFlyTime;
            }
            if (other.gameObject.tag == "Enemy")
            {
                if (isGas)
                {
                    damage *= 3;
                }
                other.gameObject.GetComponent<Enemy>().OnDamage(damage, hurtDistance,true);
                Die();
            }
            if (other.gameObject.tag == "NotEnemy")
            {
                bubbleClose.isEnemy = true;
                bubbleClose.gameObject.GetComponent<SpriteRenderer>().sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;
                bubbleClose.Item = other.gameObject;
                other.gameObject.SetActive(false);
            }
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (!GameManager.Instance.isEsc || !GameManager.Instance.isDie)
        {
            if (other.gameObject.tag == "Bullet" && this.gameObject.tag == "Bullet" && !isGas && !other.gameObject.GetComponent<Bullet>().isGas)
            {
                other.gameObject.tag = "MeetBullet";
                this.gameObject.tag = "MeetBullet";
            }
            if (other.gameObject.tag == "MeetBullet" && this.gameObject.tag == "Bullet" && !isGas && !other.gameObject.GetComponent<Bullet>().isGas)
            {
                this.gameObject.tag = "CanThreeDelete";
            }
            if (other.gameObject.tag == "CanThreeDelete")
            {
                this.gameObject.tag = "CanThreeDelete";
                bulletCanceled.SetActive(true);
                GameManager.Instance.bullets.Add(this);
            }
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (!GameManager.Instance.isEsc || !GameManager.Instance.isDie)
        {
            if (other.gameObject.tag == "MeetBullet")
            {
                other.gameObject.tag = "Bullet";
                this.gameObject.tag = "Bullet";
            }
        }
    }
    public void Die()
    {
        nowCorotine = null;
        bigTime = BiggiestScale;
        flyTime = allFlyTime;
        canShoot = false;
        canBlow = false;
        if (bubbleClose != null && bubbleClose.isEnemy && destoyTime < allDestroyTime)
        {
            bubbleClose.Item.GetComponent<Enemy>().Die();
        }
        if (bubbleClose.Item != null && (destoyTime >= allDestroyTime || !bubbleClose.isEnemy))
        {
            bubbleClose.Item.transform.position = this.transform.position;
            bubbleClose.Item.SetActive(true);
        }
        Destroy(this.gameObject);
        GameManager.Instance.bullets.Remove(this);
    }
    public IEnumerator BeBlow()
    {
        while (canBlow)
        {
            transform.Translate(horizontal * 4f * Time.deltaTime, 0, 0);
            blowTime += Time.deltaTime;
            yield return new WaitForSeconds(blowTime);
        }
    }
    public IEnumerator Bigger()
    {
        while (bigTime < BiggiestScale)
        {
            bigTime += Time.deltaTime;
            transform.localScale = new Vector3(bigTime, bigTime, bigTime);
            yield return new WaitForSeconds(bigTime);
        }
    }
}
