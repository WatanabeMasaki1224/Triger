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
            scoreText.text = "スコア" + GameManager.Instance.GetScore();
        }

        if(player != null)
        {
            trionText.text = "トリオン" + player.GetCurrentTrion();

        }
    }
}
