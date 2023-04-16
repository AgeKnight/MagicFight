using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float horizontal = 0;
    float flyTime = 0;
    float destoyTime;
    Rigidbody2D newRigidbody;
    [HideInInspector]
    public float Horizontal { get => horizontal; set => horizontal = value; }
    public float speed;
    public float allFlyTime = 0.1f;
    public float allDestroyTime;
    public float damage;
    public float useMP;
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
        }
        else
        {
            transform.Translate(horizontal * speed * Time.deltaTime, 0, 0);
            StartCoroutine(Bigger());
        }
    }
    IEnumerator Bigger()
    {
        while (flyTime < allFlyTime)
        {
            flyTime += Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x+flyTime/100,transform.localScale.y+flyTime/100,transform.localScale.z);
            yield return new WaitForSeconds(allFlyTime);
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
        else if(other.gameObject.tag == "Enemy")
        {
            flyTime = allFlyTime;
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
        Destroy(this.gameObject);
        GameManager.Instance.bullets.Remove(this);
    }
}
