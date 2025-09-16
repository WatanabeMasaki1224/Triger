using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class BaseController : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;
    [Header("HPバー")]
    public Image hpFillImage; // 緑のゲージImage
    public Text hpText;       // HP数値表示用Text

    void Start()
    {
        currentHP = maxHP;
        UpdateHPUI();
        if (hpFillImage != null)
        {
            hpFillImage.color = Color.green; // 緑にする
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
        Debug.Log("拠点破壊された");
        if (GameManager.Instance != null) 
        { 
            GameManager.Instance.ReduceScore(100); 
        }
        // リザルト画面に移動
        int finalScore = GameManager.Instance != null ? GameManager.Instance.GetScore() : 0;
        ScoreManager.Instance.AddScore(finalScore); // ランキングに追加
        UnityEngine.SceneManagement.SceneManager.LoadScene("claer"); // シーン名を正しいものに
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
            hpText.text = "拠点:"　+　currentHP ;  // HP数値を表示

    }
}
