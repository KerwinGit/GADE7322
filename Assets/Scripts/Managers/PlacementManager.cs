using UnityEngine;
using TMPro;
public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Defenders")]
    [SerializeField] private GameObject selectedDefender;
    [SerializeField] private GameObject [] defenderArr;
    [SerializeField] private TMP_Text txtSelectedDefender;

    [Header("Other Goodies")]
    [SerializeField] private Color placeableColour;
    [SerializeField] private bool clicked;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float placementRadius = 0.2f; // Minimum distance between defenders

    [SerializeField] private TMP_Text txtError;
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        txtSelectedDefender.text = "Selected Defender\n" + selectedDefender.name;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !gameManager.paused)
        {
            SpawnDefenderOnCursor();
        }
    }

    public void SpawnDefenderOnCursor()
    {
        if (gameManager.getCurrentMoney() >= selectedDefender.GetComponent<Defender>().getCost())
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
                        gameManager.removeMoney(selectedDefender.GetComponent<Defender>().getCost());
                        Instantiate(selectedDefender, hit.point, Quaternion.identity);
                    }
                    else
                    {

                        txtError.text = "Too close to another defender!";
                        Debug.Log("Too close to another defender.");
                        uiManager.FlashErrorMessage();
                    }
                }
                else
                {
                    txtError.text = "Can only place on grey!";
                    Debug.Log("Can't place, Y is " + hit.point.y);
                    uiManager.FlashErrorMessage();
                }
            }
        }
        else
        {
            txtError.text = "Not enough money!";
            Debug.Log("no money");
            uiManager.FlashErrorMessage();
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

    public void switchSelectedDefender(int defenderIndex) 
    {
        selectedDefender = defenderArr[defenderIndex];
        txtSelectedDefender.text = "Selected Defender\n" + selectedDefender.name;
    }
}

