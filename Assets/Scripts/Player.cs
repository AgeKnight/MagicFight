using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float bulletHorizontal=1;
    [HideInInspector]
    public GameObject Bullet;
    [HideInInspector]
    public GameObject Knife;
    [HideInInspector]
    public Transform KnifePosition;
    public float JumpSpeed;
    bool canJump=true;
    Rigidbody2D rigidBody;
    Vector3 eulerPlayer = new Vector3(0,180,0);
    [HideInInspector]
    public Transform bulletPosition;
    // Start is called before the first frame update
    void Awake()
    {
        rigidBody=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        EnterBattle();
        Jump();
    }

    void Move()
    {     
        float horizontal = 0 ; 
        if(Input.GetKey(KeyCode.A))
        {
            horizontal = -1;   
            bulletHorizontal = -1;  
            eulerPlayer = new Vector3(0,0,0);          
        }
        else if(Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
            bulletHorizontal = 1;
            eulerPlayer = new Vector3(0,180,0);
        }
        transform.Translate(new Vector2(horizontal*speed*Time.deltaTime,0),Space.World);
        transform.rotation = Quaternion.Euler(eulerPlayer);

    }
    void Jump()
    {
        if((Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.Space))&&canJump)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,JumpSpeed);
        }
    }
    void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            Bullet bullet = Instantiate(Bullet,bulletPosition.position,Quaternion.identity).GetComponent<Bullet>();
            bullet.Horizontal = bulletHorizontal;
        }
    }
    void EnterBattle()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Instantiate(Knife,KnifePosition.position,Knife.transform.rotation);
        }
    }
    void OnCollisionEnter2D(Collision2D other) 
    {
        switch(other.gameObject.tag)
        {
            case "Floor":
            {
                canJump=true;
                break;
            }          
        }        
    }
    void OnCollisionExit2D(Collision2D other) 
    {
        switch(other.gameObject.tag)
        {
            case "Floor":
            {
                canJump=false;
                break;
            }          
        }      
    }
}
