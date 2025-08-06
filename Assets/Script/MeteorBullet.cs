using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBullet : MonoBehaviour
{
    private int damage;
    private float speed;
    private Vector3 direction;
    public float radius = 2.0f;
    public float lifeTime = 3f;

    public void Setup(Vector3 dir, float spd, int dmg)
    {
        direction = dir.normalized; 
        speed = spd;                 
        damage = dmg;               
        Destroy(gameObject, lifeTime); 
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
    }
        
    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Enemy>()?.TakeDamage(damage);
            }
        }

    }


    void OnDrawGizmosSelected() //爆発の範囲を視覚的に確認するためのデバッグ機能
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
