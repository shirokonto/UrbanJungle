using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Event_Namespace;

// Source: www.youtube.com/watch?v=zc8ac_qUXQY&t=643s

public class GameLoader : MonoBehaviour
{
  [SerializeField] private bool loadScenes;
  
  [Header("Events")]
  [SerializeField] private GameEvent onFadeComplete;
  [SerializeField] private GameEvent onLoadGameScenesComplete;
  
  [Header("Canvas")]
  [SerializeField] private CanvasManager canvasManager;
  [SerializeField] private CanvasGroup fadeMenu;
  [SerializeField] private MusicBehaviour musicBehaviour;
  [SerializeField] private float fadeTime = 1f;
  
  private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
  

  private void HideFadeMenu(Action onfadeCompleteAction = null)
  {
    LeanTween.alphaCanvas(fadeMenu, 0f, fadeTime).setOnComplete(() =>
    {
      fadeMenu.gameObject.SetActive(false);
      onfadeCompleteAction?.Invoke();
    });
  }
  
  private void ShowFadeMenu(Action onCompleteAction = null)
  {
    fadeMenu.gameObject.SetActive(true);
    LeanTween.alphaCanvas(fadeMenu, 1f, fadeTime).setOnComplete(onCompleteAction);
  }

  public void Quit()
  {
    Application.Quit();
  }
  
  public void LoadGameScenes()
  {
    if (!loadScenes)
    {
      canvasManager.CloseCanvas();
      return;
    }
    
    musicBehaviour.Disable(fadeTime);
    ShowFadeMenu(() =>
    {
      canvasManager.CloseCanvas();
      
      scenesToLoad.Add(SceneManager.LoadSceneAsync("Character",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("WilmasRoom",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("RouteA",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("Route_B",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("Route_C",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("Smartphone",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("CloudFloor",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("CollectableItems",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("UI",LoadSceneMode.Additive));
      scenesToLoad[scenesToLoad.Count - 1].completed +=  _ =>
      {
        onLoadGameScenesComplete.Raise();
        HideFadeMenu(() => onFadeComplete.Raise());
      };
    });
  }

  public void UnloadGameScenes(MenuType_SO menuToBeOpened)
  {
    if (!loadScenes)
    {
      return;
    }
    
    ShowFadeMenu(() =>
    {
      canvasManager.SwitchCanvas(menuToBeOpened);
      
      SceneManager.UnloadSceneAsync("Character");
      SceneManager.UnloadSceneAsync("WilmasRoom");
      SceneManager.UnloadSceneAsync("RouteA");
      SceneManager.UnloadSceneAsync("Route_B");
      SceneManager.UnloadSceneAsync("Route_C");
      SceneManager.UnloadSceneAsync("Smartphone");
      SceneManager.UnloadSceneAsync("CloudFloor");
      SceneManager.UnloadSceneAsync("CollectableItems");
      SceneManager.UnloadSceneAsync("UI");

      scenesToLoad[scenesToLoad.Count - 1].completed += _ =>
      {
        HideFadeMenu(() =>
        {
          scenesToLoad.Clear();
          musicBehaviour.Enable();
        });
      };
    });
  }
}
