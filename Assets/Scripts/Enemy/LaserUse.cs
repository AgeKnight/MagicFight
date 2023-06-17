using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUse : MonoBehaviour
{
    public GameObject Laser;
    public void UseLaser()
    {
        Laser.SetActive(true);
    }
}
