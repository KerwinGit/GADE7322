using UnityEngine;

public class Defender : MonoBehaviour
{
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

    
}
