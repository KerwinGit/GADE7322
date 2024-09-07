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

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        mainTower = gameManager.mainTower.GetComponent<Defender>();

        target = mainTower;

        agent = GetComponent<NavMeshAgent>();

        lineRenderer = GetComponent<LineRenderer>();

        health = 60;
        atkDamage = 100;
        atkDelay = 0;
        agent.speed = 200;
        agent.stoppingDistance = 0;
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

    IEnumerator Attack(Defender target)
    {
        isAttacking = true;
        yield return new WaitForSeconds(atkDelay);

        
        // Deal damage to the enemy
        target.TakeDamage(atkDamage);

        isAttacking = false;

        Instantiate(explosionPF);

        TakeDamage(atkDamage);        
    }

}
