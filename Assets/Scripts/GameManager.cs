using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 需要存檔的:CharatorNum
    /// </summary>
    static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }
    [HideInInspector]
    public List<Bullet> bullets = new List<Bullet>();
    [HideInInspector]
    public int CharatorNum;
    [HideInInspector]
    public bool isDie;
    [HideInInspector]
    public bool isWin;
    public GameObject[] Pause;
    [HideInInspector]
    public bool isEsc;
    [HideInInspector]
    public Text GameMessager;
    public Player player;
    void Awake()
    {
        isEsc = false;
        isDie = false;
        isWin = false;
        CharatorNum = 1;
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
    void EscapeTitle()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isDie && !isWin)
        {
            GameMessager.text = "暫停中";
            isEsc = !isEsc;
            Pause[0].SetActive(isEsc);
        }
        if (isDie && !isWin)
        {
            GameMessager.text = "你輸了";
            Pause[0].SetActive(true);
        }
        if (isEsc || isWin || isDie)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void Return()
    {
        SceneManager.LoadScene("Game");
    }
    public void Win()
    {
        Pause[1].SetActive(true);
    }
    public void Quit()
    {
        SceneManager.LoadScene("Main");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().Die();
        }
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().Die();
        }
    }
}
