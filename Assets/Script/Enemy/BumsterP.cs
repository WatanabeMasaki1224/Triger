using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BumsterP : MonoBehaviour
{
    public Transform playerTarget; 
    public float moveSpeed = 2f;
    public float stopDistance = 5f; // この距離で止まって攻撃
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float fireInterval = 2f;
    public Transform firePoint;
    private float fireTimer;

    void Start()
    {
        // シーン内の Player オブジェクトを探す
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerTarget = player.transform;
    }

    void Update()
    {
        if (playerTarget == null) return;
        Flip();
        float distance = Vector2.Distance(transform.position, playerTarget.position);

        if (distance > stopDistance)
        {
            // 拠点に向かって移動
            transform.position = Vector2.MoveTowards(
                transform.position,
                playerTarget.position,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            // 攻撃モード
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = fireInterval;
            }
        }
    }

    void Shoot()
    {
        if (firePoint == null) return;
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        float  angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0,0,angle));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }

    void Flip()
    {
        if (playerTarget == null) return;
        Vector3 scale = transform.localScale;
        if(playerTarget.position.x > transform.position.x)
            scale.x = -Mathf.Abs(scale.x);
        else
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;   

    }
}
