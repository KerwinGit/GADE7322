using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberDefenderAttack : DefenderAttack
{
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the BomberEnemy component
        BomberEnemy bomberEnemy = other.GetComponent<BomberEnemy>();

        if (bomberEnemy != null && !targets.Contains(other.gameObject)) // Only add if it's a BomberEnemy and not already targeted
        {
            targets.Add(other.gameObject);
        }
    }

    // Override the Attack method to only attack bombers
    protected override IEnumerator Attack(Enemy target)
    {
        BomberEnemy bomberEnemy = target as BomberEnemy;
        if (bomberEnemy != null)
        {
            yield return base.Attack(target); //base attack called for bombers
        }
        
    }
}
