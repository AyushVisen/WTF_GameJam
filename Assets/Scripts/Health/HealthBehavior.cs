using Ayush;
using UnityEngine;
using UnityEngine.UI;

namespace WTF_GameJam.Health
{
	public class HealthBehavior : MonoBehaviour, IUpdateCallback
	{
		[field: SerializeField, Range( 0f, 100f )]
		public float MaxHealth { get; private set; }

		[field: SerializeField]
		public bool IsInvincible { get; set; }

		[field: SerializeField]
		public bool CanRegenerate { get; set; }

		[field: SerializeField]
		public float RegenerationRate { get; private set; }

		[field: SerializeField]
		public Image HealthFillImage { get; private set; }

		[field: SerializeField]
		public float CurrentHealth { get; private set; }

		public bool IsDead => CurrentHealth <= 0;

		private UpdateCallbackService _updateCallbackService;

		private void Start()
		{
			CurrentHealth = MaxHealth;
			if (GameManager.Instance.TryGetService( out _updateCallbackService ))
			{
				RegisterUpdateCallback();
			}
		}

		private void OnDestroy()
		{
			if (_updateCallbackService != null)
			{
				UnregisterUpdateCallback();
			}
		}

		public void ModifyHealth( float amount, bool forceModify = false )
		{
			if (IsInvincible && !forceModify)
			{
				return;
			}

			CurrentHealth = Mathf.Clamp( CurrentHealth + amount, 0, MaxHealth );
			if (HealthFillImage != null)
			{
				HealthFillImage.fillAmount = CurrentHealth / MaxHealth;
			}
		}

		public void RegisterUpdateCallback()
		{
			_updateCallbackService.RegisterUpdateCallback( this );
		}

		public void UnregisterUpdateCallback()
		{
			_updateCallbackService.UnregisterUpdateCallback( this );
		}

		public void UpdateCallback()
		{
			if (CanRegenerate && !IsDead && CurrentHealth < MaxHealth)
			{
				ModifyHealth( RegenerationRate * Time.deltaTime );
			}
		}
	}
}