using Features.Character_Namespace.Scripts.CharacterBehaviours;
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
		[Tooltip("Layer to control speed by floor")]
		[SerializeField] private LayerMask forceSpeedLayer;

		[Header("Jump")]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		[SerializeField] private float jumpTimeout = 0.2f;
    
		private float _jumpTimeoutDelta;
		private float _animationBlend_walkType;
		private float _rotationVelocity;
		private RaycastHit hit;

		protected override void Enter()
		{
			// reset our timeouts on start
			_jumpTimeoutDelta = jumpTimeout;
        
			// reset based on current input
			Input.jump = false;
			ApplySpeed(false);
			
			//start sound loop
			
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
			float inputMagnitude = Input.analogMovement ? Input.move.magnitude : 1f;
			
			//Set target animation blend
			float speed_targetAnimationBlend = 0f;
			float walkType_targetAnimationBlend = 0f;
			
			if (_manager.IsGroundedToLayer(forceSpeedLayer, out Collider floorCollider))
			{
				if (floorCollider.TryGetComponent(out ForceSpeedBehaviour forceSpeedBehaviour))
				{
					speed_targetAnimationBlend = forceSpeedBehaviour.GetTargetSpeed(Input.sprint);
					walkType_targetAnimationBlend = forceSpeedBehaviour.GetMovementType();
				}
				else
				{
					Debug.LogError($"You need to add the ForceSpeedBehaviour to {floorCollider.name}");
				}
			}
			else
			{
				speed_targetAnimationBlend = ForceSpeedBehaviour.GetTargetSpeed(Input.sprint, MovementSpeed.SlowRun, MovementSpeed.FastRun);
				walkType_targetAnimationBlend = (float) MovementType.Default;
			}

			if (Input.move == Vector2.zero) speed_targetAnimationBlend = (float) MovementSpeed.Stand;

			//set current animation blend
			_manager.Speed_AnimationBlend = useBlend ? Mathf.Lerp(_manager.Speed_AnimationBlend, speed_targetAnimationBlend, Time.deltaTime * speedChangeRate) : speed_targetAnimationBlend;
			_animationBlend_walkType = useBlend ? Mathf.Lerp(_animationBlend_walkType, walkType_targetAnimationBlend, Time.deltaTime * speedChangeRate) : walkType_targetAnimationBlend;

			// update animator if using character
			if (HasAnimator)
			{
				Animator.SetFloat(_animIDSpeed, _manager.Speed_AnimationBlend < 0.1 ? 0 : Mathf.Round(_manager.Speed_AnimationBlend * 100f) / 100f);
				Animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
		    
				Animator.SetFloat(_animIDWalkType, _animationBlend_walkType < 0.1 ? 0 : Mathf.Round(_animationBlend_walkType * 100) / 100);
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

			// apply gravity over time (multiply by delta time twice to linearly speed up over time)
			_manager.VerticalVelocity += _manager.gravity * Time.deltaTime;
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

		bool CheckOnTerrain()
		{
			if (hit.collider != null && hit.collider.CompareTag("Terrain"))
			{
				return true;
			} else {
				return false;
			}
		}
	}
}
