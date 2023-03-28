using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Bullet> bullets = new List<Bullet>();
    public Bullet bullet;
    static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }
    void Awake() 
    {
        bullet.Init(this);
    }
    void Update() 
    {
        ThreeDie();
    }
    void ThreeDie()
    {
        if(bullets.Count>=3)
        {
            Debug.Log("可以三消了");
            for (int i = bullets.Count-1; i >= 0; i--)
            {
                if(bullets[i].canThreeDelete)
                {
                    bullets[i].Die();
                }
            }
        }
    }
}
