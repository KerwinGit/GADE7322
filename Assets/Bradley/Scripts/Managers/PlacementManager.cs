using UnityEngine;

public class PlacementManager : MonoBehaviour
{

    [SerializeField] private GameObject defenderPrefab;
    [SerializeField] private Color placeableColour;
    [SerializeField] private bool clicked;
    [SerializeField] private LayerMask terrainLayer;
    // Start is called before the first frame update

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SpawnDefenderOnCursor();
        }
    }
    public void SpawnDefenderOnCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.point.y < 40 && hit.point.y >0.4f)
            {
                Instantiate(defenderPrefab, hit.point, Quaternion.identity);
            }
            else
            {
                Debug.Log("Can't Y is " + hit.point.y);
            }

        }

    }


}

