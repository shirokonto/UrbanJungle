using System.Collections.Generic;
using UnityEngine;
using Utils.StateMachine_Namespace;

namespace Features.GameStates_Namespace.Scripts
{
    public abstract class State_SO : ScriptableObject, IState
    {
        [field: SerializeField] protected List<State_SO> validStateShifts;

        public bool IsValidStateShift(State_SO requestedState)
        {
            if (validStateShifts.Contains(requestedState)) return true;
            
            Debug.LogWarning($"Swapping to the state {requestedState} is not possible because it is not added to the {name} List!");
            return false;
        }
        
        public abstract void Enter();

        public virtual void Execute() { }

        public virtual void Exit() { }
    }
}
