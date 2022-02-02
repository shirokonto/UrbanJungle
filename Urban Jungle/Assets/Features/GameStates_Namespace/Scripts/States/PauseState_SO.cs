using UnityEngine;

namespace Features.GameStates_Namespace.Scripts.States
{
    [CreateAssetMenu(fileName = "PauseState", menuName = "GameStates/Pause")]
    public class PauseState_SO : State_SO
    {
        [SerializeField] protected GameStateController_SO gameStateController;
        [SerializeField] private MenuType_SO pauseScreen;
        
        public override void Enter()
        {
            gameStateController.CanvasManager.AddCanvas(pauseScreen);
            Time.timeScale = 0f;
        }

        public override void Exit()
        {
            gameStateController.CanvasManager.RemoveCanvas();
            gameStateController.CanvasManager.HideCanvas();
            Time.timeScale = 1f;
        }
    }
}
