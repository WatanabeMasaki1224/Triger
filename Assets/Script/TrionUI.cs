using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrionUI : MonoBehaviour
{
    [SerializeField] private Image fillNormal; // 緑
    [SerializeField] private Image fillExtra;  // 青
    [SerializeField] private Image fillDamage; // 赤
    [SerializeField] private Text trionText;

    [SerializeField] private PlayerController player; // プレイヤー参照

    void Start()
    {
        // 白画像に色を乗算してゲージ色を設定
        fillNormal.color = Color.green;  // 緑
        fillExtra.color = Color.blue;    // 青
        fillDamage.color = Color.red;    // 赤
    }

    void Update()
    {
        if (player == null) return;

        int current = player.GetCurrentTrion();
        int max = player.maxTrion;

        // 数値更新
        trionText.text = "エネルギー:" + current;

        // 現在トリオン比
        float ratioNormal = Mathf.Clamp01((float)current / max);

        // 超過分
        float ratioExtra = current > max ? (float)(current - max) / max : 0f;

        // 赤は背景なので fillAmount は変えない
        fillDamage.fillAmount = 1f;

        // 緑ゲージ
        fillNormal.fillAmount = ratioNormal;

        // 青ゲージ
        fillExtra.fillAmount = ratioExtra;
    }
}
