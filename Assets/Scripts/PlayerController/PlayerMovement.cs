using UnityEngine;
using WTF_GameJam.Health;

namespace WTF_GameJam.Player
{
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
		public float DashSpeed { get; private set; }

		[field: SerializeField]
		public float DashTime { get; private set; }

		[field: SerializeField]
		public float DashCooldownTime { get; private set; }

		[field: SerializeField]
		public float SwordSwingDamageRange { get; private set; }

		[field: SerializeField]
		public float AoeCoolDownTime { get; private set; }

		public UIHandler UIHandler { get; private set; }
		public HealthBehavior HealthBehavior { get; private set; }

		public Vector3 LookDirection { get; private set; }
		public Vector3 MoveInput { get; private set; }
		public bool SwingAttackInput { get; private set; }
		public bool AOEAttackInput { get; private set; }
		public Vector3 Velocity => CharacterController.Motor.BaseVelocity;
		public bool IsDashing => _dashTimeRemaining > 0f;
		public bool IsAttacking { get; private set; }
		public TypeOfAttack CurrentAttackType { get; private set; }

		private PlayerInputSystem _playerInputSystem;
		private float _dashTimeRemaining;
		private float _dashCooldownTime;
		private float _aoeCoolDownTimeRemaining;

		private void Awake()
		{
			_playerInputSystem = new PlayerInputSystem();
			_playerInputSystem.Player.Enable();
		}

		private void Start()
		{
			HealthBehavior = GetComponent<HealthBehavior>();
			UIHandler = FindFirstObjectByType<UIHandler>();
			UIHandler.PlayerHealthBehaviour = HealthBehavior;
			CurrentAttackType = TypeOfAttack.None;
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
			var crouchInput = _playerInputSystem.Player.Crouch.IsPressed();
			var mousePosition = _playerInputSystem.Player.MousePosition.ReadValue<Vector2>();
			var dashInput = _playerInputSystem.Player.Dash.IsPressed();
			SwingAttackInput = _playerInputSystem.Player.SwingAttack.IsPressed() && !IsDashing && !IsAttacking;
			AOEAttackInput = _playerInputSystem.Player.AOEAttack.IsPressed() && !IsDashing && !IsAttacking;

			if(_aoeCoolDownTimeRemaining > 0f)
			{
				_aoeCoolDownTimeRemaining -= Time.deltaTime;
				AOEAttackInput = false;
				if (UIHandler != null && UIHandler.AoeCoolDownTimerUI != null)
				{
					UIHandler.AoeCoolDownTimerUI.fillAmount = (1 - _aoeCoolDownTimeRemaining / AoeCoolDownTime);
				}
			}

			if (AOEAttackInput)
			{
				if(_aoeCoolDownTimeRemaining <= 0f)
				{
					_aoeCoolDownTimeRemaining = AoeCoolDownTime;
				}
			}

			if (CurrentAttackType == TypeOfAttack.None)
			{
				if(SwingAttackInput)
				{
					CurrentAttackType = TypeOfAttack.SwordSwing;
				}
				if(AOEAttackInput)
				{
					CurrentAttackType = TypeOfAttack.AOE;
				}
			}

			var lookDirection = CharacterController.Motor.BaseVelocity.normalized;
			
			if (lookDirection.sqrMagnitude > 0)
			{
				PlayerAvatarRoot.transform.forward = new Vector3( lookDirection.x, 0, lookDirection.z );
			}

			float moveSpeed = 0;

			if(IsAttacking == false)
			{
				if (dashInput)
				{
					if(_dashTimeRemaining <= 0f && _dashCooldownTime <= 0f)
					{
						_dashTimeRemaining = DashTime;
					}
				}
				else
				{
					moveSpeed = NormalSpeed;
				}

				if (_dashTimeRemaining > 0f)
				{
					_dashTimeRemaining -= Time.deltaTime;

					if(_dashTimeRemaining <= 0f)
					{
						_dashCooldownTime = DashCooldownTime;
					}

					MoveInput = new Vector2( PlayerAvatarRoot.transform.forward.x, PlayerAvatarRoot.transform.forward.z );
					moveSpeed = DashSpeed;
				}
			}

			if(_dashCooldownTime > 0)
			{
				_dashCooldownTime -= Time.deltaTime;

				if(UIHandler != null && UIHandler.DashCoolDownTimerUI != null)
				{
					UIHandler.DashCoolDownTimerUI.fillAmount = (1 - _dashCooldownTime / DashCooldownTime);
				}
			}

			if (UIHandler != null && UIHandler.DashGuideUI != null)
			{
				UIHandler.DashGuideUI.SetActive( !IsAttacking && !IsDashing && _dashCooldownTime <= 0);
			}

			if(UIHandler != null && UIHandler.DashCoolDownTextUI != null)
			{
				UIHandler.DashCoolDownTextUI.SetActive( _dashCooldownTime > 0 );
			}

			CharacterController.MaxStableMoveSpeed = moveSpeed;

			LookDirection = lookDirection;

			// Build the CharacterInputs struct
			characterInputs.MoveAxisForward = MoveInput.y;
			characterInputs.MoveAxisRight = MoveInput.x;
			characterInputs.CrouchDown = crouchInput;
			characterInputs.CrouchUp = !crouchInput;

			// Apply inputs to character
			CharacterController.SetInputs( ref characterInputs );
		}

		public void SetIsAttacking(bool isAttacking)
		{
			IsAttacking = isAttacking;
			if(IsAttacking == false)
			{
				CurrentAttackType = TypeOfAttack.None;
			}
		}
	}

	public enum TypeOfAttack
	{
		None,
		SwordSwing,
		AOE
	}
}