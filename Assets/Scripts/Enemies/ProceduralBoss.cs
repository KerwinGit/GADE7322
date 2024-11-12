using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ProceduralBoss : Enemy
{
    private string name = "";

    private float maxStatBudget = 500f;
    private float healthRatio, damageRatio, speedRatio;

    [SerializeField] private LineRenderer lineRenderer; // Reference to LineRenderer
    private bool isAttacking = false;
    private float buffRadius = 100;

    private Slider bossBar;

    private enum bossType
    {
        balanced,
        attacker,
        tank,
        support
    }

    [SerializeField] private bossType type;

    string[] enemyNames = new string[]
{
    "Gloomfang",
    "Dreadmaw",
    "Ironhide",
    "Boneclaw",
    "Grimshade",
    "Nightstalker",
    "Frostbite",
    "Bloodspike",
    "Rageclaw",
    "Venomtail",
    "Shadowfiend",
    "Blazefury",
    "Skullcrusher",
    "Terrorfang",
    "Ghoulshade",
    "Stormreaver",
    "Hellfire",
    "Rotting Maw",
    "Darkwhisper",
    "Flamepike",
    "Thunderclap",
    "Ashenblade",
    "Voidcaller",
    "Spinebreaker",
    "Soulrender",
    "Steeljaw",
    "Stonefist",
    "Icevein",
    "Emberheart",
    "Voidspawn",
    "Cinderclaw",
    "Thornspike",
    "Deathbringer",
    "Holloweye",
    "Scorchmaw",
    "Rottingfang",
    "Plagueshade",
    "Blightmire",
    "Wrathscale",
    "Gorehound"
};

    string[] damageArchetypes = {
    "Assassin",
    "Berserker",
    "Ravager",
    "Vindicator",
    "Blade",
    "Reaper",
    "Destroyer",
    "Warbringer",
    "Striker",
    "Marauder"
};
    string[] tankArchetypes = {
    "Ironclad",
    "Bulwark",
    "Fortress",
    "Aegis",
    "Juggernaut",
    "Colossus",
    "Goliath",
    "Shieldbearer",
    "Titan",
    "Guardian"
};
    string[] supportArchetypes = {
    "Caretaker",
    "Mystic",
    "Oracle",
    "Protector",
    "Sentinel",
    "Guardian",
    "Sage",
    "Lifebinder",
    "Harbinger",
    "Luminary"
};
    string[] balancedArchetypes = {
    "Vanguard",
    "Warden",
    "Arbiter",
    "Sentinel",
    "Enforcer",
    "Champion",
    "Phalanx",
    "Guardian",
    "Marshal",
    "Legionnaire"
};

    [SerializeField] Renderer horn1;
    [SerializeField] Renderer horn2;
    [SerializeField] Renderer band;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        System.Random random = new System.Random(DateTime.Now.Millisecond);
        type = (bossType)random.Next(0, Enum.GetValues(typeof(bossType)).Length);

        agent = GetComponent<NavMeshAgent>();

        // Generate balanced ratios for health, damage, and speed
        GenerateBalancedStats();

        // Assign values based on the ratios and the max budget
        health = maxStatBudget * healthRatio;
        atkDamage = Mathf.RoundToInt(maxStatBudget * damageRatio);
        agent.speed = maxStatBudget * speedRatio;
        atkDelay = 3;
        agent.stoppingDistance = 75;
        goldAmt = 100;

        string name1 = enemyNames[random.Next(0, enemyNames.Length)];
        string name2 = "";

        switch(type)
        {
            case bossType.attacker:
                health /= 2;
                atkDamage *= 2;
                name2 = damageArchetypes[random.Next(0, damageArchetypes.Length)];
                horn1.material.color = Color.red;
                horn2.material.color = Color.red;
                band.material.color = Color.red;
                break;
            case bossType.tank:
                health *= 2;
                atkDamage /= 2;
                name2 = tankArchetypes[random.Next(0, tankArchetypes.Length)];
                horn1.material.color = Color.blue;
                horn2.material.color = Color.blue;
                band.material.color = Color.blue;
                break;
            case bossType.support:
                atkDelay *= 2;
                atkDamage /= 2;
                name2 = supportArchetypes[random.Next(0, supportArchetypes.Length)];
                horn1.material.color = Color.green;
                horn2.material.color = Color.green;
                band.material.color = Color.green;
                break;
            case bossType.balanced:
                name2 = balancedArchetypes[random.Next(0, balancedArchetypes.Length)];
                horn1.material.color = Color.black;
                horn2.material.color = Color.black;
                band.material.color = Color.black;
                break;
        }


        bossBar = gameManager.bossBar;
        bossBar.maxValue = health;
        hpBar.maxValue = health;

        name = $"{name1} the {name2}";
        gameManager.bossText.text = name;

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
        bossBar.value = health;

        if (health <= 0)
        {
            giveMoney();
            gameManager.bossCounter--;
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
        if(type == bossType.support)
        {
            ApplyBuff();
        }

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

    private void ApplyBuff()
    {
        List<GameObject> enemiesInRange = new List<GameObject>();

        // Get all colliders within the specified radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, buffRadius);

        // Loop through each collider and add the ones tagged as "Enemy" to the list
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy") && collider.gameObject != this.gameObject)
            {
                enemiesInRange.Add(collider.gameObject);
            }
        }

        foreach(GameObject enemy in enemiesInRange)
        {
            enemy.GetComponent<Enemy>().GetBuff();
        }
    }
}
