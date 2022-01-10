using UnityEngine;

namespace Utils.StateMachine_Namespace
{
    public interface IStateAnimator
    {
        //initialization to the beginning
        void Enter(GameObject gameObject);

        //doing stuff continuously
        void Execute();

        //doing stuff continuously for Unity's OnAnimatorMove event method
        void OnAnimatorMove();

        // what to do if machine kicks it out
        void Exit();

    }
}