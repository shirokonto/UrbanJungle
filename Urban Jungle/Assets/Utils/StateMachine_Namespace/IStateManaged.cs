
namespace Utils.StateMachine_Namespace
{
    public interface IStateManaged
    {
        void RequestState(IStateAnimator requestedStateAnimator);
    }
}
