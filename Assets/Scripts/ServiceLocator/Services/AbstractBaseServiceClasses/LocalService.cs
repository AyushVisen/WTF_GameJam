using System;
using UnityEngine;

namespace Ayush
{
	public abstract class LocalService : MonoBehaviour, ILocalService
	{
		[SerializeField]
		private bool _replaceExistingService;
		protected GameManager GameManager { get; private set; }
		public bool IsReady { get; protected set; }

		//This is done to avoid race condition in registering service, if Service's Awake get executed after this, then it will throw exception
		[Obsolete( "Override OnInit and use that instead" )]
		protected void Awake() { }

		protected void Start()
		{
			GameManager = GameManager.Instance;

			if (GameManager == null)
			{
				return;
			}

			OnInit( GameManager.RegisterService( this, _replaceExistingService ) );
		}
		protected void OnDestroy()
		{
			if (GameManager != null)
			{
				OnDisposed( GameManager.TryRemoveService( this ) );
			}
		}

		protected virtual void OnInit( bool serviceAdded )
		{
		}

		protected virtual void OnDisposed( bool serviceRemoved )
		{
		}

		#region Implementation of IService

		public void Dispose()
		{
			Destroy( gameObject );
		}

		#endregion
	}
}
