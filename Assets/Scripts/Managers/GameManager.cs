using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject mainTower;
    [SerializeField] private int currentMoney;


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
}
