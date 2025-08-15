using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("�ړ�")]
    public float moveSpeed = 5f;
    [Header("�g���I���ݒ�")]
    public int maxTrion = 100;
    private int currentTrion;
    [Header("�g���K�[�ݒ�")]
    public MainTrigger[] mainTriggers;
    private int currentMainIndex = 0;
    public SubTrigger[] subTriggers;
    private int currentSubIndex = 0;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lookDirection = Vector2.right; //�����͉E����

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

        // ���킲�Ƃ̃^�C�}�[�X�V
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

        // �ړ����͂�����ꍇ�̂݌������X�V
        if (moveInput != Vector2.zero)
        {
            lookDirection = GetEightDirection(moveInput);
        }
    }

    Vector2 GetEightDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // 0~360�ɕϊ�
        if (angle < 0) angle += 360f;

        // 8�������ƂɃX�i�b�v
        if (angle >= 337.5f || angle < 22.5f) return Vector2.right;
        if (angle >= 22.5f && angle < 67.5f) return new Vector2(1, 1).normalized;
        if (angle >= 67.5f && angle < 112.5f) return Vector2.up;
        if (angle >= 112.5f && angle < 157.5f) return new Vector2(-1, 1).normalized;
        if (angle >= 157.5f && angle < 202.5f) return Vector2.left;
        if (angle >= 202.5f && angle < 247.5f) return new Vector2(-1, -1).normalized;
        if (angle >= 247.5f && angle < 292.5f) return Vector2.down;
        if (angle >= 292.5f && angle < 337.5f) return new Vector2(1, -1).normalized;

        return Vector2.right; // �f�t�H���g
    }


    void AttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mainTriggers.Length > 0 && currentTrion >= mainTriggers[currentMainIndex].trionCost)
            {
                Vector3 direction = lookDirection; // �L�����̌����ɉ����Ĕ���

                // ���ˏ���
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
            // �T�u�g���K�[UI�X�V����
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
        //�Q�[���I����ʂɈڍs
    }

    public int GetCurrentTrion()
    {
        return currentTrion;
    }

}
