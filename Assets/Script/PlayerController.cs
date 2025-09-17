using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("移動")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lookDirection = Vector2.down; // 初期は下向き
    [Header("トリオン設定")]
    public int maxTrion = 100;
    private int currentTrion;
    [Header("トリガー設定")]
    public MainTrigger[] mainTriggers;
    private int currentMainIndex = 0;
    public SubTrigger[] subTriggers;
    private int currentSubIndex = 0;
    private bool isAttacking = false; //攻撃中かの判定
    private Animator animator;
    [Header("効果音クリップ")]
    public AudioClip slashSound;   // 剣攻撃用
    public AudioClip bulletSound;  // 弾攻撃用
    public AudioClip hitSound;     // 被弾時
    public AudioClip cureSound;    // 回復時

    [Header("効果音ソース")]
    public AudioSource slashSource;
    public AudioSource bulletSource;
    public AudioSource hitSource;
    public AudioSource cureSource;

    private SpriteRenderer sr;
    public float hitFlash = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentTrion = maxTrion;
        sr = GetComponent<SpriteRenderer>();
        if (slashSource == null) slashSource = gameObject.AddComponent<AudioSource>();
        if (bulletSource == null) bulletSource = gameObject.AddComponent<AudioSource>();
        if (hitSource == null) hitSource = gameObject.AddComponent<AudioSource>();
        if (cureSource == null) cureSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (currentTrion <= 0)
        {
            Die();
        }

        MovementInput();
        AttackInput();
        TriggerSwitch();

        // 武器ごとのタイマー更新
        foreach (var trigger in mainTriggers)
        {
            trigger.UpdateTimer(Time.deltaTime);
        }

    }


    void MovementInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v);

        // 移動しているか
        bool isMoving = moveInput != Vector2.zero;
        animator.SetBool("isMoving", isMoving);
        // 移動中のみ lookDirection を更新
        if (isMoving)
        {
            lookDirection = moveInput.normalized;
        }

        // BlendTree に方向を渡す
        animator.SetFloat("moveX", lookDirection.x);
        animator.SetFloat("moveY", lookDirection.y);

    }

   
     Vector2 GetEightDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // 0~360に変換
        if (angle < 0) angle += 360f;

        // 8方向ごとにスナップ
        if (angle >= 337.5f || angle < 22.5f) return Vector2.right;
        if (angle >= 22.5f && angle < 67.5f) return new Vector2(1, 1).normalized;
        if (angle >= 67.5f && angle < 112.5f) return Vector2.up;
        if (angle >= 112.5f && angle < 157.5f) return new Vector2(-1, 1).normalized;
        if (angle >= 157.5f && angle < 202.5f) return Vector2.left;
        if (angle >= 202.5f && angle < 247.5f) return new Vector2(-1, -1).normalized;
        if (angle >= 247.5f && angle < 292.5f) return Vector2.down;
        if (angle >= 292.5f && angle < 337.5f) return new Vector2(1, -1).normalized;

        return Vector2.right; // デフォルト
    }


    void AttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var currentTrigger = mainTriggers[currentMainIndex];

            // トリオンとクールタイムの両方をチェック
            if (currentTrion >= currentTrigger.trionCost && currentTrigger.CanUse())
            {
                isAttacking = true; // 攻撃中フラグON
                Vector3 direction = lookDirection;

                currentTrigger.Use(transform.position, direction);
                currentTrion -= currentTrigger.trionCost;
                //効果音の分岐
                if (currentTrigger.type == TriggerType.Slash)
                {
                    if (slashSound != null) slashSource.PlayOneShot(slashSound);
                }
                else // 弾攻撃
                {
                    if (bulletSound != null) bulletSource.PlayOneShot(bulletSound);
                }

                // 攻撃中フラグを戻す時間を設定
                float attackDuration = 0.5f; // 弾も剣も共通で短時間停止
                StartCoroutine(ResetAttackFlag(attackDuration));
            }
            else
            {
                Debug.Log("攻撃はまだ使用できません（クールタイム中またはトリオン不足）");
            }
        }
    }

    void TriggerSwitch()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            currentMainIndex = (currentMainIndex + 1) % mainTriggers.Length;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {   
            currentSubIndex = (currentSubIndex + 1) % subTriggers.Length;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentTrion <= 0) return; // すでに死んでいる場合は何もしない
        currentTrion -= damage;
        Debug.Log(currentTrion);   
        
        if(hitSound != null)
        {
            hitSource.PlayOneShot(hitSound);
        }

        if(sr != null)
        {
            StartCoroutine(FlashRed());
        }
    }

    private void Die()
    {
        // リザルト画面へ
        int finalScore = GameManager.Instance.GetScore(); //現在のスコア所得
        ScoreManager.Instance.AddScore(finalScore);　//ランキングに追加
        SceneManager.LoadScene("claer");
    }

    private IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
    }

    public int GetCurrentTrion()
    {
        return currentTrion;
    }

    public void CureTrion(int amount)
    {
        currentTrion += amount;
        /*if (currentTrion > maxTrion)
        {
           currentTrion = maxTrion;
        }*/

        if (cureSound != null)
        {
            cureSource.PlayOneShot(cureSound);
        }
    }

    IEnumerator ResetAttackFlag(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }

    void FixedUpdate()
    {
        if (!isAttacking)
            rb.velocity = moveInput * moveSpeed;
        else
            rb.velocity = Vector2.zero;
    }
}
