using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpController : MonoBehaviour
{
    [HideInInspector]
    public float hp;
    public float totalHP;
    public Slider hpBar;
    // Start is called before the first frame update
    void Start()
    {
        hp = totalHP;
    }
    // Update is called once per frame
    void Update()
    {
        hpBar.value = hp / totalHP;
    }
    public void OnDamage(float damage, Vector3 hisTransform = new Vector3(), float hurtDistance = 0, bool isUseKnife = false)
    {
        if (this.gameObject.tag == "Player" && !this.gameObject.GetComponent<Player>().isDodge && !this.gameObject.GetComponent<Player>().isInvincible)
        {
            hp -= damage;
            this.gameObject.GetComponent<Player>().isDodge = true;
        }
        if (this.gameObject.tag == "Enemy" || this.gameObject.tag == "Boss")
        {
            hp -= damage;
            if (this.gameObject.GetComponent<Enemy>())
            {
                this.gameObject.GetComponent<Enemy>().sprite.sprite = this.gameObject.GetComponent<Enemy>().damage.sprite[2];
            }
        }
        Hurt(hurtDistance, hisTransform);
        if (hp <= 0)
        {
            if (this.gameObject.tag == "Enemy" && !this.gameObject.GetComponent<Enemy>().isDied && !isUseKnife)
            {
                this.gameObject.GetComponent<Enemy>().canMove = false;
            }
            else
            {
                Die();
            }
            hp = 0;
            hpBar.gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }
    }
    void Hurt(float hurtDistance, Vector3 hisTransform)
    {
        Vector3 hurtPosition = new Vector3();
        int EnemyDirection = 0;
        if (hisTransform.x - transform.position.x < 0)
        {
            EnemyDirection = 1;
        }
        else if (hisTransform.x - transform.position.x > 0)
        {
            EnemyDirection = -1;
        }
        hurtPosition.y = transform.position.y;
        hurtPosition.x = transform.position.x + hurtDistance * EnemyDirection;
        transform.position = Vector3.MoveTowards(transform.position, hurtPosition, 100 * Time.deltaTime);
    }
    public void Die()
    {
        switch (this.gameObject.tag)
        {
            case "Player":
                GameManager.Instance.isDie = true;
                break;
            case "Enemy":
                this.gameObject.GetComponent<Enemy>().DrogItem();
                break;
            case "NotEnemy":
                this.gameObject.GetComponent<Enemy>().DrogItem();
                break;
            case "Boss":
                UsageCase.progress = 2;
                UsageCase.isLocked = false;
                GameManager.Instance.isWin = true;
                break;
        }
        Destroy(this.gameObject);
    }
}
