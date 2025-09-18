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
    public AudioClip calculationClip;
    public AudioClip calculationDoneClip;

    public GameObject nextButton;          // GAME CLEAR��ɕ\��
    public TMP_Text scoreCalculationText;  // �u�X�R�A�v�Z���c�v

    void Start()
    {
        // �ŏ��͉��o�pUI���\��
        mainMessageText.gameObject.SetActive(false);
        nextButton.SetActive(false);
        scoreCalculationText.gameObject.SetActive(false);
        titleButton.SetActive(false);
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
        // 2. �X�R�A�v�Z���o
        scoreCalculationText.text = "�X�R�A�v�Z���c";
        scoreCalculationText.gameObject.SetActive(true);

        float calcTime = 2.5f; // �v�Z����
        float timer = 0f;

        while (timer < calcTime)
        {
            timer += Time.deltaTime;
            // �����_���œd�q��
            if (Random.value < 0.1f && audioSource != null && calculationClip != null)
            {
                audioSource.PlayOneShot(calculationClip);
            }
            yield return null;
        }

        // �v�Z�I����
        if (audioSource != null && calculationDoneClip != null)
            audioSource.PlayOneShot(calculationDoneClip);

        scoreCalculationText.text = "�v�Z�I���I";
        yield return new WaitForSeconds(0.5f);
        scoreCalculationText.gameObject.SetActive(false);

        // 3. ���U���g�\��
        DisplayScore();
    }
    void DisplayScore()
    {
        // �g�p�l����\��
        if (scorePanel != null) scorePanel.SetActive(true);

        // �����̃X�R�A���\��
        if (myScoreText != null) myScoreText.gameObject.SetActive(true);
        // �X�R�A�ꗗ���擾
        List<int> scores = ScoreManager.Instance.GetScores();

        int myScore = GameManager.Instance.GetScore();
        bool myScoreDisplayed = false; // �����̃X�R�A���\�����ꂽ��
        // �����L���O�p�e�L�X�g�����ׂėL����
        for (int i = 0; i < scoreTexts.Count; i++)
        {
            scoreTexts[i].gameObject.SetActive(true);
        }
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
        titleButton.SetActive(true);
    }

    public void OnClickTitleButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
