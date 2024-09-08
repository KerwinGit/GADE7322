using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int cost = 10;
    private void Update()
    {
        if(health <=0)
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

    public int getCost() 
    {
        return cost;
    }

    
}
