using UnityEngine;
using System.Collections;
public class Defender : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int cost = 10;
    [SerializeField] private bool isMain;

    [SerializeField] private MeshRenderer meshRenderer;
    private Color originalColor;
    public float flashDuration = 0.1f;

    private void Start()
    {
        originalColor = meshRenderer.material.color;
    }
    private void Update()
    {
        if(health <=0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isMain)
        {
            Flash();
        }
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

    private void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        
            meshRenderer.material.color = Color.white;

            yield return new WaitForSeconds(flashDuration);

            meshRenderer.material.color = originalColor;

            yield return new WaitForSeconds(0.5f);

        

    }

}
