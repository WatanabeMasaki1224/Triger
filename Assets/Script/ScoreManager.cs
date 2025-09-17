using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public List<int> scores = new List<int>();
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private const string SCORE_KEY = "HighScores";
    private ScoreData scoreData;

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
            return;
        }
        LoadScores();
    }

    public void AddScore(int newScore)
    {
        scoreData.scores.Add(newScore);
        scoreData.scores.Sort((a, b) => b.CompareTo(a)); // ç~èáÇ…É\Å[Ég

        if (scoreData.scores.Count > 10) // è„à 10åèÇæÇØï€éù
        {
            scoreData.scores.RemoveAt(scoreData.scores.Count - 1);
        }

        SaveScores();
    }

    public List<int> GetScores()
    {
        return scoreData.scores;
    }

    private void SaveScores()
    {
        string json = JsonUtility.ToJson(scoreData);
        PlayerPrefs.SetString(SCORE_KEY, json);
        PlayerPrefs.Save();
    }

    private void LoadScores()
    {
        if (PlayerPrefs.HasKey(SCORE_KEY))
        {
            string json = PlayerPrefs.GetString(SCORE_KEY);
            scoreData = JsonUtility.FromJson<ScoreData>(json);
        }
        else
        {
            scoreData = new ScoreData();
        }
    }
}
