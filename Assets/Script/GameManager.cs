using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } //ƒVƒ“ƒOƒ‹ƒgƒ“

    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("score:" + score);
    }

    public void ReduceScore(int amount)
    {
        score -= amount;
        Debug.Log("Score;" + score);
    }

    public int GetScore()
    {
        return score;
    }
}
