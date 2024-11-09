using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class ProceduralBoss : Enemy
{
    private float maxStatBudget = 1000f;
    private float healthRatio, damageRatio, speedRatio;

    [SerializeField] private LineRenderer lineRenderer; // Reference to LineRenderer
    private bool isAttacking = false;

    private enum bossType
    {
        balanced,
        attacker,
        tank,
        support
    }

    private bossType type;


    private void Awake()
    {
        System.Random random = new System.Random(DateTime.Now.Millisecond);
        type = (bossType)random.Next(0, Enum.GetValues(typeof(bossType)).Length);

        // Generate balanced ratios for health, damage, and speed
        GenerateBalancedStats();

        // Assign values based on the ratios and the max budget
        health = maxStatBudget * healthRatio;
        atkDamage = Mathf.RoundToInt(maxStatBudget * damageRatio);
        agent.speed = maxStatBudget * speedRatio;
        goldAmt = 100;
        hpBar.maxValue = health;

        switch(type)
        {
            case bossType.attacker:
                atkDamage *= 2;
                break;
            case bossType.tank:
                health *= 2;
                break;
            case bossType.support:
                atkDelay /= 2;
                atkDamage /= 2;
                break;
            case bossType.balanced:
                ;
                break;
        }

        // Other setup
        target = gameManager.mainTower.GetComponent<Defender>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void GenerateBalancedStats()
    {
        // Randomly assign ratios while maintaining balance
        healthRatio = UnityEngine.Random.Range(0.3f, 0.5f); // Health takes 30% to 50% of the budget
        damageRatio = UnityEngine.Random.Range(0.2f, 0.4f); // Damage takes 20% to 40% of the budget
        speedRatio = 1f - (healthRatio + damageRatio); // Remaining budget goes to speed

        // Ensure speed doesn't fall below a minimum threshold
        if (speedRatio < 0.1f) speedRatio = 0.1f;
    }

    private void Update()
    {
        hpBar.value = health;

        if (health <= 0)
        {
            giveMoney();
            Destroy(gameObject);
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

    #region laser attack
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
        lineRenderer.SetPosition(1, new Vector3(target.transform.position.x, target.transform.position.y + 30, target.transform.position.z));    // End at the tower

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
    #endregion

    IEnumerator Buff()
    {
        yield return null;
    }
}
