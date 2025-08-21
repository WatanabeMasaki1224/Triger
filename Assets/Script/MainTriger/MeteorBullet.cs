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
    private Animator animator;
    private bool isExploding = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Setup(Vector3 dir, float spd, int dmg)
    {
        direction = dir.normalized; 
        speed = spd;                 
        damage = dmg;               
        Destroy(gameObject, lifeTime); 
    }

    void Update()
    {
        if (!isExploding)
        {
            transform.position += direction * speed * Time.deltaTime;
            float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
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
        if(isExploding)
        {
            return;
        }
        isExploding = true;
        speed = 0;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Enemy>()?.TakeDamage(damage);
            }
        }

        if(animator != null)
        {
            animator.SetTrigger("Explode");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnExplosionAnimationEnd()
    {
        Destroy(gameObject);
    }


    void OnDrawGizmosSelected() //爆発の範囲を視覚的に確認するためのデバッグ機能
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
