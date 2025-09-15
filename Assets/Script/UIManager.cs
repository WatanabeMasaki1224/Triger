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
            // 5���\���B0���߂ŕ\��
            int score = GameManager.Instance.GetScore();
            scoreText.text = "�X�R�A:" + score.ToString("D5");
        }

        /*if(player != null)
        {
            trionText.text = "�g���I��" + player.GetCurrentTrion();

        }*/
    }
}
