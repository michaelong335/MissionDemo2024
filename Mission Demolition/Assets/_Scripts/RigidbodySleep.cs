using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class RigidbodySleep : MonoBehaviour
{
    private int sleepCountDown = 4;
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (sleepCountDown > 0)
        {
            rigid.Sleep();
            sleepCountDown--;
        }
    }
}
