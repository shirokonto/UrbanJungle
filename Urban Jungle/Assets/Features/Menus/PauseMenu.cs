using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Source: https://www.youtube.com/watch?v=JivuXdrIHK0&t=541s
public class PauseMenu : MonoBehaviour
{

    // the game is not paused 
    public static bool PausedGame = false;

    // "button" in inspector for the pauseMenu
    public GameObject pauseMenu;
    // Update is called once per frame

    
    /// <summary>
    /// press "F" for PauseMenu
    /// if game is paused we want to go back to the game and if its not paused we want it to pause
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))

        {
            if (PausedGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
               
        }
    }
    
    
    //Scene is loaded and we can play 
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        PausedGame = false;
        SceneManager.LoadScene("Character"); //Scene einfügen
        
    }
    
    //When P is pressed the game freezes and we see our PauseMenuScene
    public void Pause()
    {
        // if (Input.GetKeyDown(KeyCode.P)) 
        // SceneManager.LoadScene("PauseMenu");
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        PausedGame = true;
        //SceneManager.LoadScene("PauseMenu");
    }

   
    //When "MainMenu" button is pressed, we will see our main menu on the screen
    public void Menu()
    {
        Time.timeScale = 0f;
        PausedGame = true; 
        SceneManager.LoadScene("MainMenu");

      //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      // Time.timeScale = 1f;
    }

    //Wenn "Exit" button is pressed = game is quit
    public void ExitGame()
    {
        Debug.Log("Exit ...");
        Application.Quit();
        
    }
}

