using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrionUI : MonoBehaviour
{
    [SerializeField] private Image fillNormal; // ��
    [SerializeField] private Image fillExtra;  // ��
    [SerializeField] private Image fillDamage; // ��
    [SerializeField] private Text trionText;

    [SerializeField] private PlayerController player; // �v���C���[�Q��

    void Start()
    {
        // ���摜�ɐF����Z���ăQ�[�W�F��ݒ�
        fillNormal.color = Color.green;  // ��
        fillExtra.color = Color.blue;    // ��
        fillDamage.color = Color.red;    // ��
    }

    void Update()
    {
        if (player == null) return;

        int current = player.GetCurrentTrion();
        int max = player.maxTrion;

        // ���l�X�V
        trionText.text = "�G�l���M�[:" + current;

        // ���݃g���I����
        float ratioNormal = Mathf.Clamp01((float)current / max);

        // ���ߕ�
        float ratioExtra = current > max ? (float)(current - max) / max : 0f;

        // �Ԃ͔w�i�Ȃ̂� fillAmount �͕ς��Ȃ�
        fillDamage.fillAmount = 1f;

        // �΃Q�[�W
        fillNormal.fillAmount = ratioNormal;

        // �Q�[�W
        fillExtra.fillAmount = ratioExtra;
    }
}
