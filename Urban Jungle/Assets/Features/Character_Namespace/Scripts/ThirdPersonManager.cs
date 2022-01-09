using UnityEngine;
using Utils.StateMachine_Namespace;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif


namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonManager : MonoBehaviour
	{
		[SerializeField][ReadOnly] private AnimatorState_SO currentState;
		
		[Header("Available States")]
		[SerializeField] private AnimatorState_SO groundedState;
		[SerializeField] private AnimatorState_SO airState;
		
		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		[SerializeField] private bool grounded = true;
		[Tooltip("Useful for rough ground")]
		[SerializeField] private float groundedOffset = -0.1f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		[SerializeField] private float groundedRadius = 0.2f;
		[Tooltip("What layers the character uses as ground")]
		[SerializeField] private LayerMask groundLayers;
		
		[Space(10)]
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float gravity = -15.0f;

		//References getter and setter
		public Animator animator { get; private set; }
		public CharacterController controller { get; private set; }
		public StarterAssetsInputs input { get; private set; }
		public GameObject mainCamera { get; private set; }
		
		//Values getter and setter
		public float jumpSpeed { get; set; }
		public float targetRotation { get; set; }
		public float verticalVelocity { get; set; }
		public static float terminalVelocity => 53.0f;
		
		
		private readonly int animIDGrounded = Animator.StringToHash("Grounded");
		
		private StateMachine stateMachine;

		private void Awake()
		{
			// get a reference to our main camera
			if (mainCamera == null)
			{
				mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			animator = GetComponent<Animator>();
			controller = GetComponent<CharacterController>();
			input = GetComponent<StarterAssetsInputs>();

			stateMachine = new StateMachine();
			stateMachine.Initialize(groundedState, gameObject);
			currentState = groundedState;
		}

		private void Update()
		{
			animator = GetComponent<Animator>();
			
			stateMachine.Update();

			GroundedCheck();
			switch (grounded)
			{
				case true when stateMachine.GetCurrentState() is AirState:
					RequestState(groundedState);
					break;
				case false when stateMachine.GetCurrentState() is GroundedState:
					RequestState(airState);
					break;
			}
		}

		private void OnAnimatorMove()
		{
			stateMachine.OnAnimatorMove();
		}

		private void RequestState(AnimatorState_SO stateAnimator)
		{
			if (((AnimatorState_SO) stateMachine.GetCurrentState()).IsValidStateShift(stateAnimator))
			{
				stateMachine.ChangeState(stateAnimator, gameObject);
				currentState = stateAnimator;
			}
		}
		
		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 position = transform.position;
			Vector3 spherePosition = new Vector3(position.x, position.y - groundedOffset, position.z);
			grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (animator != null)
			{
				animator.SetBool(animIDGrounded, grounded);
			}
		}
		
		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			Gizmos.color = grounded ? transparentGreen : transparentRed;
		
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Vector3 position = transform.position;
			Gizmos.DrawSphere(new Vector3(position.x, position.y - groundedOffset, position.z), groundedRadius);
		}
	}
}