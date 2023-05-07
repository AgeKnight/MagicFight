using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public List<Bullet> bullets = new List<Bullet>();
    [HideInInspector]
    public Bullet bullet;
    static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }
    [HideInInspector]
    public int CharatorNum = 1;
    [HideInInspector]
    public bool isDie = false;
    public GameObject Pause;
    [HideInInspector]
    public bool isEsc = false;
    public Text GameMessager;
    void Awake()
    {
        Time.timeScale = 1;
        Instance = this;
    }
    void Update()
    {
        if (bullets.Count >= 3)
        {
            ThreeDie();
        }
        EscapeTitle();
    }
    void ThreeDie()
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            bullets[i].Die();
        }
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().Die();
        }
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().Die();
        }
    }
    void EscapeTitle()
    {
        if(Input.GetKeyDown(KeyCode.Escape)&&!isDie)
        {
            GameMessager.text = "暫停中";
            isEsc = !isEsc;
            Pause.SetActive(isEsc);
        }
        if(isEsc)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        if(isDie)
        {
            GameMessager.text = "你輸了";
            isEsc = true;
            Pause.SetActive(true);
        }        
    }
    public void Return()
    {
        SceneManager.LoadScene("Game");
    }
}
