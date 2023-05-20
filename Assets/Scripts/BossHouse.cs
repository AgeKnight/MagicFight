using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHouse : MonoBehaviour
{
    PolygonCollider2D bossColider ; 
    public GameObject Cm2;
    public GameObject Cm1;
    public GameObject[] Ground;
    void Awake() 
    {
        bossColider = GetComponent<PolygonCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            Cm1.GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = false; 
            Cm1.GetComponent<Cinemachine.CinemachineConfiner>().enabled = false; 
            Cm2.GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = true; 
            Cm2.GetComponent<Cinemachine.CinemachineConfiner>().enabled = true; 
            Invoke("GroundAnime",0.1f);

        }
    }
    void GroundAnime()
    {
        Ground[0].GetComponent<Animator>().enabled = true;
        Ground[1].GetComponent<Animator>().enabled = true;
    }
}
