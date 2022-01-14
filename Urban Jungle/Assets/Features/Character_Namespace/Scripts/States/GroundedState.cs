using StarterAssets;
using UnityEngine;

namespace Features.Character_Namespace.Scripts.States
{
	[CreateAssetMenu]
	public class GroundedState : AnimatorState_SO
	{
		[Header("Movement")]
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		[SerializeField] private float rotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		[SerializeField] private float speedChangeRate = 10.0f;
		[Tooltip("The Layer for crouching on objects")]
		[SerializeField] private LayerMask crouchLayer;
		[Tooltip("The Layer for force the character into a walk")]
		[SerializeField] private LayerMask walkLayer;
		[Tooltip("Whether the Character is forced to walk or not")]
		[SerializeField] private bool forceWalk;

		[Header("Jump")]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		[SerializeField] private float jumpTimeout = 0.2f;
    
		private float _jumpTimeoutDelta;
		private float _animationBlend_walkType;
		private float _rotationVelocity;

		protected override void Enter()
		{
			// reset our timeouts on start
			_jumpTimeoutDelta = jumpTimeout;
        
			// reset based on current input
			Input.jump = false;
			ApplySpeed(false);
			
			// update animator if using character
			if (Animator != null)
			{
				Animator.SetBool(_animIDGrounded, true);
			}
		}

		public override void Execute()
		{
			ApplyGravity();
			ApplySpeed(true);
			ApplyRotation();
	    
			Jump();
		}
    
		public override void OnAnimatorMove()
		{
			Vector3 velocity = Animator.deltaPosition;
			velocity.y = _manager.VerticalVelocity * Time.deltaTime;
			Controller.Move(velocity);
		}

		public override void Exit()
		{
			base.Exit();
	    
			// update animator if using character
			if (HasAnimator)
			{
				Animator.SetBool(_animIDGrounded, false);
			}
		}

		private void ApplySpeed(bool useBlend)
		{
			// set target speed based on inputs
			float speed_targetAnimationBlend = Input.sprint ? AnimBlendThreshold_FastRun : AnimBlendThreshold_SlowRun;

			bool isGroundedToCrouchLayer = _manager.IsGroundedToLayer(crouchLayer, out Collider _);
			float walkType_targetAnimationBlend = isGroundedToCrouchLayer ? AnimBlendThreshold_Crouch : AnimBlendThreshold_DefaultMovement;

			if (_manager.IsGroundedToLayer(walkLayer, out Collider _) || forceWalk || isGroundedToCrouchLayer)
			{
				speed_targetAnimationBlend = AnimBlendThreshold_Walk;
			}

			if (Input.move == Vector2.zero) speed_targetAnimationBlend = AnimBlendThreshold_StandIdle;
	    
			float inputMagnitude = Input.analogMovement ? Input.move.magnitude : 1f;
			
			//set blend
			_manager.Speed_AnimationBlend = useBlend ? Mathf.Lerp(_manager.Speed_AnimationBlend, speed_targetAnimationBlend, Time.deltaTime * speedChangeRate) : speed_targetAnimationBlend;
			_animationBlend_walkType = useBlend ? Mathf.Lerp(_animationBlend_walkType, walkType_targetAnimationBlend, Time.deltaTime * speedChangeRate) : walkType_targetAnimationBlend;

			// update animator if using character
			if (HasAnimator)
			{
				Animator.SetFloat(_animIDSpeed, _manager.Speed_AnimationBlend);
				Animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
		    
				Animator.SetFloat(_animIDWalkType, _animationBlend_walkType);
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
			// stop our velocity dropping infinitely when grounded
			if (_manager.VerticalVelocity < 0.0f)
			{
				_manager.VerticalVelocity = -2f;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_manager.VerticalVelocity < ThirdPersonManager.TerminalVelocity)
			{
				_manager.VerticalVelocity += _manager.gravity * Time.deltaTime;
			}
		}

		private void Jump()
		{
			// jump timeout
			if (_jumpTimeoutDelta >= 0.0f)
			{
				_jumpTimeoutDelta -= Time.deltaTime;
			}

			// Jump
			if (Input.jump && _jumpTimeoutDelta <= 0.0f)
			{
				_manager.RequestState(_manager.jumpState);
			}
		}
	}
}
