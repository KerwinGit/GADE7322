using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderAttack : MonoBehaviour
{
    [SerializeField] protected float atkDelay = 1;
    [SerializeField] protected int atkDamage = 1;
    [SerializeField] protected LineRenderer lineRenderer; // Reference to LineRenderer
    protected List<GameObject> targets; // Change to List<GameObject>
    protected bool isAttacking = false;

    private void Awake()
    {
        targets = new List<GameObject>(); // Initialize as a List
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false; // Start with line renderer disabled
        StartCoroutine(CleanUpNullTargets());
    }

    private void Update()
    {
        if (targets.Count > 0)
        {
            if (!isAttacking && targets[0] != null) // Use index for List
            {
                StartCoroutine(Attack(targets[0].GetComponent<Enemy>()));
            }
        }
        else
        {
            // Debug.Log("No enemy Targeted");
        }
    }

    protected virtual IEnumerator Attack(Enemy target)
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

        // Remove the enemy from the list if it's dead
        if (target.GetHealth() <= 0)
        {
            RemoveTarget(target.gameObject); // Remove the specific enemy
        }

        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !targets.Contains(other.gameObject)) // Avoid duplicates
        {
            targets.Add(other.gameObject); // Add to List
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targets[0]) // Check if it's the first target
        {
            RemoveTarget(other.gameObject); // Remove the specific enemy
        }
    }

    protected void VisualizeAttack(Enemy target)
    {
        // Set the positions for the LineRenderer
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, lineRenderer.GetPosition(0).y, transform.position.z)); // Start at the tower
        lineRenderer.SetPosition(1, target.transform.position);    // End at the enemy

        // Show the line
        lineRenderer.enabled = true;

        // Disable the line after a brief delay
        StartCoroutine(HideLineAfterDelay(0.1f)); // Hide after 0.1 seconds
    }

    private IEnumerator HideLineAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lineRenderer.enabled = false;
    }

    protected void RemoveTarget(GameObject target)
    {
        // Remove the specific target from the list
        targets.Remove(target);
    }

    private IEnumerator CleanUpNullTargets()
    {
        while (true)
        {
            // Remove null targets from the list
            targets.RemoveAll(target => target == null);

            // Wait for a short duration before checking again
            yield return new WaitForSeconds(0.5f);
        }
    }
}
