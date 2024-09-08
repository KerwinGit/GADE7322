using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using Unity.VisualScripting;

public class BomberEnemy : Enemy
{
    [SerializeField] private LineRenderer lineRenderer; // Reference to LineRenderer
    private bool isAttacking = false;

    [SerializeField] private GameObject explosionPF;

    private MeshRenderer meshRenderer; 
    private Color originalColor;

    public Color flashColor = Color.white;
    public float flashDuration = 0.1f;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        mainTower = gameManager.mainTower.GetComponent<Defender>();

        target = mainTower;

        agent = GetComponent<NavMeshAgent>();

        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;

        health = 60;
        atkDamage = 100;
        atkDelay = 0;
        agent.speed = 200;
        agent.stoppingDistance = 0;
        value = 3;

        Flash();
    }

    void Update()
    {
        agent.destination = target.transform.position;
        

        if (Vector3.Distance(this.transform.position, target.transform.position) < 20)
        {
            if (!isAttacking && target != null)
            {
                StartCoroutine(Attack(target));
            }
        }

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        while (true)
        {
            meshRenderer.material.color = flashColor;

            yield return new WaitForSeconds(flashDuration);

            meshRenderer.material.color = originalColor;

            yield return new WaitForSeconds(0.5f);

        }
        
    }

    IEnumerator Attack(Defender target)
    {
        isAttacking = true;
        yield return new WaitForSeconds(atkDelay);

        
        // Deal damage to the enemy
        target.TakeDamage(atkDamage);

        isAttacking = false;

        Instantiate(explosionPF);
        FindObjectOfType<CameraShake>().TriggerShake();
        TakeDamage(atkDamage);        
    }

}
