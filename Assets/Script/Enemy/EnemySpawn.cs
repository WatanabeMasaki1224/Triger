using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public GameObject enemyPrefab;
    public int enemyCount = 5;
    public float spawnCooltime = 5f;
    [HideInInspector] public float timer = 0f; // �����p�^�C�}�[
}

public class EnemySpawn : MonoBehaviour
{
    [Header("�o���|�C���g")]
    public Transform[] spawnPoints;
    [Header("�G�̎��")]
    public List<EnemyData> enemyTypes;
    [Header("�S�̂̍ő哯���o����")]
    public int maxTotalEnemies = 20;


    void Update()
    {
        int totalEnemies = 0;
        foreach (var enemyType in enemyTypes)
        {
            totalEnemies += GameObject.FindGameObjectsWithTag(enemyType.enemyPrefab.tag).Length;
        }

        // �S�̍ő吔�ɒB���Ă���΃X�|�[�������Ȃ�
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

        // �����_���o���ʒu
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(enemyType.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

}
