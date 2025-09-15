using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text trionText;
    public PlayerController player;

   
    void Update()
    {
        if(GameManager.Instance != null)
        {
            // 5桁表示。0埋めで表示
            int score = GameManager.Instance.GetScore();
            scoreText.text = "スコア:" + score.ToString("D5");
        }

        /*if(player != null)
        {
            trionText.text = "トリオン" + player.GetCurrentTrion();

        }*/
    }
}
