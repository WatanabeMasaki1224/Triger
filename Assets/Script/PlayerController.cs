using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("�ړ�")]
    public float moveSpeed = 5f;
    [Header("�g���I���ݒ�")]
    public int maxTrion = 100;
    private int currentTrion;
    [Header("�X�v���C�g�ݒ�")]
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    private SpriteRenderer spriteRenderer;
    [Header("�g���K�[�ݒ�")]
    public MainTrigger[] mainTriggers;
    private int currentMainIndex = 0;
    public SubTrigger[] subTriggers;
    private int currentSubIndex = 0;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lookDirection = Vector2.right; //�����͉E����
    private bool isAttacking = false; //�U�������̔���

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        currentTrion = maxTrion;
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
        SpriteDirection();

        // ���킲�Ƃ̃^�C�}�[�X�V
        foreach (var trigger in mainTriggers)
        {
            trigger.UpdateTimer(Time.deltaTime);
        }

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

    void SpriteDirection()
    {
        if(Mathf.Abs(lookDirection.x) > Mathf.Abs(lookDirection.y))
        {
            if(lookDirection.x > 0)
            {
                spriteRenderer.sprite = spriteRight;
            }
            else
            {
                spriteRenderer.sprite = spriteLeft;
            }
        }
        else
        {
            if(lookDirection.y > 0)
            {
                spriteRenderer.sprite = spriteUp;
            }
            else
            {
                spriteRenderer.sprite = spriteDown;
            }
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
            var currentTrigger = mainTriggers[currentMainIndex];

            // �g���I���ƃN�[���^�C���̗������`�F�b�N
            if (currentTrion >= currentTrigger.trionCost && currentTrigger.CanUse())
            {
                isAttacking = true; // �U�����t���OON
                Vector3 direction = lookDirection;

                currentTrigger.Use(transform.position, direction);
                currentTrion -= currentTrigger.trionCost;

                // �U�����t���O��߂����Ԃ�ݒ�
                float attackDuration = 0.5f; // �e���������ʂŒZ���Ԓ�~
                StartCoroutine(ResetAttackFlag(attackDuration));
            }
            else
            {
                Debug.Log("�U���͂܂��g�p�ł��܂���i�N�[���^�C�����܂��̓g���I���s���j");
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
        if (currentTrion <= 0) return; // ���łɎ���ł���ꍇ�͉������Ȃ�
        currentTrion -= damage;
        Debug.Log(currentTrion);    
    }

    private void Die()
    {
        // ���U���g��ʂ�
        int finalScore = GameManager.Instance.GetScore(); //���݂̃X�R�A����
        ScoreManager.Instance.AddScore(finalScore);�@//�����L���O�ɒǉ�
        SceneManager.LoadScene("claer");
    }


    public int GetCurrentTrion()
    {
        return currentTrion;
    }

    public void CureTrion(int amount)
    {
        currentTrion += amount;
        if (currentTrion > maxTrion)
        {
           currentTrion = maxTrion;
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
