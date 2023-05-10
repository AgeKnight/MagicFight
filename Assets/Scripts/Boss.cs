using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    bool canJump = false;
    bool isSeePlayer = false;
    Player player;
    [HideInInspector]
    public Rigidbody2D rigidBody;
    public float JumpSpeed;
    public override void Move()
    {
        AllCountAnabiosis = 0;
        if (isSeePlayer && player != null)
        {
            Jump();
            Vector3 targetPosition = player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject.GetComponent<Player>();
            isSeePlayer = true;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Floor":
                canJump = true;
                break;
            case "NotJump":
                canJump = false;
                break;
            default:
                break;
        }
    }
    void Jump()
    {
        if (canJump)
        {
            canJump = false;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpSpeed);
        }
    }
}
