using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text txtGold;
    [SerializeField] private TMP_Text txtError;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        txtGold.text = "Money: " + gameManager.getCurrentMoney();
    }

    public void FlashErrorMessage() 
    {
        StartCoroutine(FlashText());
    }
    IEnumerator FlashText()
    {
        // Flash twice (on, off, on, off)
        for (int i = 0; i < 2; i++)
        {
            txtError.enabled = true;  // Turn the text on
            yield return new WaitForSeconds(1f); // Wait for 1 second

            txtError.enabled = false; // Turn the text off
            yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds
        }

        // Make sure the text is off after flashing twice
        txtError.enabled = false;
    }
}
