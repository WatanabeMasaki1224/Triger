using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [Header("�����L���O�\���p")]
    public List<TMP_Text> scoreTexts; // �����L���O�p�̃e�L�X�g�i10�j
    public TMP_Text myScoreText; // �����̃X�R�A�\��
    public GameObject titleButton; // �^�C�g���ɖ߂�{�^��

    void Start()
    {
        // �X�R�A�ꗗ���擾
        List<int> scores = ScoreManager.Instance.GetScores();

        int myScore = GameManager.Instance.GetScore();
        bool myScoreDisplayed = false; // �����̃X�R�A���\�����ꂽ��

        // �����L���O�\��
        for (int i = 0; i < scoreTexts.Count; i++)
        {
            if (i < scores.Count)
            {
                scoreTexts[i].text = $"No.{i + 1}:{scores[i]}";

                // �����̃X�R�A�Ȃ���F�ɂ���
                if (scores[i] == myScore && !myScoreDisplayed)
                {
                    myScoreDisplayed = true;

                    // ���F�O���f�[�V����
                    var rainbow = new VertexGradient(
                        Color.red,    // ����
                        Color.yellow, // �E��
                        Color.green,  // ����
                        Color.blue    // �E��
                    );
                    scoreTexts[i].enableVertexGradient = true; // �� �����ǉ�
                    scoreTexts[i].colorGradient = rainbow;
                }
                else
                {
                    // �ʏ�F�i���j�ɖ߂�
                    scoreTexts[i].color = Color.white;
                }
            }
            else
            {
                scoreTexts[i].text = $"No.{i + 1}: ---";
                scoreTexts[i].color = Color.white;
            }
        }

        // �����̃X�R�A��ʕ\���i�K�v�Ȃ�j
        myScoreText.text = $"Your Score: {myScore}";
    }

    public void OnClickTitleButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
