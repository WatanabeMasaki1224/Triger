using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iruger : MonoBehaviour
{
    public float speed = 2f;
    private Transform target;
    public int damage = 1;
    public GameObject explosionPrefab; // 爆発アニメーションPrefab
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

        Flip();
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
            Explode();
            return;
        }
        else if (other.CompareTag("Player"))
        {
            PlayerController playerCtrl = other.GetComponent<PlayerController>();
            if (playerCtrl != null)
            {
                playerCtrl.TakeDamage(damage); // PlayerController に TakeDamage があれば呼ぶ
            }
            Explode();
            return;
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void Flip()
    {
        if (target == null) return;

        Vector3 scale = transform.localScale;

        if (target.position.x > transform.position.x)
            scale.x = -Mathf.Abs(scale.x);  // 右向き
        else
            scale.x = Mathf.Abs(scale.x); // 左向き

        transform.localScale = scale;
    }
}
