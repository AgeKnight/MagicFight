using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor" || other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet")
        {
            Player.Instance.animator.enabled = true;
            Player.Instance.canJump = 0;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor" || other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet")
        {
            Player.Instance.animator.enabled = false;
        }
    }
}
