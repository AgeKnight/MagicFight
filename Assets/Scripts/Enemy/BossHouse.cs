using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHouse : MonoBehaviour
{
    GameObject CameraPos;
    GameObject Ground;
    GameObject Main;
    [HideInInspector]
    public GameObject[] AttackRange;
    public GameObject[] BossThings; 
    public Material BackGround;
    static BossHouse instance;
    public static BossHouse Instance { get => instance; set => instance = value; }
    void Awake() 
    {
        instance = this;
        CameraPos = this.gameObject.transform.GetChild(0).gameObject;
        Ground = this.gameObject.transform.GetChild(1).gameObject;
        Main = this.gameObject.transform.GetChild(2).gameObject;
        AttackRange[0] = this.gameObject.transform.GetChild(3).gameObject;
        AttackRange[1] = this.gameObject.transform.GetChild(4).gameObject;
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            Main.GetComponent<Cinemachine.CinemachineBrain>().enabled = false; 
            Camera.main. orthographicSize = 6;
            Main.transform.position = CameraPos.transform.position;
            Main.GetComponent<Skybox>().material = BackGround;
            for (int i = 0; i < 2; i++)
            {
                BossThings[i].SetActive(true);
            }       
            GroundAnime();
        }
    }
    void GroundAnime()
    {
        Ground.SetActive(true);
    }
}
