using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject mainTower;
    [SerializeField] private int currentMoney;

    private float elapsedTime = 0f;
    private float lastIncrementTime = 0f;
    private float incrementTime = 20f;
    public int incrementCount = 1;

    private float spawnTime = 10f;
    private float lastSpawnTime = 0f;


    [SerializeField] private List<GameObject> spawnerObjects;

    [SerializeField] TMP_Text timerText;



    private void Update()
    {
        elapsedTime += Time.deltaTime;

        timerText.text = elapsedTime.ToString("F1");

        if (elapsedTime - lastIncrementTime >= incrementTime)
        {
            incrementCount++;

            lastIncrementTime = elapsedTime;
        }

        if(elapsedTime - lastSpawnTime >= spawnTime)
        {
            StartCoroutine(SpawnCoroutine());

            lastSpawnTime = elapsedTime;
        }
    }

    private IEnumerator SpawnCoroutine()
    {
            int activateCount = incrementCount;

            if (activateCount > spawnerObjects.Count)
            {
                activateCount = spawnerObjects.Count;
            }

            List<GameObject> shuffled = spawnerObjects;
            ShuffleList(shuffled);            

            for (int i = 0; i < activateCount; i++)
            {
                Debug.Log("active " + i + "]");
                GameObject obj = shuffled[i];
                obj.SetActive(true);
            }

            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < activateCount; i++)
            {
                GameObject obj = shuffled[i];
                obj.SetActive(false);
            }

        //StopCoroutine(SpawnCoroutine());
    }

    public void addMoney(int money) 
    {
        currentMoney += money;
    }

    public void removeMoney(int money) 
    {
        currentMoney -= money;
    }

    public void setCurrentMoney(int money) 
    {
        currentMoney = money;
    }

    public int getCurrentMoney() 
    {
        return currentMoney;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    private void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
