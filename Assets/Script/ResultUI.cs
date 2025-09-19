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
    public AudioClip countUpClip;          // スコア加算の音
    public AudioClip finishClip;           // カウントアップ終了音

    public GameObject nextButton;          // GAME CLEAR後に表示
    public float scoreCountUpSpeed = 500f; // 1秒あたりのスコア増加量
    public float soundInterval = 0.05f; // 効果音を鳴らす間隔


    void Start()
    {
        // 最初は演出用UIを非表示
        mainMessageText.gameObject.SetActive(false);
        nextButton.SetActive(false);
        titleButton.SetActive(false);
        if (myScoreText != null) myScoreText.gameObject.SetActive(false);
        if (scorePanel != null) scorePanel.SetActive(false);
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
        // 最初は小さく
        mainMessageText.transform.localScale = Vector3.zero;
        // アニメーションで拡大
        float duration = 0.6f; // 文字が大きくなる時間
        float elapsed = 0f;
        Vector3 targetScale = Vector3.one; // 通常サイズ（1,1,1）

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // イージングをかけて「バーン！」っぽく
            float scale = Mathf.Sin(t * Mathf.PI * 0.5f);
            mainMessageText.transform.localScale = Vector3.one * scale;

            yield return null;
        }

        // 最終的にピッタリ通常サイズに
        mainMessageText.transform.localScale = targetScale;

        if (gameClearEffect != null)
        {
            gameClearEffect.SetActive(true);

            // ParticleSystemを取得して再生
            var ps = gameClearEffect.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Play();
        }
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
        // スコアパネル表示
        if (scorePanel != null) scorePanel.SetActive(true);
        if (myScoreText != null) myScoreText.gameObject.SetActive(true);

        int finalScore = GameManager.Instance.GetScore();
        int displayedScore = 0;
        float duration = 2.0f; 
        float elapsed = 0f;
        float soundTimer = 0f;
        // 0からカウントアップ
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // スコアを補間して計算（0 → finalScore）
            displayedScore = Mathf.RoundToInt(Mathf.Lerp(0, finalScore, t));
            myScoreText.text = $"Your Score: {displayedScore}";

            // 🔊 ピッ音（一定間隔で再生）
            soundTimer += Time.deltaTime;
            if (soundTimer >= soundInterval && audioSource != null && countUpClip != null)
            {
                soundTimer = 0f;
                audioSource.pitch = 1f + t; // 徐々に高く
                audioSource.PlayOneShot(countUpClip);
            }

            yield return null;
        }

        // 最終値を保証
        myScoreText.text = $"Your Score: {finalScore}";

        // カウントアップ終了音
        if (audioSource != null && finishClip != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(finishClip);
        }

        // カウントアップ完了後、ランキング表示
        DisplayRanking();
    }
    void DisplayRanking()
        {
            // スコア一覧を取得
            List<int> scores = ScoreManager.Instance.GetScores();

            int myScore = GameManager.Instance.GetScore();
            bool myScoreDisplayed = false; // 自分のスコアが表示されたか
                                           // ランキング表示
            for (int i = 0; i < scoreTexts.Count; i++)
            {
                scoreTexts[i].gameObject.SetActive(true);
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
            titleButton.SetActive(true);
        }

    public void OnClickTitleButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
