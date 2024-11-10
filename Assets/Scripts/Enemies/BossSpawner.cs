using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] GameObject boss;

    private void OnEnable()
    {
        Instantiate(boss, transform.position, Quaternion.identity);
    }
}
