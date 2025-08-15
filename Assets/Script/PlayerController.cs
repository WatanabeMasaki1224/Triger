using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("移動")]
    public float moveSpeed = 5f;
    [Header("トリオン設定")]
    public int maxTrion = 100;
    private int currentTrion;
    [Header("トリガー設定")]
    public MainTrigger[] mainTriggers;
    private int currentMainIndex = 0;
    public SubTrigger[] subTriggers;
    private int currentSubIndex = 0;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lookDirection = Vector2.right; //初期は右向き

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTrion = maxTrion;
    }

    void Update()
    {
        MovementInput();
        AttackInput();
        TriggerSwitch();

        // 武器ごとのタイマー更新
        foreach (var trigger in mainTriggers)
        {
            trigger.UpdateTimer(Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    void MovementInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;

        // 移動入力がある場合のみ向きを更新
        if (moveInput != Vector2.zero)
        {
            lookDirection = GetEightDirection(moveInput);
        }
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
            if (mainTriggers.Length > 0 && currentTrion >= mainTriggers[currentMainIndex].trionCost)
            {
                Vector3 direction = lookDirection; // キャラの向きに沿って発射

                // 発射処理
                mainTriggers[currentMainIndex].Use(transform.position, direction);
                currentTrion -= mainTriggers[currentMainIndex].trionCost;
            }
        }

        /*if (Input.GetMouseButtonDown(1))
        {
            if(subTriggers.Length > 0 && currentTrion >= subTriggers[currentSubIndex].trionCost)
            {
                subTriggers[currentSubIndex].Use(transform.position);
                currentTrion -= subTriggers[currentSubIndex].trionCost;
            }
        }*/
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
            // サブトリガーUI更新処理
        }
    }

    public void TakeDamage(int damage)
    {
        currentTrion -= damage;
        Debug.Log(currentTrion);
        if (currentTrion <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        //ゲーム終了画面に移行
    }

    public int GetCurrentTrion()
    {
        return currentTrion;
    }

}
