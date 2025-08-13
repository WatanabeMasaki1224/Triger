using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iruger : MonoBehaviour
{
    public int speed = 2;
    private Transform target;
    public int damage = 1;

    void Update()
    {
        if (target == null)
        {
            GameObject b = GameObject.FindGameObjectWithTag("Base");
            if (b != null)
            {
                target = b.transform;
            }
            else
            {
                // Playerがまだ見つからないなら処理せず終了
                return;
            }
        }


        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Base"))
        {
            BaseController baseCtrl = other.GetComponent<BaseController>();
            if (baseCtrl != null)
            {
                baseCtrl.TakeDamage(damage);
            }

            // 自爆（スコアは入れない）
            Destroy(gameObject);
        }
    }
}
