using UnityEngine;
using Utils.StateMachine_Namespace;

namespace Features.GameStates_Namespace.Scripts
{
    public class StateController_SO : ScriptableObject
    {
        [SerializeField] private State_SO startingState;
        
        private StateMachine stateMachine;
        
        protected void Initialize()
        {
            stateMachine = new StateMachine();
            stateMachine.Initialize(startingState);
        }
        
        public void RequestState(State_SO state)
        {
            if (((State_SO) GetState()).IsValidStateShift(state))
            {
                stateMachine.ChangeState(state);
            }
        }

        public IState GetState()
        {
            return stateMachine.GetCurrentState();
        }

        public void UpdateState()
        {
            stateMachine.Update();
        }
    }
}
