//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AOEAttackTower : DefenderAttack
//{
//    [SerializeField] private float aoeRadius = 5f; // AOE radius, editable in Inspector
//    [SerializeField] private ParticleSystem explosionEffect; // Particle system for explosion

//    protected override IEnumerator Attack(Enemy target)
//    {
//        isAttacking = true;
//        yield return new WaitForSeconds(atkDelay);

//        if (target != null)
//        {
//            VisualizeAttack(target);

//            // Perform AOE attack - find all enemies within the AOE radius
//            Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, aoeRadius);
//            foreach (var hitCollider in hitColliders)
//            {
//                if (hitCollider.CompareTag("Enemy"))
//                {
//                    Enemy enemy = hitCollider.GetComponent<Enemy>();
//                    if (enemy != null)
//                    {
//                        enemy.TakeDamage(atkDamage);
//                    }
//                }
//            }

//            // Spawn particle effect at the attack site
//            if (explosionEffect != null)
//            {
//                Instantiate(explosionEffect, target.transform.position, Quaternion.identity);
//            }

//            // Remove the enemy from the queue if it's dead
//            if (target.GetHealth() <= 0)
//            {
//                targets.Dequeue();
//            }
//        }

//        isAttacking = false;
//    }
//}
