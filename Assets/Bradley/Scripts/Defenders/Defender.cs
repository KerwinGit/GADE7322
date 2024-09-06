using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] private int health = 100;

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

    
}
