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
        // 全敵の合計を一度だけ数える（Enemyタグのオブジェクト数）
        int totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        // 全体最大数に達していればスポーンさせない
        if (totalEnemies >= maxTotalEnemies) return;

        foreach (var enemyType in enemyTypes)
        {
            int currentCount = CountEnemyOfType(enemyType.enemyPrefab);
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

    int CountEnemyOfType(GameObject prefab)
    {
        int count = 0;
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.name.Contains(prefab.name)) count++;
        }
        return count;
    }

    void SpawnEnemy(EnemyData enemyType)
    {
        if (spawnPoints.Length == 0 || enemyType.enemyPrefab == null) return;
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(enemyType.enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // 名前が "(Clone)" 付きになるので整形しておくと CountEnemyOfType で数えやすい
        newEnemy.name = enemyType.enemyPrefab.name + "_Enemy";
    }

}
