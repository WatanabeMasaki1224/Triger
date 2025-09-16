using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class BaseController : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;
    [Header("HP�o�[")]
    public Image hpFillImage; // �΂̃Q�[�WImage
    public Text hpText;       // HP���l�\���pText

    void Start()
    {
        currentHP = maxHP;
        UpdateHPUI();
        if (hpFillImage != null)
        {
            hpFillImage.color = Color.green; // �΂ɂ���
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log(currentHP);
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPUI(); 
        if (currentHP <= 0)
        {
            Destroyed();
        }

    }

    void Destroyed()
    {
        Debug.Log("���_�j�󂳂ꂽ");
        if (GameManager.Instance != null) 
        { 
            GameManager.Instance.ReduceScore(100); 
        }
        // ���U���g��ʂɈړ�
        int finalScore = GameManager.Instance != null ? GameManager.Instance.GetScore() : 0;
        ScoreManager.Instance.AddScore(finalScore); // �����L���O�ɒǉ�
        UnityEngine.SceneManagement.SceneManager.LoadScene("claer"); // �V�[�����𐳂������̂�
        Destroy(gameObject);
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

    private void UpdateHPUI()
    {
        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = (float)currentHP / maxHP;
        }
        if (hpText != null)
            hpText.text = "���_:"�@+�@currentHP ;  // HP���l��\��

    }
}
