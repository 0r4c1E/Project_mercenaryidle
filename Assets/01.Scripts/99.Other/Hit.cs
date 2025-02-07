using BarParu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public Type.Aliy hitAliy;
    public int hitCnt;
    public int cnt;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hitAliy == Type.Aliy.Player)
        {
            if (collision.CompareTag("Enemy") && cnt < hitCnt)
            {
                cnt++;
                collision.GetComponent<CharacterController>().Dead();
                //ParuPool.inst.ReturnPoolObject("Unit_2001", collision.gameObject);
            }
        }
        else if(hitAliy == Type.Aliy.Enemy)
        {
            if (collision.CompareTag("Player") && cnt < hitCnt)
            {
                cnt++;
                //ParuPool.inst.ReturnPoolObject("Unit_2001", collision.gameObject);
            }
        }

    }
}
