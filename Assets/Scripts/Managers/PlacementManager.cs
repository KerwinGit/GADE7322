using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject defenderPrefab;
    [SerializeField] private Color placeableColour;
    [SerializeField] private bool clicked;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float placementRadius = 0.2f; // Minimum distance between defenders

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SpawnDefenderOnCursor();
        }
    }

    public void SpawnDefenderOnCursor()
    {
        if (gameManager.getCurrentMoney() >= defenderPrefab.GetComponent<Defender>().getCost())
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
            {
                if (hit.point.y < 40 && hit.point.y > 0.4f)
                {
                    // Check for nearby defenders
                    if (!IsTooCloseToAnotherDefender(hit.point))
                    {
                        gameManager.removeMoney(defenderPrefab.GetComponent<Defender>().getCost());
                        Instantiate(defenderPrefab, hit.point, Quaternion.identity);
                    }
                    else
                    {
                        Debug.Log("Too close to another defender.");
                    }
                }
                else
                {
                    Debug.Log("Can't place, Y is " + hit.point.y);
                }
            }
        }
        else
        {
            Debug.Log("no money");
        }

    }

    private bool IsTooCloseToAnotherDefender(Vector3 spawnPoint)
    {
        // Find all objects tagged as "defender"
        GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");
        foreach (GameObject defender in defenders)
        {
            // Check distance between the spawn point and each defender
            if (Vector3.Distance(spawnPoint, defender.transform.position) < placementRadius)
            {
                return true; // Defender is too close
            }
        }
        return false; // No defenders are too close
    }
}

