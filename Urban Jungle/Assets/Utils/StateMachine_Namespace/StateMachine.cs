
namespace Utils.StateMachine_Namespace
{
    /// <summary>
    /// A MonoBehaviour who uses a StateMachine decides whether a State
    /// is changed or not based on Events and communicates it to the StateMachine
    /// and if it does it provides a new State (IState Object).
    /// It can also request to go back to a previous state.
    /// </summary>
    public class StateMachine
    {
        public IState currentState { get; private set; }
        public IState previousState;

        public void Initialize(IState startingState)
        {
            currentState = startingState;
            startingState.Enter();
        }

        public void ChangeState(IState newState)
        {
            currentState?.Exit();
            previousState = currentState;
            currentState = newState;
            currentState.Enter();
        }

        public IState GetCurrentState()
        {
            return currentState;
        }

        public void Update()
        {
            currentState?.Execute();
        }

        public void SwitchToPreviousState()
        {
            currentState.Exit();
            currentState = previousState;
            currentState.Enter();
        }
    }
}