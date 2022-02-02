
namespace Utils.StateMachine_Namespace
{
    public interface IStateManaged
    {
        void RequestState(IState requestedState);
    }
}
