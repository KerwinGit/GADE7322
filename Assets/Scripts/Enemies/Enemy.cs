using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected float atkDelay;
    [SerializeField] protected int atkDamage;
    [SerializeField] protected int health;
    [SerializeField] protected int value;

    protected NavMeshAgent agent;

    [SerializeField] protected Defender mainTower;
    [SerializeField] protected Defender target;

    public void TakeDamage(int damage) 
    {
        health -= damage;
    }

    public int GetHealth() 
    {
        return health;
    }

    public void giveMoney() 
    {
        gameManager.addMoney(value);
    }
    
}