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
        spawnTotal = startTotal + gameManager.incrementCount;

        specialProbability = bigProbability + bomberProbability;

        basicProbability = 1f - specialProbability;

        Spawn(spawnTotal);
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
}
