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

    [Header("画像設定")]
    public Sprite idleSprite;   // 待機
    public Sprite attackSprite; // 攻撃
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = idleSprite;
    }

    private void Update()
    {
        if(player  == null)
        {
            return;
        }

        Flip();
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
        
        spriteRenderer.sprite = attackSprite;
        Vector3 dir = (player.position - transform.position).normalized;
        float angle  = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        GameObject slash = Instantiate(slashPrefab, transform.position, Quaternion.Euler(0,0,angle+90f));
        BossSlash slashScript = slash.GetComponent<BossSlash>();
        if (slashScript != null)
        {
            slashScript.Setup(dir, meleeDamage); // ← Setup 関数を作る
        }
        // 攻撃後に元の画像に戻す（
        StartCoroutine(ResetSprite(0.2f));
    }

    void AttackRanged()
    {
        if (bulletPrefab != null && firePoint != null && player != null)
        {
            // 画像切替
            spriteRenderer.sprite = attackSprite;
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
            // 攻撃後に元の画像に戻す
            StartCoroutine(ResetSprite(0.4f));
        }

    }

    IEnumerator ResetSprite(float delay)
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.sprite = idleSprite;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangedRange);
    }

    void Flip()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;

        if (player.position.x > transform.position.x)
            scale.x = -Mathf.Abs(scale.x);  // プレイヤーが右 → 右向き
        else
            scale.x = Mathf.Abs(scale.x); // プレイヤーが左 → 左向き

        transform.localScale = scale;
    }
}
