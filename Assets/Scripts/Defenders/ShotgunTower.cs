using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunTower : DefenderAttack
{
    [SerializeField] private int maxTargets = 5; // Maximum number of enemies to attack
    [SerializeField] private float lineDuration = 0.1f; // Duration for line visibility
    [SerializeField] private GameObject explosionEffect;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 5f;
    [SerializeField] private float dampingSpeed = 0.7f;

    protected override IEnumerator Attack(Enemy target)
    {
        isAttacking = true;
        yield return new WaitForSeconds(atkDelay);

        if (target != null)
        {
            VisualizeAttack(target);
            Instantiate(explosionEffect, target.transform.position, Quaternion.identity);
            FindObjectOfType<CameraShake>().TriggerShake(shakeDuration, shakeMagnitude, dampingSpeed);
            // Get all enemies within the attack range
            List<Enemy> enemiesInRange = new List<Enemy>();

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, lineRenderer.GetPosition(1).magnitude);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    enemiesInRange.Add(hitCollider.GetComponent<Enemy>());
                }
            }

            // Attack enemies up to the maxTargets limit
            for (int i = 0; i < Mathf.Min(maxTargets, enemiesInRange.Count); i++)
            {
                Enemy enemyToAttack = enemiesInRange[i];
                if (enemyToAttack != null)
                {
                    // Deal damage to the enemy
                    enemyToAttack.TakeDamage(atkDamage);


                    // Remove the enemy from the list if it's dead
                    if (enemyToAttack.GetHealth() <= 0)
                    {
                        RemoveTarget(enemyToAttack.gameObject); // Remove the specific enemy
                    }
                }
            }

            // Remove the original target from the list if it's dead
            if (target.GetHealth() <= 0)
            {
                RemoveTarget(target.gameObject); // Remove the specific enemy
            }
        }

        isAttacking = false;
    }

}
