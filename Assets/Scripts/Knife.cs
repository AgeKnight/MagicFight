using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }
    void Attack()
    {
        transform.Rotate(0, 0, 90 * Time.deltaTime * 4);
        Die();
        
    }
    void Die()
    {
        Destroy(gameObject,0.4f);
    }
}
