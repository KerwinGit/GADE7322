using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderAttack : MonoBehaviour
{
    [SerializeField] private float atkDelay = 1;
    [SerializeField] private int atkDamage = 1;
    [SerializeField] private LineRenderer lineRenderer; // Reference to LineRenderer
    private Queue<GameObject> targets;
    private bool isAttacking = false;

    private void Awake()
    {
        targets = new Queue<GameObject>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false; // Start with line renderer disabled
    }

    private void Update()
    {
        if (targets.Count > 0)
        {
            if (!isAttacking & targets.Peek() != null)
            {
                StartCoroutine(Attack(targets.Peek().GetComponent<Enemy>()));
            }
        }
        else
        {
            //Debug.Log("No enemy Targeted");
        }
    }

    IEnumerator Attack(Enemy target)
    {
        isAttacking = true;
        yield return new WaitForSeconds(atkDelay);

        // Show the attack line for a brief moment

        if(target !=  null ) 
        {
            VisualizeAttack(target);
        }

        // Deal damage to the enemy
        target.TakeDamage(atkDamage);

        // Remove the enemy from the queue if it's dead
        if (target.GetHealth() <= 0)
        {
            targets.Dequeue();
        }

        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            targets.Enqueue(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targets.Peek())
        {
            targets.Dequeue();
        }
    }

    private void VisualizeAttack(Enemy target)
    {
        // Set the positions for the LineRenderer
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, lineRenderer.GetPosition(0).y, transform.position.z)); // Start at the tower
        lineRenderer.SetPosition(1, target.transform.position);    // End at the enemy

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
