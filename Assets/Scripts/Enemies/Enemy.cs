using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed  = 1;
    [SerializeField] private int health = 100;

    private NavMeshAgent agent;

    [SerializeField] private Transform mainTower;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = mainTower.position;

        if(health<=0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage) 
    {
        health -= damage;
    }

    public int GetHealth() 
    {
        return health;
    }

    public float GetSpeed() 
    {
        return speed;
    }
    
}
