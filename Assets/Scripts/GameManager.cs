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
    bool isOpen = false;
    static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }
    [HideInInspector]
    public List<Bullet> bullets = new List<Bullet>();
    [HideInInspector]
    public bool isDie;
    [HideInInspector]
    public bool isWin;
    public GameObject[] Pause;
    [HideInInspector]
    public bool isEsc;
    [HideInInspector]
    public Text GameMessager;
    [HideInInspector]
    public bool isInBoss = false;
    void Awake()
    {
        UsageCase.isLocked = false;
        UsageCase.progress = 0;
        Pause[2].SetActive(true);
        Pause[2].SetActive(false);
        isEsc = false;
        isDie = false;
        isWin = false;
        Time.timeScale = 1;
        Instance = this;
    }
    void Update()
    {
        if (!UsageCase.isLocked)
        {
            return;
        }
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
        if (Input.GetKeyDown(KeyCode.Escape) && !isDie && !isWin && !isOpen)
        {
            GameMessager.text = "暫停中";
            isEsc = !isEsc;
            Pause[0].SetActive(isEsc);
        }
        if (isDie)
        {
            GameMessager.text = "你輸了";
            Pause[0].SetActive(true);
        }
        if (isWin)
        {
            GameMessager.text = "你贏了";
        }
        if (isEsc || isDie )
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            if (Input.GetKeyDown(KeyCode.C))
            {
                InventoryManager.Instance.thisItem = null;
                InventoryManager.Instance.text.text = "";
                isOpen = !isOpen;
                Pause[2].SetActive(isOpen);
            }
        }
    }
    public void Return()
    {
        SceneManager.LoadScene("Game");
    }
    public void Win()
    {
        Pause[0].SetActive(true);
    }
    public void Quit()
    {
        SceneManager.LoadScene("Main");
    }
    public void TextUre()
    {
        Pause[1].SetActive(false);
    }
    public void HideTextUre()
    {
        Pause[1].SetActive(true);
        UsageCase.progress=-1;
    }
}
