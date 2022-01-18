using StarterAssets;
using UnityEngine;

namespace Features.Character_Namespace.Scripts.States
{
    [CreateAssetMenu]
    public class AirState : AnimatorState_SO
    {
        [Header("Rotation in Air")]
        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [SerializeField] private float rotationSmoothTime = 0.12f;
        [Tooltip("Should the character be able to move the flight direction")]
        [SerializeField] private bool enableRotation = true;

        [Header("Fall")]
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float fallTimeout = 0.3f;

        private float _fallTimeoutDelta;
        private float _rotationVelocity;
        
        protected override void Enter()
        {
            _fallTimeoutDelta = fallTimeout;
            
            Vector3 velocity = Controller.velocity;
            float magnitude = new Vector3(velocity.x, 0f, velocity.z).magnitude;
            _manager.JumpSpeed = Mathf.Clamp(magnitude, (float) MovementSpeed.Stand, (float) MovementSpeed.FastRun);
        }

        public override void Execute()
        {
            base.Execute();
        
            ApplyGravity();
            if (enableRotation)
            {
                ApplyRotation();
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
                Animator.SetBool(_animIDFreeFall, false);
            }
        }

        private void ApplyRotation()
        {
            // normalise input direction
            Vector3 inputDirection = new Vector3(Input.move.x, 0.0f, Input.move.y).normalized;

            // if there is a move input rotate player when the player is moving
            if (Input.move != Vector2.zero)
            {
                _manager.TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + GameObject.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(_transform.eulerAngles.y, _manager.TargetRotation, ref _rotationVelocity, rotationSmoothTime);

                // rotate to face input direction relative to camera position
                _transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }

        private void ApplyGravity()
        {
            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                if (HasAnimator)
                {
                    Animator.SetBool(_animIDFreeFall, true);
                }
            }
        
            if (_manager.VerticalVelocity < ThirdPersonManager.TerminalVelocity)
            {
                _manager.VerticalVelocity += _manager.gravity * Time.deltaTime;
            }
        }
    }
}
