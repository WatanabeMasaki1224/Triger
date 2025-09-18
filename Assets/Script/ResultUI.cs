using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [Header("ランキング表示用")]
    public List<TMP_Text> scoreTexts; // ランキング用のテキスト（10個）
    public TMP_Text myScoreText; // 自分のスコア表示
    public GameObject titleButton; // タイトルに戻るボタン
    public GameObject scorePanel; // 枠パネル
    [Header("演出用")]
    public TMP_Text mainMessageText;       // 中央のメッセージ
    public GameObject gameClearEffect;     // パーティクルなど
    public AudioSource audioSource;        // ファンファーレや計算SE用
    public AudioClip fanfareClip;
    public AudioClip calculationClip;
    public AudioClip calculationDoneClip;

    public GameObject nextButton;          // GAME CLEAR後に表示
    public TMP_Text scoreCalculationText;  // 「スコア計算中…」

    void Start()
    {
        // 最初は演出用UIを非表示
        mainMessageText.gameObject.SetActive(false);
        nextButton.SetActive(false);
        scoreCalculationText.gameObject.SetActive(false);
        titleButton.SetActive(false);
        foreach (var t in scoreTexts)
            t.gameObject.SetActive(false);

        // ゲームクリア演出を開始
        StartCoroutine(GameClearSequence());
    }

    IEnumerator GameClearSequence()
    {
        // 1. GAME CLEAR表示
        mainMessageText.text = "GAME CLEAR!!";
        mainMessageText.gameObject.SetActive(true);

        if (gameClearEffect != null) gameClearEffect.SetActive(true);
        if (audioSource != null && fanfareClip != null) audioSource.PlayOneShot(fanfareClip);

        // 少し待ってから「次へ」ボタンを表示
        yield return new WaitForSeconds(1.5f);
        nextButton.SetActive(true);
    }

    // 次へボタン押下時
    public void OnClickNextButton()
    {
        nextButton.SetActive(false);
        mainMessageText.gameObject.SetActive(false);
        if (gameClearEffect != null) gameClearEffect.SetActive(false);

        StartCoroutine(ScoreCalculationSequence());
    }

    IEnumerator ScoreCalculationSequence()
    {
        // 2. スコア計算演出
        scoreCalculationText.text = "スコア計算中…";
        scoreCalculationText.gameObject.SetActive(true);

        float calcTime = 2.5f; // 計算時間
        float timer = 0f;

        while (timer < calcTime)
        {
            timer += Time.deltaTime;
            // ランダムで電子音
            if (Random.value < 0.1f && audioSource != null && calculationClip != null)
            {
                audioSource.PlayOneShot(calculationClip);
            }
            yield return null;
        }

        // 計算終了音
        if (audioSource != null && calculationDoneClip != null)
            audioSource.PlayOneShot(calculationDoneClip);

        scoreCalculationText.text = "計算終了！";
        yield return new WaitForSeconds(0.5f);
        scoreCalculationText.gameObject.SetActive(false);

        // 3. リザルト表示
        DisplayScore();
    }
    void DisplayScore()
    {
        // 枠パネルを表示
        if (scorePanel != null) scorePanel.SetActive(true);

        // 自分のスコアも表示
        if (myScoreText != null) myScoreText.gameObject.SetActive(true);
        // スコア一覧を取得
        List<int> scores = ScoreManager.Instance.GetScores();

        int myScore = GameManager.Instance.GetScore();
        bool myScoreDisplayed = false; // 自分のスコアが表示されたか
        // ランキング用テキストをすべて有効化
        for (int i = 0; i < scoreTexts.Count; i++)
        {
            scoreTexts[i].gameObject.SetActive(true);
        }
        // ランキング表示
        for (int i = 0; i < scoreTexts.Count; i++)
        {
            if (i < scores.Count)
            {
                scoreTexts[i].text = $"No.{i + 1}:{scores[i]}";

                // 自分のスコアなら虹色にする
                if (scores[i] == myScore && !myScoreDisplayed)
                {
                    myScoreDisplayed = true;

                    // 虹色グラデーション
                    var rainbow = new VertexGradient(
                        Color.red,    // 左上
                        Color.yellow, // 右上
                        Color.green,  // 左下
                        Color.blue    // 右下
                    );
                    scoreTexts[i].enableVertexGradient = true; // ← これを追加
                    scoreTexts[i].colorGradient = rainbow;
                }
                else
                {
                    // 通常色（白）に戻す
                    scoreTexts[i].color = Color.white;
                }
            }
            else
            {
                scoreTexts[i].text = $"No.{i + 1}: ---";
                scoreTexts[i].color = Color.white;
            }
        }

        // 自分のスコアを別表示（必要なら）
        myScoreText.text = $"Your Score: {myScore}";
        titleButton.SetActive(true);
    }

    public void OnClickTitleButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
