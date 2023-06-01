using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    bool isCreate = false;
    float MoveTime = 0;
    Vector3 nowPos;
    public enum MoveType
    {
        Horizontal,
        Vertical
    }
    public MoveType moveType;
    public float speed;
    public float moveDistance;
    public void Awake()
    {
        nowPos = transform.position;
    }
    void Update() 
    {
        Move();
    }
    void Move()
    {
        switch (moveType)
        {
            case MoveType.Horizontal:
                transform.Translate(0,speed*Time.deltaTime,0);
                if(Vector3.Distance(transform.position ,nowPos)  >= moveDistance && !isCreate)
                {
                    isCreate = true;
                    GameObject temp = Instantiate(this.gameObject,nowPos,Quaternion.identity);
                    temp.transform.SetParent(this.gameObject.transform.parent);
                }
                if(Vector3.Distance(transform.position ,nowPos) >= moveDistance*5)
                {
                    Destroy(gameObject);
                }
                break;
            case MoveType.Vertical:
                if (MoveTime < moveDistance)
                {
                    transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.Self);
                    MoveTime += Time.deltaTime;
                }
                else
                {
                    transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0), Space.Self);
                    MoveTime += Time.deltaTime;
                    if (MoveTime >= moveDistance * 2)
                    {
                        MoveTime = 0;
                    }
                }
                break;
        }
    }
}
