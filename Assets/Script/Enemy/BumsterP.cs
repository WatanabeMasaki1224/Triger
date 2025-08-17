using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumsterP : MonoBehaviour
{
    public Transform playerTarget; 
    public float moveSpeed = 2f;
    public float stopDistance = 5f; // ���̋����Ŏ~�܂��čU��
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float fireInterval = 2f;

    private float fireTimer;

    void Start()
    {
        // �V�[������ Player �I�u�W�F�N�g��T��
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
            // ���_�Ɍ������Ĉړ�
            transform.position = Vector2.MoveTowards(
                transform.position,
                playerTarget.position,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            // �U�����[�h
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
        Vector3 spawnPos = transform.position + direction * 1f;//�G��傫�����c�X�̒l�����₷// �����O�ɂ��炷

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }
}
