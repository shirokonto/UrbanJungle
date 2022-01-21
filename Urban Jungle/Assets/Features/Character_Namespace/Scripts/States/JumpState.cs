using UnityEditor.Sprites;
using UnityEngine;

namespace Features.Character_Namespace.Scripts.States
{
    [CreateAssetMenu]
    public class JumpState : AnimatorState_SO
    {
        [Tooltip("The height the player can jump")]
        [SerializeField] private float jumpHeight = 1.2f;
        
        protected override void Enter()
        {
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            _manager.VerticalVelocity = Mathf.Sqrt(jumpHeight * -2f * _manager.gravity);

            Vector3 velocity = Controller.velocity;
            _manager.JumpSpeed = new Vector3(velocity.x, 0f, velocity.z).magnitude;
            //Mathf.Clamp(magnitude, (float) MovementSpeed.Stand, (float) MovementSpeed.FastRun);

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
            if (_manager.VerticalVelocity < ThirdPersonManager.TerminalVelocity)
            {
                _manager.VerticalVelocity += _manager.gravity * Time.deltaTime;
            }
        }
    }
}
