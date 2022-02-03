using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.GameStates_Namespace.Scripts.States
{
    [CreateAssetMenu(fileName = "MenuState", menuName = "GameStates/Menu")]
    public class MenuState_SO : State_SO
    {
        [SerializeField] protected GameStateController_SO gameStateController;
        [SerializeField] private MenuType_SO swapMenu;
        public float fadeTime = 1f;

        private List<AsyncOperation> ScenesToLoad => gameStateController.ScenesToLoad;
        
        public override void Enter()
        {
            if (ScenesToLoad.Count == 0) return;
            
            ShowFadeMenu(() =>
            {
                gameStateController.CanvasManager.AddCanvas(swapMenu);
                
                SceneManager.UnloadSceneAsync("WilmasRoom");
                SceneManager.UnloadSceneAsync("CollectableItems");
                SceneManager.UnloadSceneAsync("Route_A");
                SceneManager.UnloadSceneAsync("Route_B");
                SceneManager.UnloadSceneAsync("Route_C");
                SceneManager.UnloadSceneAsync("CloudFloor");
                SceneManager.UnloadSceneAsync("Character");
                SceneManager.UnloadSceneAsync("Smartphone");
                SceneManager.UnloadSceneAsync("IngameTimer");
                SceneManager.UnloadSceneAsync("Music");

                ScenesToLoad[ScenesToLoad.Count - 1].completed += _ =>
                {
                    HideFadeMenu(() =>
                    {
                        ScenesToLoad.Clear();
                        gameStateController.MusicBehaviour.Enable();
                    });
                };
            });
        }

        public override void Exit()
        {
            gameStateController.MusicBehaviour.Disable(fadeTime);
            ShowFadeMenu(() =>
            {
                gameStateController.CanvasManager.HideCanvas();

                ScenesToLoad.Add(SceneManager.LoadSceneAsync("WilmasRoom",LoadSceneMode.Additive));
                ScenesToLoad.Add(SceneManager.LoadSceneAsync("CollectableItems",LoadSceneMode.Additive));
                ScenesToLoad.Add(SceneManager.LoadSceneAsync("Route_A",LoadSceneMode.Additive));
                ScenesToLoad.Add(SceneManager.LoadSceneAsync("Route_B",LoadSceneMode.Additive));
                ScenesToLoad.Add(SceneManager.LoadSceneAsync("Route_C",LoadSceneMode.Additive));
                ScenesToLoad.Add(SceneManager.LoadSceneAsync("CloudFloor",LoadSceneMode.Additive));
                ScenesToLoad.Add(SceneManager.LoadSceneAsync("Character",LoadSceneMode.Additive));
                ScenesToLoad.Add(SceneManager.LoadSceneAsync("Smartphone",LoadSceneMode.Additive));
                ScenesToLoad.Add(SceneManager.LoadSceneAsync("IngameTimer",LoadSceneMode.Additive));
                ScenesToLoad.Add(SceneManager.LoadSceneAsync("Music",LoadSceneMode.Additive));

                ScenesToLoad[ScenesToLoad.Count - 1].completed +=  _ =>
                {
                    HideFadeMenu();
                };
            });
        }

        private void ShowFadeMenu(Action onCompleteAction = null)
        {
            gameStateController.FadeMenu.gameObject.SetActive(true);
            LeanTween.alphaCanvas(gameStateController.FadeMenu, 1f, fadeTime).setOnComplete(onCompleteAction);
        }
        
        private void HideFadeMenu(Action onfadeCompleteAction = null)
        {
            LeanTween.alphaCanvas(gameStateController.FadeMenu, 0f, fadeTime).setOnComplete(() =>
            {
                gameStateController.FadeMenu.gameObject.SetActive(false);
                onfadeCompleteAction?.Invoke();
            });
        }
    }
}
