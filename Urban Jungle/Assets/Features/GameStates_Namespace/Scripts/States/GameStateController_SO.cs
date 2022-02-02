using System.Collections.Generic;
using UnityEngine;

namespace Features.GameStates_Namespace.Scripts.States
{
    [CreateAssetMenu(fileName = "StateController", menuName = "GameStates/StateController")]
    public class GameStateController_SO : StateController_SO
    {
        public CanvasManager CanvasManager { get; private set; }
        public CanvasGroup FadeMenu { get; private set; }
        public MusicBehaviour MusicBehaviour { get; private set; }
        public List<AsyncOperation> ScenesToLoad { get; set; }

        public void SetReferences(CanvasManager canvasManager, CanvasGroup fadeMenu, MusicBehaviour musicBehaviour)
        {
            CanvasManager = canvasManager;
            FadeMenu = fadeMenu;
            MusicBehaviour = musicBehaviour;
            
            ScenesToLoad = new List<AsyncOperation>();

            Initialize();
        }
    }
}
