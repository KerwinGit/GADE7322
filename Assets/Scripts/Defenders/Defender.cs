using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
public class Defender : MonoBehaviour
{
    [SerializeField] protected GameManager gameManager;
    [SerializeField] public int baseHealth = 100;
    [SerializeField] public int health = 100;
    [SerializeField] private int cost = 10;
    [SerializeField] public bool isMain;

    [SerializeField] private MeshRenderer meshRenderer;
    private Color originalColor;
    public float flashDuration = 0.1f;

    [SerializeField] protected Slider hpBar;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        hpBar.maxValue = health;

        originalColor = meshRenderer.material.color;
    }
    private void Update()
    {
        hpBar.value = health;

        if(health <=0)
        {
            gameManager.xSpawnBias -= transform.position.x;
            gameManager.zSpawnBias -= transform.position.z;

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
