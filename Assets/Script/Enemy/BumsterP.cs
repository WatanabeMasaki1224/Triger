using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumsterP : MonoBehaviour
{
    public Transform playerTarget; 
    public float moveSpeed = 2f;
    public float stopDistance = 5f; // この距離で止まって攻撃
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float fireInterval = 2f;

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
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        Vector3 spawnPos = transform.position + direction * 1f;//敵を大きく白田個々の値も増やす// 少し前にずらす

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }
}
