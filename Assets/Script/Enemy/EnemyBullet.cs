using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public float lifeTime = 3f;
    private Vector3 direction;

    public void Setup(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime); // ˆê’èŽžŠÔŒã‚ÉŽ©“®”j‰ó
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }
            else if (other.CompareTag("Base"))
            {
                var baseObj = other.GetComponent<BaseController>();
                if (baseObj != null)
                {
                    baseObj.TakeDamage(damage);
                }
            }
            Destroy(gameObject);
        }
    }
}
