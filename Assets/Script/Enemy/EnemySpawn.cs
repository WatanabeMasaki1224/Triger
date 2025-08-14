using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public GameObject enemyPrefab;
    public int enemyCount = 5;
    public float spawnCooltime = 5f;
    [HideInInspector] public float timer = 0f; // 内部用タイマー
}

public class EnemySpawn : MonoBehaviour
{
    [Header("出減ポイント")]
    public Transform[] spawnPoints;
    [Header("敵の種類")]
    public List<EnemyData> enemyTypes;
    [Header("全体の最大同時出現数")]
    public int maxTotalEnemies = 20;


    void Update()
    {
        int totalEnemies = 0;
        foreach (var enemyType in enemyTypes)
        {
            totalEnemies += GameObject.FindGameObjectsWithTag(enemyType.enemyPrefab.tag).Length;
        }

        // 全体最大数に達していればスポーンさせない
        if (totalEnemies >= maxTotalEnemies) return;

        foreach (var enemyType in enemyTypes)
        {
            int currentCount = GameObject.FindGameObjectsWithTag(enemyType.enemyPrefab.tag).Length;
            if (currentCount < enemyType.enemyCount)
            {
                enemyType.timer -= Time.deltaTime;

                if (enemyType.timer <= 0f)
                {
                    SpawnEnemy(enemyType);
                    enemyType.timer = enemyType.spawnCooltime;
                }
            }
        }
        
    }

    void SpawnEnemy(EnemyData enemyType)
    {
        if (spawnPoints.Length == 0 || enemyType.enemyPrefab == null) return;

        // ランダム出現位置
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(enemyType.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

}
