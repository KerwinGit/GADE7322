using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject mainTower;
    [SerializeField] private int currentMoney;

    private float elapsedTime = 0f;
    [SerializeField] private float lastIncrementTime = 0f;
    private float incrementTime = 15f;
    public int waveCount = 1;
    [SerializeField] TMP_Text waveText;

    private float spawnTime = 20f;
    [SerializeField] private float lastSpawnTime = 0f;


    [SerializeField] private List<GameObject> spawnerObjects;

    [SerializeField] TMP_Text timerText;

    public bool paused = false;

    [SerializeField] private GameObject losePanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject pausePanel;


    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private void Update()
    {

        //time tracking
        elapsedTime += Time.deltaTime;

        timerText.text = elapsedTime.ToString("F1");
        waveText.text = "Wave: " + waveCount.ToString();

        //spawn intervals
        if (elapsedTime - lastSpawnTime >= spawnTime)
        {
            waveCount++;

            StartCoroutine(SpawnCoroutine());

            lastSpawnTime = elapsedTime;
        }

        //checks if dead
        if (mainTower.GetComponent<Defender>().health <= 0)
        {
            Lose();
        }

        //pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        int activateCount = Mathf.CeilToInt(waveCount * 0.5f);
        activateCount = Mathf.Min(activateCount, spawnerObjects.Count);

        if (activateCount > spawnerObjects.Count)
        {
            activateCount = spawnerObjects.Count;
        }

        //Debug.Log(activateCount);


        //shuffles spawners and activates them randomly
        List<GameObject> shuffled = spawnerObjects;
        ShuffleList(shuffled);

        for (int i = 0; i < activateCount; i++)
        {
            //Debug.Log("active " + i + "]");
            GameObject obj = shuffled[i];
            obj.SetActive(true);
        }

        yield return new WaitForSeconds(5f);

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

    private void ShuffleList<T>(List<T> list)     //helper method for shuffling spawners
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

    private void Lose()
    {
        scoreText.text = timerText.text;
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void TogglePause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            paused = false;
        }
        else
        {
            pausePanel.SetActive(true);
            paused = true;
            Time.timeScale = 0;

        }
    }

    public int getWaveCount()
    {
        return waveCount;
    }
}
