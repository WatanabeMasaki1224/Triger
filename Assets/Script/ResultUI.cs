using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [Header("ランキング表示用")]
    public List<TMP_Text> scoreTexts; // ランキング用のテキスト（10個）
    public TMP_Text myScoreText; // 自分のスコア表示
    public GameObject titleButton; // タイトルに戻るボタン

    void Start()
    {
        // スコア一覧を取得
        List<int> scores = ScoreManager.Instance.GetScores();

        int myScore = GameManager.Instance.GetScore();
        bool myScoreDisplayed = false; // 自分のスコアが表示されたか

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
    }

    public void OnClickTitleButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
