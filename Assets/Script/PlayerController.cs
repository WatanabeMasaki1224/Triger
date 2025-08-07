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
    }

    void AttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mainTriggers.Length > 0 && currentTrion >= mainTriggers[currentMainIndex].trionCost)
            {
                // マウスの位置をワールド座標で取得
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0f; // Z座標を0に揃える（2Dゲームで重要）

                // プレイヤーの位置からマウス方向へのベクトルを計算
                Vector3 direction = (mousePos - transform.position).normalized;

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

}
