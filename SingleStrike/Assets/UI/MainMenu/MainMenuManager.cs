using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Method to load the game scene
    public void StartGame()
    {
        // Load the main game scene (make sure your scene is added to the build settings)
        SceneManager.LoadScene("MovementTest");  // Replace "GameScene" with the actual name of your game scene
    }

    // Method for opening options (can later be used to load an options menu)
    public void OpenOptions()
    {
        Debug.Log("Options menu clicked!");
        // You can add logic to open an options menu
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quit game clicked!");
        Application.Quit();
    }
}
