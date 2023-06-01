using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHouse : MonoBehaviour
{
    GameObject Main;
    GameObject PlayerTransform;
    [HideInInspector]
    public GameObject[] AttackRange;
    public GameObject[] BossThings;
    public GameObject NowCamera;
    static BossHouse instance;
    public static BossHouse Instance { get => instance; set => instance = value; }
    void Awake()
    {
        instance = this;
        Main = this.gameObject.transform.GetChild(0).gameObject;
        AttackRange[0] = this.gameObject.transform.GetChild(1).gameObject;
        AttackRange[1] = this.gameObject.transform.GetChild(2).gameObject;
        PlayerTransform = this.gameObject.transform.GetChild(3).gameObject;
    }
    public void EnterBossHouse()
    {
        Player.Instance.transform.position = PlayerTransform.transform.position;
        NowCamera.SetActive(false);
        Main.SetActive(true);
        for (int i = 0; i < BossThings.Length; i++)
        {
            BossThings[i].SetActive(true);
        }
    }
}
