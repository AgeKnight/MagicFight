using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowBubble : MonoBehaviour
{
    Bullet Bubbles;
    void Update() 
    {
        Blow();
    }
    void Blow()
    {
        if (Input.GetKey(KeyCode.L) && Bubbles != null)
        {
            Bubbles.Horizontal = Player.Instance.bulletHorizontal;
            Bubbles.canBlow = true;
            Bubbles.nowCorotine = StartCoroutine(Bubbles.BeBlow());
        }
        if (Input.GetKeyUp(KeyCode.L)&& Bubbles != null)
        {
            Bubbles.canBlow = false;
            Bubbles.nowCorotine = null;
            Bubbles.blowTime = 0;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Bullet" || other.gameObject.tag == "MeetBullet") && Bubbles == null)
        {
            Bubbles = other.gameObject.GetComponent<Bullet>();
            Bubbles.canBlow = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Bullet>() == Bubbles&& Bubbles != null)
        {
            Bubbles.canBlow = false;
            Bubbles = null;
        }
    }
}
