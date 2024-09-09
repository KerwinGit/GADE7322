using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BasicEnemy : Enemy
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

        health = 20;
        atkDamage = 10;
        atkDelay = 1;
        agent.speed = 35;
        agent.stoppingDistance = 50;
        goldAmt = 3;

        hpBar.maxValue = health;
    }

    void Update()
    {
        hpBar.value = health;

        if (health <= 0)
        {
            giveMoney();
            Destroy(this.gameObject);
        }

        if (Vector3.Distance(this.transform.position, target.transform.position) < 100)
        {
            if (!isAttacking && target != null)
            {
                StartCoroutine(Attack(target));
            }
        }

        if (target == null || target.GetHealth() <= 0)
        {
            target = mainTower;
        }
        else
        {
            agent.destination = target.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Defender"))
        {

            target = other.gameObject.GetComponent<Defender>();

        }
        else if (other.CompareTag("MainTower") && target != null)
        {
            target = other.gameObject.GetComponent<Defender>();
        }
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
        lineRenderer.SetPosition(1, new Vector3(target.transform.position.x, target.transform.position.y+30, target.transform.position.z));    // End at the tower

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
