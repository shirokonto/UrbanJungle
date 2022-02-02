using UnityEngine;

namespace Features.GameStates_Namespace.Scripts.States
{
    [CreateAssetMenu(fileName = "PlayState", menuName = "GameStates/Play")]
    public class PlayState_SO : State_SO
    {
        public override void Enter()
        {
            Debug.Log("enter playstate");
        }
    }
}
