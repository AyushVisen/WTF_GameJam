using UnityEngine;

namespace Ayush
{
	public class AlwaysCameraFace : MonoBehaviour, IUpdateCallback
	{
		[field: SerializeField]
		public bool TrackCameraEvryFrame { get; private set; }

		private UpdateCallbackService _updateCallbackService;
		private Camera _camera;

		private void Start()
		{
			_camera = Camera.main;
			FaceCamera();

			if (TrackCameraEvryFrame && GameManager.Instance.TryGetService( out _updateCallbackService ))
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

		private void FaceCamera()
		{
			transform.forward = -_camera.transform.forward;
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
			FaceCamera();
		}
	}
}