using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.GameStates_Namespace.Scripts.States
{
    public class GameManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private GameStateController_SO gameStateController;
        [SerializeField] private PauseState_SO pauseState;
        [SerializeField] private CanvasManager canvasManager;
        [SerializeField] private CanvasGroup fadeMenu;
        [SerializeField] private MusicBehaviour musicBehaviour;

        private List<Scene> unloadedScenes = new List<Scene>();

        private void Awake()
        {
            LeanTween.init(1000);
            gameStateController.SetReferences(canvasManager, fadeMenu, musicBehaviour);
        }
        
        private void Start()
        {
            for (int i = 1; i < SceneManager.sceneCount; i++)
            {
                unloadedScenes.Add(SceneManager.GetSceneAt(i));
                SceneManager.UnloadSceneAsync(unloadedScenes[unloadedScenes.Count - 1]);
            }
        }

        private void OnDestroy()
        {
            foreach (var scene in unloadedScenes)
            {
                SceneManager.LoadSceneAsync(scene.name);
            }
            unloadedScenes.Clear();
        }

        public void Quit()
        {
            Application.Quit();
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (gameStateController.GetState() is PlayState_SO)
            {
                gameStateController.RequestState(pauseState);
            }
        }
    }
}
