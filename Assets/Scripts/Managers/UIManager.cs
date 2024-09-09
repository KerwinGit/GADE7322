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
        txtError.enabled = true;  // Turn the text on
        yield return new WaitForSeconds(1f); // Wait for 1 second

        txtError.enabled = false; // Turn the text off

        // Make sure the text is off after flashing twice
        txtError.enabled = false;
    }
}
