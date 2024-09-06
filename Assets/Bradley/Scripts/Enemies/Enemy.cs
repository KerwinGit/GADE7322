using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed  = 1;
    [SerializeField] private int health = 100;
    // Start is called before the first frame update

   

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