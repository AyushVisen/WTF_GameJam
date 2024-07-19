using Ayush;
using WTF_GameJam.Health;
using System.Collections.Generic;
using UnityEngine;

namespace WTF_GameJam.Damage
{
	public class DamageVolume : MonoBehaviour, IUpdateCallback
	{
		[field: SerializeField, Range( 0f, 100f )]
		public float Damage { get; private set; }

		[field: SerializeField, Range( 0f, 600f )]
		public float DamageInterval { get; private set; }

		private float _timeSinceLastDamage;
		[SerializeField]
		private List<HealthBehavior> _healthBehaviorObjects = new();
		private UpdateCallbackService _updateCallbackService;

		public bool ShouldUpdate => _healthBehaviorObjects.Count > 0;

		private void Start()
		{
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

		private void OnTriggerEnter( Collider other )
		{
			var healthBehavior = other.GetComponentInChildren<HealthBehavior>();
			if (healthBehavior != null && !_healthBehaviorObjects.Contains( healthBehavior ))
			{
				_healthBehaviorObjects.Add( healthBehavior );
			}
		}

		private void OnTriggerExit( Collider other )
		{
			var healthBehavior = other.GetComponentInChildren<HealthBehavior>();
			if (healthBehavior != null && _healthBehaviorObjects.Contains( healthBehavior ))
			{
				_healthBehaviorObjects.Remove( healthBehavior );
			}
		}

		public void UpdateCallback()
		{
			foreach (var healthBehavior in _healthBehaviorObjects)
			{
				if (Time.time - _timeSinceLastDamage >= DamageInterval)
				{
					healthBehavior.ModifyHealth( -Damage );
					_timeSinceLastDamage = Time.time;
				}
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
	}
}