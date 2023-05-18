using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    void Update()
    {
        Die();
    }
    void Die()
    {
        Destroy(gameObject,0.5f);
    }
}
