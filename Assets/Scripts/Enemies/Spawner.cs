using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;

    private GameManager gameManager;

    [SerializeField] private int spawnTotal = 10; //change this to increment with time
    private float basicProbability;
    [SerializeField] private float bigProbability;
    [SerializeField] private float bomberProbability;
    private float specialProbability;

    private void Awake()
    {
        specialProbability = bigProbability + bomberProbability;

        basicProbability = 1f - specialProbability;

        Spawn(spawnTotal);
    }

    private void Spawn(int enemies)
    {
        for (int i = 0; i < enemies; i++)
        {
            float randomValue = Random.Range(0f, 1f);

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
