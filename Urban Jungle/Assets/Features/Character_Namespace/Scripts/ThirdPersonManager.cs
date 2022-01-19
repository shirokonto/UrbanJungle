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
		[SerializeField] public GroundedState groundedState;
		[SerializeField] public AirState airState;
		[SerializeField] public JumpState jumpState;
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
		[SerializeField] public LayerMask bounceLayer;

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
					if (IsGroundedToLayer(bounceLayer, out Collider floorCollider))
					{
						if (floorCollider.TryGetComponent(out BounceBehaviour bounceBehaviour))
						{
							bounceBehaviour.ApplyBounce(this);
						}
						else
						{
							Debug.LogError($"You need to add the BounceBehaviour to {floorCollider.name}");
						}
					}
					else
					{
						RequestState(groundedState);
					}
					break;
				case false when _stateMachine.GetCurrentState() is GroundedState || _stateMachine.GetCurrentState() is JumpState:
					RequestState(airState);
					break;
			}
		}

		private void OnAnimatorMove()
		{
			_stateMachine.OnAnimatorMove();
		}
		
		private void OnTriggerEnter(Collider trigger)
		{
			if (trigger.TryGetComponent(typeof(SeekTriggerBehaviour), out Component seekTriggerBehaviour))
			{
				seekAnimatorState.SetNextState(seekTriggerBehaviour as SeekTriggerBehaviour);
				RequestState(seekAnimatorState);
			}
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
		}
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = grounded ? transparentGreen : transparentRed;
		
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Vector3 position = transform.position;
			Gizmos.DrawSphere(new Vector3(position.x, position.y - groundedOffset, position.z), groundedRadius);
		}

		public bool IsGrounded() => grounded;

		public bool IsGroundedToLayer(LayerMask layerMask, out Collider floorCollider)
		{
			floorCollider = null;
			if (_groundedColliders == null) return false;
			
			//converting from layer to layerMask: 1 << layer = layerMask
			bool any = false;
			foreach (Collider groundedCollider in _groundedColliders)
			{
				if (1 << groundedCollider.gameObject.layer == layerMask)
				{
					floorCollider = groundedCollider;
					any = true;
					break;
				}
			}

			return grounded && any;
		}
		
		public void RequestState(AnimatorState_SO requestedState)
		{
			if (((AnimatorState_SO) _stateMachine.GetCurrentState()).IsValidStateShift(requestedState))
			{
				//Debug.Log(requestedState.name);
				_stateMachine.ChangeState(requestedState, gameObject);
				currentState = requestedState;
			}
		}
	}
}