using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class buttonScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void goToGameplay()
    {
        SceneManager.LoadScene(1);
    }
    public void goToTitle()
    {
        SceneManager.LoadScene(0); // Load the title scene (index 0)
    }
    // Update is called once per frame
    public void quitTheGame()
    {
        Application.Quit(); // Quit the game
    }
}
