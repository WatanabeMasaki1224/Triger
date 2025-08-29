using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
    [Header("ターゲット")]
    public Transform player;

    [Header("攻撃判定")]
    public GameObject slashPrefab;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int meleeDamage = 10;
    public int rangedDamage = 5;
    public float meleeRange = 2f;
    public float rangedRange = 6f;
    [Header("弾設定")]
    public float bulletSpeed = 5f;

    [Header("クールタイム")]
    public float meleeCoolTime = 1.5f;
    public float rangedCoolTime = 2f;
    private float meleeTimer = 0f;
    private float rangedTimer = 0f;

    
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        
    }

    private void Update()
    {
        if(player  == null)
        {
            return;
        }

        // タイマー更新
        meleeTimer -= Time.deltaTime;
        rangedTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        // 近距離攻撃
        if (distance <= meleeRange && meleeTimer <= 0f)
        {
            AttackMelee();
            meleeTimer = meleeCoolTime;
        }
        // 遠距離攻撃
        else if (distance <= rangedRange && distance > meleeRange && rangedTimer <= 0f)
        {
            AttackRanged();
            rangedTimer = rangedCoolTime;
        }
    }

    void AttackMelee()
    {
        if(slashPrefab == null)
        {
            return ;
        }

        Vector3 dir = (player.position - transform.position).normalized;
        float angle  = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        GameObject slash = Instantiate(slashPrefab, transform.position, Quaternion.Euler(0,0,angle+90f));
        BossSlash slashScript = slash.GetComponent<BossSlash>();
        if (slashScript != null)
        {
            slashScript.Setup(dir, meleeDamage); // ← Setup 関数を作る
        }
    }

    void AttackRanged()
    {
        if (bulletPrefab != null && firePoint != null && player != null)
        {
            // プレイヤーの方向を計算
            Vector3 dir = (player.position - firePoint.position).normalized;
            // 角度計算
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // 弾を生成
            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0,0,angle));
            // EnemyBullet スクリプトを取得して方向を設定
            BossBullet bullet = bulletObj.GetComponent<BossBullet>();
            if (bullet != null)
            {
                bullet.Setup(dir);
            }
        }

    }

   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangedRange);
    }


}
