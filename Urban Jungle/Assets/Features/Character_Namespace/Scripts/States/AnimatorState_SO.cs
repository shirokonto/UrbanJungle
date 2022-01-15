using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Utils.StateMachine_Namespace;

namespace Features.Character_Namespace.Scripts.States
{
    public abstract class AnimatorState_SO : ScriptableObject, IStateAnimator
    {
        [field: SerializeField] protected List<AnimatorState_SO> validStateShifts;

        //properties
        protected bool HasAnimator => _manager.Animator != null;
        protected Animator Animator => _manager.Animator;
        protected CharacterController Controller => _manager.Controller;
        protected StarterAssetsInputs Input => _manager.Input;
        protected GameObject GameObject => _manager.MainCamera;
    
        protected static float AnimBlendThreshold_DefaultMovement => 0;
        protected static float AnimBlendThreshold_Crouch => 1;
        protected static float AnimBlendThreshold_StandIdle => 0;
        protected static float AnimBlendThreshold_Walk => 1;
        protected static float AnimBlendThreshold_SlowRun => 2;
        protected static float AnimBlendThreshold_FastRun => 3;
    
        //fields
        protected Transform _transform;
        protected ThirdPersonManager _manager;
    
        protected readonly int _animIDSpeed = Animator.StringToHash("Speed");
        protected readonly int _animIDWalkType = Animator.StringToHash("WalkType");
        protected readonly int _animIDJump = Animator.StringToHash("Jump");
        protected readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        protected readonly int _animIDFreeFall = Animator.StringToHash("FreeFall");
        protected readonly int _animIDClimbLadder = Animator.StringToHash("ClimbLadder");
        protected readonly int _animIDClimbToLadderTop = Animator.StringToHash("ClimbToLadderTop");
        protected readonly int _animIDGrounded = Animator.StringToHash("Grounded");
    
    
        public bool IsValidStateShift(AnimatorState_SO requestedStateAnimator)
        {
            if (!validStateShifts.Contains(requestedStateAnimator))
            {
                Debug.LogWarning($"The state shift was not possible because {this.name} doesn't contain {requestedStateAnimator.name} in the validStateShift list");
                return false;
            }
            return true;
        }
        
        protected virtual void Enter() { }
        

        public void Enter(GameObject gameObject)
        {
            _manager = gameObject.GetComponent<ThirdPersonManager>();
            _transform = gameObject.transform;
            
            Enter();
        }

        public virtual void Execute() { }
    
        public virtual void OnAnimatorMove() { }

        public virtual void Exit() { }
    }
}
