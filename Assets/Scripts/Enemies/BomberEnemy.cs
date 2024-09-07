using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class BomberEnemy : Enemy
{
    [SerializeField] private LineRenderer lineRenderer; // Reference to LineRenderer
    private bool isAttacking = false;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        mainTower = gameManager.mainTower.GetComponent<Defender>();

        target = mainTower;

        agent = GetComponent<NavMeshAgent>();

        lineRenderer = GetComponent<LineRenderer>();

        atkDamage = 100;
        atkDelay = 0;
        agent.speed = 100;
        agent.stoppingDistance = 0;
    }

    void Update()
    {
        if (target != null)
        {
            agent.destination = target.transform.position;
        }
        else
        {
            target = mainTower;
        }

        if (Vector3.Distance(this.transform.position, target.transform.position) < 70)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Defender"))
        {
            target = other.gameObject.GetComponent<Defender>();
        }
        else if (other.CompareTag("MainTower"))
        {
            target = other.gameObject.GetComponent<Defender>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        target = mainTower;
    }

    IEnumerator Attack(Defender target)
    {
        isAttacking = true;
        yield return new WaitForSeconds(atkDelay);

        // Show the attack line for a brief moment
        if (target != null)
        {
            VisualizeAttack(target);
        }

        // Deal damage to the enemy
        target.TakeDamage(atkDamage);

        // Remove the enemy from the queue if it's dead
        if (target.GetHealth() <= 0)
        {
            target = mainTower;
        }

        isAttacking = false;
    }

    private void VisualizeAttack(Defender target)
    {
        // Set the positions for the LineRenderer
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, lineRenderer.GetPosition(0).y, transform.position.z)); // Start at the enemy
        lineRenderer.SetPosition(1, target.transform.position);    // End at the tower

        // Show the line
        lineRenderer.enabled = true;

        // Disable the line after a brief delay
        StartCoroutine(HideLineAfterDelay(0.1f)); // Hide after 0.1 seconds
    }

    IEnumerator HideLineAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lineRenderer.enabled = false;
    }
}
