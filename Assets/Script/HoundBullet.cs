using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoundBullet : MonoBehaviour
{
    private int damage;
    private float speed;
    private Transform target;
    public float range = 5f;

    public void Setup(Vector3 dir,float spd,int dmg)
    {
        speed = spd;
        damage = dmg;
        GameObject enemy = GameObject.FindWithTag("Enemy");
        if(enemy != null )
        {
            target = enemy.transform;
        }
    }

    void Update()
    {
        
    }
}
