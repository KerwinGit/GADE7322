using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour //parent class
{
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected float atkDelay;
    [SerializeField] protected int atkDamage;
    [SerializeField] protected float health;
    [SerializeField] protected int goldAmt;

    //protected float maxHP;

    [SerializeField] protected Slider hpBar;

    protected NavMeshAgent agent;

    [SerializeField] protected Defender mainTower;
    [SerializeField] protected Defender target;

    public void TakeDamage(int damage) 
    {
        health -= damage;
    }

    public float GetHealth() 
    {
        return health;
    }

    public void giveMoney() 
    {
        gameManager.addMoney(goldAmt);
    }


    public void GetBuff()
    {
        StartCoroutine(BuffCoroutine());
    }

    IEnumerator BuffCoroutine()
    {
        float originalDelay = atkDelay;

        atkDelay = atkDelay/2;

        yield return new WaitForSeconds(3);

        atkDelay = originalDelay;
    }
}
