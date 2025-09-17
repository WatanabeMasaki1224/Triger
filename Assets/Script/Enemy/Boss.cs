using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
    [Header("�^�[�Q�b�g")]
    public Transform player;

    [Header("�U������")]
    public GameObject slashPrefab;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int meleeDamage = 10;
    public int rangedDamage = 5;
    public float meleeRange = 2f;
    public float rangedRange = 6f;
    [Header("�e�ݒ�")]
    public float bulletSpeed = 5f;

    [Header("�N�[���^�C��")]
    public float meleeCoolTime = 1.5f;
    public float rangedCoolTime = 2f;
    private float meleeTimer = 0f;
    private float rangedTimer = 0f;

    [Header("�摜�ݒ�")]
    public Sprite idleSprite;   // �ҋ@
    public Sprite attackSprite; // �U��
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
        // �^�C�}�[�X�V
        meleeTimer -= Time.deltaTime;
        rangedTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        // �ߋ����U��
        if (distance <= meleeRange && meleeTimer <= 0f)
        {
            AttackMelee();
            meleeTimer = meleeCoolTime;
        }
        // �������U��
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
            slashScript.Setup(dir, meleeDamage); // �� Setup �֐������
        }
        // �U����Ɍ��̉摜�ɖ߂��i
        StartCoroutine(ResetSprite(0.2f));
    }

    void AttackRanged()
    {
        if (bulletPrefab != null && firePoint != null && player != null)
        {
            // �摜�ؑ�
            spriteRenderer.sprite = attackSprite;
            // �v���C���[�̕������v�Z
            Vector3 dir = (player.position - firePoint.position).normalized;
            // �p�x�v�Z
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // �e�𐶐�
            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0,0,angle));
            // EnemyBullet �X�N���v�g���擾���ĕ�����ݒ�
            BossBullet bullet = bulletObj.GetComponent<BossBullet>();
            if (bullet != null)
            {
                bullet.Setup(dir);
            }
            // �U����Ɍ��̉摜�ɖ߂�
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
            scale.x = -Mathf.Abs(scale.x);  // �v���C���[���E �� �E����
        else
            scale.x = Mathf.Abs(scale.x); // �v���C���[���� �� ������

        transform.localScale = scale;
    }
}
