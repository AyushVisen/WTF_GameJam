using Ayush;
using DG.Tweening;
using UnityEngine;
using WTF_GameJam.AI;
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

		[field: SerializeField]
		public AudioClip DashSFX { get; private set; }

		[field: SerializeField]
		public GameObject NavigationArrow { get; private set; }

		[field: SerializeField]
		public int BotDeathCountObjective { get; private set; }


		public GameObject BotDeathObjectiveGate { get; private set; }
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
		private AudioService _audioService;
		private int _botDeathCount;
		private Tween _objectiveCounterTween;

		private void Awake()
		{
			_playerInputSystem = new PlayerInputSystem();
			_playerInputSystem.Player.Enable();
		}

		private void Start()
		{
			HealthBehavior = GetComponent<HealthBehavior>();
			UIHandler = FindFirstObjectByType<UIHandler>();
			if (UIHandler != null)
			{
				UIHandler.PlayerHealthBehaviour = HealthBehavior;
				HealthBehavior.HealthFillImage = UIHandler.HealthFillImage;

				if (UIHandler.ObjectiveCountText != null)
				{
					UIHandler.ObjectiveCountText.SetText( "{0}/{1}", _botDeathCount, BotDeathCountObjective );
				}
				if (UIHandler.ObjectiveGuideText != null)
				{
					UIHandler.ObjectiveGuideText.gameObject.SetActive( true );
					UIHandler.ObjectiveGuideText.SetText( "Kill {0} enemies to unlock Reactor's entrance", BotDeathCountObjective );
				}
			}
			CurrentAttackType = TypeOfAttack.None;
			NavigationArrow.SetActive( false );
			GameManager.Instance.TryGetService( out _audioService );
			ExtendedBehaviorTreeProcessor.BotDeath += OnBotDeath;

			BotDeathObjectiveGate = GameObject.FindGameObjectWithTag( "BotDeathObjectiveGate" );
			if (BotDeathObjectiveGate != null)
			{
				BotDeathObjectiveGate.SetActive( true );
			}
		}

		private void OnDestroy()
		{
			_playerInputSystem.Player.Disable();
			ExtendedBehaviorTreeProcessor.BotDeath -= OnBotDeath;
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

			if (_aoeCoolDownTimeRemaining > 0f)
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
				if (_aoeCoolDownTimeRemaining <= 0f)
				{
					_aoeCoolDownTimeRemaining = AoeCoolDownTime;
				}
			}

			if (CurrentAttackType == TypeOfAttack.None)
			{
				if (SwingAttackInput)
				{
					CurrentAttackType = TypeOfAttack.SwordSwing;
				}
				if (AOEAttackInput)
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

			if (IsAttacking == false)
			{
				if (dashInput)
				{
					if (_dashTimeRemaining <= 0f && _dashCooldownTime <= 0f)
					{
						_dashTimeRemaining = DashTime;
						if (_audioService != null)
						{
							_audioService.PlaySfx( DashSFX );
						}
					}
				}
				else
				{
					moveSpeed = NormalSpeed;
				}

				if (_dashTimeRemaining > 0f)
				{
					_dashTimeRemaining -= Time.deltaTime;

					if (_dashTimeRemaining <= 0f)
					{
						_dashCooldownTime = DashCooldownTime;
					}

					MoveInput = new Vector2( PlayerAvatarRoot.transform.forward.x, PlayerAvatarRoot.transform.forward.z );
					moveSpeed = DashSpeed;
				}
			}

			if (_dashCooldownTime > 0)
			{
				_dashCooldownTime -= Time.deltaTime;

				if (UIHandler != null && UIHandler.DashCoolDownTimerUI != null)
				{
					UIHandler.DashCoolDownTimerUI.fillAmount = (1 - _dashCooldownTime / DashCooldownTime);
				}
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

		public void SetIsAttacking( bool isAttacking )
		{
			IsAttacking = isAttacking;
			if (IsAttacking == false)
			{
				CurrentAttackType = TypeOfAttack.None;
			}
		}

		private void OnBotDeath()
		{
			if (_botDeathCount >= BotDeathCountObjective)
			{
				ExtendedBehaviorTreeProcessor.BotDeath -= OnBotDeath;
				return;
			}

			_botDeathCount++;
			if (UIHandler != null && UIHandler.ObjectiveCountText != null)
			{
				if (_objectiveCounterTween != null)
				{
					_objectiveCounterTween.Kill();
					UIHandler.ObjectiveCountText.rectTransform.localScale = Vector3.one;
				}
				UIHandler.ObjectiveCountText.SetText( "{0}/{1}", _botDeathCount, BotDeathCountObjective );
				_objectiveCounterTween = UIHandler.ObjectiveCountText.rectTransform.DOPunchScale( Vector3.one * 0.5f, 0.5f );
			}

			if (_botDeathCount == BotDeathCountObjective)
			{
				NavigationArrow.SetActive( true );
				if (BotDeathObjectiveGate != null)
				{
					BotDeathObjectiveGate.SetActive( false );
				}

				if (UIHandler != null && UIHandler.ObjectiveGuideText != null)
				{
					UIHandler.ObjectiveGuideText.gameObject.SetActive( false );
				}
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