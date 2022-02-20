using Features.GameStates_Namespace.Scripts;
using UnityEngine;

namespace Features.Character_Namespace.Scripts.States
{
    [CreateAssetMenu]
    public class JumpState : AnimatorState_SO
    {
        [SerializeField] private float exitTime;
        [SerializeField] private AnimatorState_SO exitTimeStateSwitch;
        
        [Tooltip("The height the player can jump")]
        [SerializeField] private float jumpHeight = 1.2f;

        [SerializeField] private float MaxAirSpeed = 10f;

        private float _exitTimeDelta;
        
        protected override void Enter()
        {
            _exitTimeDelta = 0;
            
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            _manager.VerticalVelocity = Mathf.Sqrt(jumpHeight * -2f * _manager.gravity);

            Vector3 velocity = Controller.velocity;
            float magnitude = new Vector3(velocity.x, 0f, velocity.z).magnitude;
            _manager.JumpSpeed = Mathf.Clamp(magnitude, 1f, MaxAirSpeed);

            // update animator if using character
            if (HasAnimator)
            {
                Animator.SetTrigger(_animIDJump);
            }
        }

        public override void Execute()
        {
            base.Execute();
            
            ApplyGravity();

            _exitTimeDelta += Time.deltaTime;
            if (_exitTimeDelta >= exitTime)
            {
                _manager.RequestState(exitTimeStateSwitch);
            }
        }
        
        public override void OnAnimatorMove()
        {
            Vector3 targetDirection = Quaternion.Euler(0.0f, _manager.TargetRotation, 0.0f) * Vector3.forward;
            Controller.Move(targetDirection * (_manager.JumpSpeed * Time.deltaTime) + new Vector3(0.0f, _manager.VerticalVelocity, 0.0f) * Time.deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
            
            // update animator if using character
            if (HasAnimator)
            {
                Animator.SetBool(_animIDJump, false);
            }
        }

        private void ApplyGravity()
        {
            _manager.VerticalVelocity += _manager.gravity * Time.deltaTime;
        }
    }
}
