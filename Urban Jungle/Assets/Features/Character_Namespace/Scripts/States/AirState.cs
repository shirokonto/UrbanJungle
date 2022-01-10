using StarterAssets;
using UnityEngine;

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
    
    private readonly int animIDJump = Animator.StringToHash("Jump");
    private readonly int animIDFreeFall = Animator.StringToHash("FreeFall");
    
    private float fallTimeoutDelta;
    private float rotationVelocity;
    
    public override void Enter(GameObject gameObject)
    {
        base.Enter(gameObject);
        
        fallTimeoutDelta = fallTimeout;
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
        Vector3 targetDirection = Quaternion.Euler(0.0f, manager.targetRotation, 0.0f) * Vector3.forward;
        controller.Move(targetDirection * (manager.jumpSpeed * Time.deltaTime) + new Vector3(0.0f, manager.verticalVelocity, 0.0f) * Time.deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
        
        // update animator if using character
        if (hasAnimator)
        {
            animator.SetBool(animIDJump, false);
            animator.SetBool(animIDFreeFall, false);
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
        // fall timeout
        if (fallTimeoutDelta >= 0.0f)
        {
            fallTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            // update animator if using character
            if (hasAnimator)
            {
                animator.SetBool(animIDFreeFall, true);
            }
        }
        
        if (manager.verticalVelocity < ThirdPersonManager.terminalVelocity)
        {
            manager.verticalVelocity += manager.gravity * Time.deltaTime;
        }
    }
}
