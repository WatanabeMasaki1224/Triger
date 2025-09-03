using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumster : MonoBehaviour
{
    public Transform baseTarget; // 拠点のTransform
    public float moveSpeed = 2f;
    public float stopDistance = 5f; // この距離で止まって攻撃
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float fireInterval = 2f;

    private float fireTimer;

    void Update()
    {
        if (baseTarget == null) return;

        float distance = Vector2.Distance(transform.position, baseTarget.position);

        if (distance > stopDistance)
        {
            // 拠点に向かって移動
            transform.position = Vector2.MoveTowards(
                transform.position,
                baseTarget.position,
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
        Vector3 direction = (baseTarget.position - transform.position).normalized;
        Vector3 spawnPos = transform.position + direction * 0.1f;//敵を大きく白田個々の値も増やす// 少し前にずらす
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(0,0,angle));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }
}
