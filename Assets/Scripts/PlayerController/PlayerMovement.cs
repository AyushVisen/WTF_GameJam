using UnityEngine;

namespace WTF_GameJam.Player
{
	public enum LookInputMode
	{
		MouseMove,
		MouseDownAndMove,
	}

	public class PlayerMovement : MonoBehaviour
	{
		[field: SerializeField]
		public KCC CharacterController { get; private set; }

		[field: SerializeField]
		public Transform PlayerAvatarRoot { get; private set; }

		[field: SerializeField]
		public LayerMask MouseHitLayers { get; private set; }

		[field: SerializeField]
		public float NormalSpeed { get; private set; }

		[field: SerializeField]
		public float SprintSpeed { get; private set; }

		[field: SerializeField]
		public LookInputMode LookInputMode { get; private set; }

		public Vector3 LookDirection { get; private set; }
		public Vector3 MoveInput { get; private set; }
		public Vector3 Velocity => CharacterController.Motor.BaseVelocity;

		private PlayerInputSystem _playerInputSystem;

		private void Awake()
		{
			_playerInputSystem = new PlayerInputSystem();
			_playerInputSystem.Player.Enable();
		}

		private void OnDestroy()
		{
			_playerInputSystem.Player.Disable();
		}

		private void Update()
		{
			HandleCharacterInput();
		}

		private void HandleCharacterInput()
		{
			var characterInputs = new PlayerCharacterInputs();
			MoveInput = _playerInputSystem.Player.Move.ReadValue<Vector2>();
			var jumpInput = _playerInputSystem.Player.Jump.IsPressed();
			var crouchInput = _playerInputSystem.Player.Crouch.IsPressed();
			var mousePosition = _playerInputSystem.Player.MousePosition.ReadValue<Vector2>();
			var sprintInput = _playerInputSystem.Player.Sprint.IsPressed();
			var attackInput = _playerInputSystem.Player.Attack.IsPressed();

			var lookDirection = Vector3.zero;

			CharacterController.MaxStableMoveSpeed = SprintSpeed;

			if (LookInputMode == LookInputMode.MouseMove)
			{
				lookDirection = GetLookDirection( mousePosition );
			}
			else
			{
				if (attackInput)
				{
					lookDirection = GetLookDirection( mousePosition );
				}
				else
				{
					lookDirection = CharacterController.Motor.BaseVelocity.normalized;
				}
			}

			if (lookDirection.sqrMagnitude > 0)
			{
				PlayerAvatarRoot.transform.forward = new Vector3( lookDirection.x, 0, lookDirection.z );
			}

			LookDirection = lookDirection;

			// Build the CharacterInputs struct
			characterInputs.MoveAxisForward = MoveInput.y;
			characterInputs.MoveAxisRight = MoveInput.x;
			characterInputs.JumpDown = jumpInput;
			characterInputs.CrouchDown = crouchInput;
			characterInputs.CrouchUp = !crouchInput;

			// Apply inputs to character
			CharacterController.SetInputs( ref characterInputs );
		}

		private Vector3 GetLookDirection( Vector2 mousePosition )
		{
			var lookDirection = Vector3.zero;
			var mouseRay = Camera.main.ScreenPointToRay( mousePosition );
			var didMouseHit = Physics.Raycast( mouseRay, out var hit, Mathf.Infinity, MouseHitLayers );

			if (didMouseHit)
			{
				lookDirection = hit.point - PlayerAvatarRoot.position;
				lookDirection.y = 0;
			}

			return lookDirection.normalized;
		}
	}
}