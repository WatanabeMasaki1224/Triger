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
        // �S�G�̍��v����x����������iEnemy�^�O�̃I�u�W�F�N�g���j
        int totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        // �S�̍ő吔�ɒB���Ă���΃X�|�[�������Ȃ�
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

        // ���O�� "(Clone)" �t���ɂȂ�̂Ő��`���Ă����� CountEnemyOfType �Ő����₷��
        newEnemy.name = enemyType.enemyPrefab.name + "_Enemy";
    }

}
