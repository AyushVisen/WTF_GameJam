using Ayush.Singleton;
using WTF_GameJam;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ayush
{
	public class GameManager : Singleton<GameManager>
	{
		private Dictionary<Type, IGlobalService> GlobalServices { get; set; } = new();
		private Dictionary<Type, ILocalService> LocalServices { get; set; } = new();

		private void Start()
		{
			StartCoroutine( WaitForAllGlobalServices() );
		}

		IEnumerator WaitForAllGlobalServices()
		{
			yield return new WaitForSeconds( 1f );

			foreach (var (serviceType, service) in GlobalServices)
			{
				yield return new WaitUntil( () => service.IsReady );
			}

			SceneManager.LoadScene( Constants.PrototypeLevel );
		}

		public bool RegisterService<T>( T service, bool replaceExisting = false ) where T : class, IService
		{
			var serviceType = service.GetType();
			switch (service)
			{
				case IGlobalService globalService when replaceExisting:
					if (GlobalServices.TryGetValue( serviceType, out var existingGlobalService ))
					{
						existingGlobalService.Dispose();
					}
					GlobalServices[serviceType] = globalService;
					return true;

				case IGlobalService globalService:
					return GlobalServices.TryAdd( serviceType, globalService );

				case ILocalService localService when replaceExisting:
					if (LocalServices.TryGetValue( serviceType, out var existingLocalService ))
					{
						existingLocalService.Dispose();
					}

					LocalServices[serviceType] = localService;
					return true;

				case ILocalService localService:
					return LocalServices.TryAdd( serviceType, localService );

				default:
					Debug.LogError( "Invalid service, It has to be inherited from ILocalService or IGlobalService" );
					return false;
			}
		}

		public bool TryGetService<T>( out T service, bool isRequired = false ) where T : class, IService, new()
		{
			var typeOfService = typeof( T );

			if (typeof( IGlobalService ).IsAssignableFrom( typeOfService ))
			{
				if (GlobalServices.TryGetValue( typeOfService, out var globalService ))
				{
					service = globalService as T;
					return true;
				}
			}
			else if (typeof( ILocalService ).IsAssignableFrom( typeOfService ))
			{
				if (LocalServices.TryGetValue( typeOfService, out var localService ))
				{
					service = localService as T;
					return true;
				}
			}

			if (isRequired)
			{
				if (typeof( Component ).IsAssignableFrom( typeOfService ))
				{
					service = new GameObject( nameof( service ) ).AddComponent( typeOfService ) as T;
				}
				else
				{
					service = new T();
				}

				return RegisterService( service );
			}

			service = null;
			return false;
		}

		public bool TryRemoveService<T>( T service ) where T : class, IService
		{
			var isRemoved = service switch
			{
				IGlobalService => GlobalServices.Remove( service.GetType() ),
				ILocalService => LocalServices.Remove( service.GetType() ),
				_ => false
			};

			if (isRemoved)
			{
				service.Dispose();
			}

			return isRemoved;
		}
	}

	#region Interfaces

	public interface IService
	{
		bool IsReady => false;
		void Dispose();
	}

	public interface IGlobalService : IService { }

	public interface ILocalService : IService { }
	#endregion
}