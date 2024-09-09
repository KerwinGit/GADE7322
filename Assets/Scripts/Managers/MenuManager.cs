using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadLevel() //load the game level
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu() //load the main menu
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Close() //close the game
    {
        Application.Quit();
    }

}

