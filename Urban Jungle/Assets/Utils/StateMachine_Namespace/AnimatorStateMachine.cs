
using UnityEngine;

namespace Utils.StateMachine_Namespace
{
    /// <summary>
    /// A MonoBehaviour who uses a StateMachine decides whether a State
    /// is changed or not based on Events and communicates it to the StateMachine
    /// and if it does it provides a new State (IState Object).
    /// It can also request to go back to a previous state.
    /// </summary>
    public class AnimatorStateMachine
    {
        public IStateAnimator currentStateAnimator { get; private set; }
        public IStateAnimator previousStateAnimator;

        public void Initialize(IStateAnimator startingStateAnimator, GameObject gameObject)
        {
            currentStateAnimator = startingStateAnimator;
            startingStateAnimator.Enter(gameObject);
        }

        public void ChangeState(IStateAnimator newStateAnimator, GameObject gameObject)
        {
            currentStateAnimator?.Exit();
            previousStateAnimator = currentStateAnimator;
            currentStateAnimator = newStateAnimator;
            currentStateAnimator.Enter(gameObject);
        }

        public IStateAnimator GetCurrentState()
        {
            return currentStateAnimator;
        }
        
        public IStateAnimator GetPreviousState()
        {
            return previousStateAnimator;
        }

        public void Update()
        {
            currentStateAnimator?.Execute();
        }

        public void OnAnimatorMove()
        {
            currentStateAnimator?.OnAnimatorMove();
        }

        public void SwitchToPreviousState(GameObject gameObject)
        {
            currentStateAnimator.Exit();
            currentStateAnimator = previousStateAnimator;
            currentStateAnimator.Enter(gameObject);
        }
    }
}