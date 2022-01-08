using StarterAssets;
using UnityEngine;

[CreateAssetMenu]
public class GroundedState : AnimatorState_SO
{
	[Header("Movement")]
	[Tooltip("How fast the character turns to face movement direction")]
	[Range(0.0f, 0.3f)]
	[SerializeField] private float rotationSmoothTime = 0.12f;
	[Tooltip("Acceleration and deceleration")]
	[SerializeField] private float speedChangeRate = 10.0f;
	[Tooltip("Whether the Character is forced to walk or not")]
	[SerializeField] private bool forceWalk = false;

	[Header("Jump")]
    [Tooltip("The height the player can jump")]
	[SerializeField] private float jumpHeight = 1.2f;
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    [SerializeField] private float jumpTimeout = 0.2f;
    
    private float jumpTimeoutDelta;
    private float animationBlend;
    private float rotationVelocity;

    private float animBlendThreshold_StandIdle => 0;
    private float animBlendThreshold_Walk => 1;
    private float animBlendThreshold_SlowRun => 2;
    private float animBlendThreshold_FastRun => 3;
    
    // animation IDs
    private readonly int animIDSpeed = Animator.StringToHash("Speed");
    private readonly int animIDJump = Animator.StringToHash("Jump");
    private readonly int animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

    public override void Enter(GameObject gameObject)
    {
	    base.Enter(gameObject);
	    
	    // reset our timeouts on start
        jumpTimeoutDelta = jumpTimeout;
        
        // reset based on current input
        input.jump = false;
        ApplySpeed(false);
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
	    Vector3 velocity = animator.deltaPosition;
	    velocity.y = manager.verticalVelocity * Time.deltaTime;
	    controller.Move(velocity);
    }

    public override void Exit()
    {
	    base.Exit();
	    
	    // if we are not grounded, do not jump
	    input.jump = false;
    }

    private void ApplySpeed(bool useBlend)
    {
	    // set target speed based on inputs
	    float targetAnimationBlend = input.sprint ? animBlendThreshold_FastRun : animBlendThreshold_SlowRun;
	    targetAnimationBlend = forceWalk ? animBlendThreshold_Walk : targetAnimationBlend;
	    
	    if (input.move == Vector2.zero) targetAnimationBlend = animBlendThreshold_StandIdle;
	    float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;
	    
	    //set blend
	    animationBlend = useBlend ? Mathf.Lerp(animationBlend, targetAnimationBlend, Time.deltaTime * speedChangeRate) : targetAnimationBlend;

	    // update animator if using character
	    if (hasAnimator)
	    {
		    animator.SetFloat(animIDSpeed, animationBlend);
		    animator.SetFloat(animIDMotionSpeed, inputMagnitude);
	    }
    }
    
    private void ApplyRotation()
    {
	    // normalise input direction
	    Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

	    // if there is a move input rotate player when the player is moving
	    if (input.move != Vector2.zero)
	    {
		    manager.targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
		    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, manager.targetRotation, ref rotationVelocity, rotationSmoothTime);

		    // rotate to face input direction relative to camera position
		    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
	    }
    }

    private void ApplyGravity()
    {
	    // stop our velocity dropping infinitely when grounded
	    if (manager.verticalVelocity < 0.0f)
	    {
		    manager.verticalVelocity = 0f;
	    }

	    // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
	    if (manager.verticalVelocity < ThirdPersonManager.terminalVelocity)
	    {
		    manager.verticalVelocity += manager.gravity * Time.deltaTime;
	    }
    }

	private void Jump()
	{
		// jump timeout
		if (jumpTimeoutDelta >= 0.0f)
		{
			jumpTimeoutDelta -= Time.deltaTime;
		}

		// Jump
		if (input.jump && jumpTimeoutDelta <= 0.0f)
		{
			// the square root of H * -2 * G = how much velocity needed to reach desired height
			manager.verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * manager.gravity);
			Vector3 velocity = controller.velocity;
			manager.jumpSpeed = new Vector3(velocity.x, 0f, velocity.z).magnitude;

			// update animator if using character
			if (hasAnimator)
			{
				animator.SetBool(animIDJump, true);
			}
			
			// prevent more jump iterations
			input.jump = false;
		}
	}
}
