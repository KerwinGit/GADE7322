using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // Import to detect UI elements

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Defenders")]
    [SerializeField] private GameObject selectedDefender;
    [SerializeField] private GameObject[] defenderArr;
    [SerializeField] private TMP_Text txtSelectedDefender;
    [SerializeField] private TMP_Text txtDefenderCost;

    [Header("Other Goodies")]
    [SerializeField] private Color placeableColour;
    [SerializeField] private bool clicked;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float placementRadius = 0.2f; // Minimum distance between defenders

    [SerializeField] private TMP_Text txtError;
    [SerializeField] private UIManager uiManager;

    //[SerializeField] private GameObject closeTower;

    private void Awake()
    {
        txtSelectedDefender.text = "Selected Defender\n" + selectedDefender.name;
        txtDefenderCost.text = "Defender Cost: " + selectedDefender.GetComponent<Defender>().getCost();
    }

    private void Update()
    {
        // Check for left mouse button click and ensure game is not paused
        if (Input.GetKeyDown(KeyCode.Mouse0) && !gameManager.paused)
        {
            // Ensure mouse is not over a UI element before placing a defender
            if (!IsPointerOverUI())
            {
                SpawnDefenderOnCursor();
            }
            else
            {
                Debug.Log("Pointer is over UI, not placing defender.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {

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

                        gameManager.xSpawnBias += hit.point.x;
                        gameManager.zSpawnBias += hit.point.z;

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
            Debug.Log("No money");
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

    // Method to detect if the pointer is over a UI element
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(); // Detect if mouse is over UI
    }

    public void switchSelectedDefender(int defenderIndex)
    {
        selectedDefender = defenderArr[defenderIndex];
        txtSelectedDefender.text = "Selected Defender\n" + selectedDefender.name;
        txtDefenderCost.text = "Defender Cost: " + selectedDefender.GetComponent<Defender>().getCost();
    }
}
