
// Start is called before the first frame update
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoseMenu : MonoBehaviour
{
   
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    //public static bool Gameisfinished = false;

    //"Button" in Inspector for the EndMenu
    public GameObject GameEnd;


    //When gameover = Screen is freezed and EndMenu is set true
    
    public void End()
    {
   
        GameEnd.SetActive(true);
        Time.timeScale = 0f;
      
    }

    //When button PlayAgain is pressed, the game/scene will reload from the beginning
    // Game is not freezed
    public void PlayAgain()
    {
        scenesToLoad.Add(SceneManager.LoadSceneAsync("AnimationScene",LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("MazeGenerationScene",LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Music",LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("MapScene",LoadSceneMode.Additive));
       
    }
    
    //When button "Main menu" is pressed, the scene "MainMenu" is on the screen
    public void Menu()
    {
        // Time.timeScale = 1f;
        SceneManager.LoadScene("CanvasController");
    }

}
