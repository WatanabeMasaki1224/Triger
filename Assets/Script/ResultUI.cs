using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [Header("�����L���O�\���p")]
    public List<TMP_Text> scoreTexts; // �����L���O�p�̃e�L�X�g�i10�j
    public TMP_Text myScoreText; // �����̃X�R�A�\��
    public GameObject titleButton; // �^�C�g���ɖ߂�{�^��
    public GameObject scorePanel; // �g�p�l��
    [Header("���o�p")]
    public TMP_Text mainMessageText;       // �����̃��b�Z�[�W
    public GameObject gameClearEffect;     // �p�[�e�B�N���Ȃ�
    public AudioSource audioSource;        // �t�@���t�@�[����v�ZSE�p
    public AudioClip fanfareClip;
   
    public GameObject nextButton;          // GAME CLEAR��ɕ\��
    public float scoreCountUpSpeed = 500f; // 1�b������̃X�R�A������


    void Start()
    {
        // �ŏ��͉��o�pUI���\��
        mainMessageText.gameObject.SetActive(false);
        nextButton.SetActive(false);
        titleButton.SetActive(false);
        if (myScoreText != null) myScoreText.gameObject.SetActive(false);
        if (scorePanel != null) scorePanel.SetActive(false);
        foreach (var t in scoreTexts)
            t.gameObject.SetActive(false);

        // �Q�[���N���A���o���J�n
        StartCoroutine(GameClearSequence());
    }

    IEnumerator GameClearSequence()
    {
        // 1. GAME CLEAR�\��
        mainMessageText.text = "GAME CLEAR!!";
        mainMessageText.gameObject.SetActive(true);

        if (gameClearEffect != null) gameClearEffect.SetActive(true);
        if (audioSource != null && fanfareClip != null) audioSource.PlayOneShot(fanfareClip);

        // �����҂��Ă���u���ցv�{�^����\��
        yield return new WaitForSeconds(1.5f);
        nextButton.SetActive(true);
    }

    // ���փ{�^��������
    public void OnClickNextButton()
    {
        nextButton.SetActive(false);
        mainMessageText.gameObject.SetActive(false);
        if (gameClearEffect != null) gameClearEffect.SetActive(false);

        StartCoroutine(ScoreCalculationSequence());
    }

    IEnumerator ScoreCalculationSequence()
    {
        // �X�R�A�p�l���\��
        if (scorePanel != null) scorePanel.SetActive(true);
        if (myScoreText != null) myScoreText.gameObject.SetActive(true);

        int finalScore = GameManager.Instance.GetScore();
        int displayedScore = 0;

        // 0����J�E���g�A�b�v
        while (displayedScore < finalScore)
        {
            displayedScore += Mathf.CeilToInt(scoreCountUpSpeed * Time.deltaTime);
            if (displayedScore > finalScore) displayedScore = finalScore;
            myScoreText.text = $"Your Score: {displayedScore}";
            yield return null;
        }

        // �J�E���g�A�b�v������A�����L���O�\��
        DisplayRanking();
    }
    void DisplayRanking()
    {
        // �X�R�A�ꗗ���擾
        List<int> scores = ScoreManager.Instance.GetScores();

        int myScore = GameManager.Instance.GetScore();
        bool myScoreDisplayed = false; // �����̃X�R�A���\�����ꂽ��
        // �����L���O�\��
        for (int i = 0; i < scoreTexts.Count; i++)
        {
            scoreTexts[i].gameObject.SetActive(true);
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
        titleButton.SetActive(true);
    }

    public void OnClickTitleButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
