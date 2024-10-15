using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private int startTotal = 5;
    private int spawnTotal;
    private float basicProbability;
    [SerializeField] private float bigProbability;
    [SerializeField] private float bomberProbability;
    private float specialProbability;

    private void OnEnable()
    {
        #region legacy
        //spawnTotal = startTotal + gameManager.waveCount;

        //specialProbability = bigProbability + bomberProbability;

        //basicProbability = 1f - specialProbability;

        //Spawn(spawnTotal);
        #endregion

        int Wave = gameManager.getWaveCount();

        SpawnEnemies(Wave);
    }

    private void Spawn(int enemies)
    {
        Debug.Log("spawn");
        for (int i = 0; i < enemies; i++)
        {
            float randomValue = Random.Range(0f, 1f);

            //spawns according to probability of each class to spawn
            if (randomValue < basicProbability)
            {
                Instantiate(enemyPrefabs[0], transform.position, Quaternion.identity);
            }
            else
            {
                randomValue = Random.Range(0f, 1f);

                if(randomValue < bigProbability/specialProbability)
                {
                    Instantiate(enemyPrefabs[1], transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(enemyPrefabs[2], transform.position, Quaternion.identity);
                }
            }

        }
    }

    int CalculateTotalEnemies(int waveNumber)
    {
        int baseEnemyCount = 5;
        float scalingFactor = 1.1f;
        return Mathf.RoundToInt(baseEnemyCount * Mathf.Pow(scalingFactor, waveNumber - 1));
    }

    (int normal, int big, int bomber) CalculateEnemyTypes(int waveNumber, int totalEnemies)
    {
        float normalEnemyProportion = Mathf.Max(0.7f - waveNumber * 0.02f, 0.5f);
        float bigEnemyProportion = Mathf.Min(0.25f + waveNumber * 0.015f, 0.35f);
        //float bomberEnemyProportion = 1.0f - (normalEnemyProportion + bigEnemyProportion);

        int normalEnemyCount = Mathf.RoundToInt(totalEnemies * normalEnemyProportion);
        int bigEnemyCount = Mathf.RoundToInt(totalEnemies * bigEnemyProportion);
        int bomberEnemyCount = totalEnemies - (normalEnemyCount + bigEnemyCount);

        return (normalEnemyCount, bigEnemyCount, bomberEnemyCount);
    }

    void SpawnEnemies(int waveNumber)
    {
        int totalEnemies = CalculateTotalEnemies(waveNumber);
        var (normal, big, bomber) = CalculateEnemyTypes(waveNumber, totalEnemies);

        for (int i = 0; i < normal; i++) 
        {
            Instantiate(enemyPrefabs[0], transform.position, Quaternion.identity);
        }
        for (int i = 0; i < big; i++)
        {
            Instantiate(enemyPrefabs[1], transform.position, Quaternion.identity);
        }
        for (int i = 0; i < bomber; i++)
        {
            Instantiate(enemyPrefabs[2], transform.position, Quaternion.identity);
        }
    }

}
