using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject Bullet;
    public GameObject Knife;
    public Transform KnifePosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        EnterBattle();
    }

    void Move()
    {
        float horizontal = 0;
        float vertical = 0;

        if(Input.GetKey(KeyCode.W))
        {
            horizontal = 0;
            vertical = 1;
        }
        if(Input.GetKey(KeyCode.S))
        {
            horizontal = 0;
            vertical = -1;
        }
        if(Input.GetKey(KeyCode.A))
        {
            horizontal = -1;
            vertical = 0;
        }
        if(Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
            vertical = 0;
        }

        transform.Translate(new Vector2(horizontal*speed*Time.deltaTime,vertical*speed*Time.deltaTime),Space.World);
    }
    void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(Bullet,this.transform.position,Quaternion.identity);
        }
    }
    void EnterBattle()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Instantiate(Knife,KnifePosition.position,Knife.transform.rotation);
        }
    }
}
