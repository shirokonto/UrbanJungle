using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Features.Character_Namespace.Scripts.States;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.StateMachine_Namespace;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
#endif


namespace Features.Character_Namespace.Scripts
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonManager : MonoBehaviour
	{
		// ReSharper disable once NotAccessedField.Local
		[SerializeField][ReadOnly] private AnimatorState_SO currentState;
		
		[Header("Available States")]
		[SerializeField] private GroundedState groundedState;
		[SerializeField] private AirState airState;
		[SerializeField] private LadderState ladderAnimatorState;
		[SerializeField] private SeekState seekAnimatorState;
		
		[Header("Character")]
		[SerializeField] public Transform hipsRoot;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float gravity = -15.0f;
		
		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		[SerializeField] private bool grounded = true;
		[Tooltip("Useful for rough ground")]
		[SerializeField] private float groundedOffset = -0.1f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		[SerializeField] private float groundedRadius = 0.2f;
		[Tooltip("What layers the character uses as ground")]
		[SerializeField] private LayerMask groundLayers;

		[Header("Debug")]
		public Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
		public Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

		//References getter and setter
		public Animator Animator { get; private set; }
		public CharacterController Controller { get; private set; }
		public StarterAssetsInputs Input { get; private set; }
		public GameObject MainCamera { get; private set; }
		
		//Values getter and setter
		public float JumpSpeed { get; set; }
		public float Speed_AnimationBlend { get; set; }
		public float TargetRotation { get; set; }
		public float VerticalVelocity { get; set; }
		public static float TerminalVelocity => 53.0f;
		
		private readonly int _animIDGrounded = Animator.StringToHash("Grounded");

		private Collider[] _groundedColliders;
		private StateMachine _stateMachine;

		private void Awake()
		{
			// get a reference to our main camera
			if (MainCamera == null)
			{
				MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			Animator = GetComponent<Animator>();
			Controller = GetComponent<CharacterController>();
			Input = GetComponent<StarterAssetsInputs>();

			_stateMachine = new StateMachine();
			_stateMachine.Initialize(groundedState, gameObject);
			currentState = groundedState;
		}

		private void Update()
		{
			Animator = GetComponent<Animator>();
			GroundedCheck();
			
			_stateMachine.Update();
			
			switch (grounded)
			{
				case true when _stateMachine.GetCurrentState() is AirState:
					RequestState(groundedState);
					break;
				case false when _stateMachine.GetCurrentState() is GroundedState:
					RequestState(airState);
					break;
			}
		}

		private void OnTriggerEnter(Collider trigger)
		{
			if (trigger.TryGetComponent(typeof(SeekTriggerBehaviour), out Component seekTriggerBehaviour))
			{
				seekAnimatorState.SetNextState(seekTriggerBehaviour as SeekTriggerBehaviour);
				RequestState(seekAnimatorState);
			}
		}

		private void OnAnimatorMove()
		{
			_stateMachine.OnAnimatorMove();
		}

		[SuppressMessage("ReSharper", "Unity.PreferNonAllocApi")]
		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 position = transform.position;
			Vector3 spherePosition = new Vector3(position.x, position.y - groundedOffset, position.z);
			
			_groundedColliders = Physics.OverlapSphere(spherePosition, groundedRadius, groundLayers,
				QueryTriggerInteraction.Ignore);

			grounded = _groundedColliders.Length != 0;

			// update animator if using character
			if (Animator != null)
			{
				Animator.SetBool(_animIDGrounded, grounded);
			}
		}
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = grounded ? transparentGreen : transparentRed;
		
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Vector3 position = transform.position;
			Gizmos.DrawSphere(new Vector3(position.x, position.y - groundedOffset, position.z), groundedRadius);
		}

		public bool IsGroundedToLayer(LayerMask layerMask)
		{
			if (_groundedColliders == null) return false;
			
			//converting from layer to layerMask: 1 << layer = layerMask
			return grounded && _groundedColliders.Any(groundedCollider => 1 << groundedCollider.gameObject.layer == layerMask);
		}
		
		public void RequestState(AnimatorState_SO stateAnimator)
		{
			if (((AnimatorState_SO) _stateMachine.GetCurrentState()).IsValidStateShift(stateAnimator))
			{
				_stateMachine.ChangeState(stateAnimator, gameObject);
				currentState = stateAnimator;
			}
		}

		public void EnterGroundedState()
		{
			RequestState(groundedState);
		}
	}
}