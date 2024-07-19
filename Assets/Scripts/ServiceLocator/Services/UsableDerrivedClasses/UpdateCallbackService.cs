using System.Collections.Generic;

namespace Ayush
{
	public class UpdateCallbackService : GlobalService
	{
		public SortedList<int, List<IUpdateCallback>> UpdateCallbacksSortedList { get; private set; }

		protected override void OnInit( bool serviceAdded )
		{
			base.OnInit( serviceAdded );

			if (serviceAdded)
			{
				UpdateCallbacksSortedList = new();
				IsReady = true;
			}
		}

		public void RegisterUpdateCallback( IUpdateCallback updateCallback )
		{
			if (UpdateCallbacksSortedList.ContainsKey( updateCallback.Priority ))
			{
				UpdateCallbacksSortedList[updateCallback.Priority].Add( updateCallback );
			}
			else
			{
				UpdateCallbacksSortedList.Add( updateCallback.Priority, new List<IUpdateCallback> { updateCallback } );
			}
		}

		public void UnregisterUpdateCallback( IUpdateCallback updateCallback )
		{
			if (UpdateCallbacksSortedList.ContainsKey( updateCallback.Priority ))
			{
				UpdateCallbacksSortedList[updateCallback.Priority].Remove( updateCallback );
			}
		}

		private void Update()
		{
			foreach (var (priority, callbacksList) in UpdateCallbacksSortedList)
			{
				foreach (var callback in callbacksList)
				{
					if (callback == null || !callback.ShouldUpdate)
					{
						continue;
					}

					callback.UpdateCallback();
				}
			}
		}
	}
}