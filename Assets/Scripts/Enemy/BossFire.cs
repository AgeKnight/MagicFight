using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFire : MonoBehaviour
{
    public GameObject Fire;
    float FireTime = 0;
    void Update()
    {
        FireTime+=Time.deltaTime;
        if(FireTime>=0.3f)
        {
            Fire.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
