using Features.GameStates_Namespace.Scripts.States;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Event_Namespace;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
#endif

namespace Features.InputSystem
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[SerializeField] private GameStateController_SO gameStateController;
		[SerializeField] private PauseState_SO pauseState;
		[SerializeField] private PlayState_SO playState;
		[SerializeField] private GameEvent onHandyButtonPressed;
		
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnPause(InputValue value)
		{
			if (gameStateController.GetState() is PauseState_SO)
			{
				gameStateController.RequestState(playState);
			}
			else if (gameStateController.GetState() is PlayState_SO)
			{
				gameStateController.RequestState(pauseState);
			}
		}

		public void OnHandyButton(InputValue value)
		{
			onHandyButtonPressed?.Raise();
		}

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}