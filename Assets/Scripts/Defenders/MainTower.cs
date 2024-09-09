using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainTower : MonoBehaviour
{
    [SerializeField] private Defender mainTower;

    private bool regenning;

    private void Update()
    {
        if(mainTower.health < 1000 && !regenning)
        {
            StartCoroutine(Regen());
        }

        if (mainTower.health <= 0)
        {
            StopCoroutine(Regen());
        }
    }


    IEnumerator Regen()
    {
        regenning = true;

        while(mainTower.health<1000)
        {
            yield return new WaitForSeconds(1f);
            mainTower.health += 5;
        }

        regenning = false;
    }
}
