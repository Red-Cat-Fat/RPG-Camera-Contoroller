using Gameplay.Cameras.Mechanics.Data;
using Gameplay.InputSystems;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics.Logic
{
	public class ZoomCameraMechanic
		: BaseCameraMechanic
	{
		private readonly IInputService _inputService;
		private readonly RigCamera _rigCamera;
		private readonly Transform _slider;
		private readonly float _zoomSpeed;
		private readonly float _minDistance;
		private readonly float _maxDistance;

		private float _currentDistance = 10f;

		public ZoomCameraMechanic(
			IInputService inputService,
			RigCamera rigCamera,
			Transform slider,
			float zoomSpeed,
			float minDistance,
			float maxDistance
		)
		{
			_inputService = inputService;
			_rigCamera = rigCamera;
			_slider = slider;
			_zoomSpeed = zoomSpeed;
			_minDistance = minDistance;
			_maxDistance = maxDistance;
		}

		protected override void DoEnable()
		{
			_currentDistance = Vector3.Distance(
				_rigCamera.CameraLink.transform.position,
				_rigCamera.transform.position
			);
		}

		protected override void DoUpdate(float deltaTime)
		{
			var zoomInput = _inputService.ZoomValue;
			if (zoomInput == 0)
				return;

			_currentDistance = Mathf.Clamp(
				_currentDistance - zoomInput * _zoomSpeed,
				_minDistance,
				_maxDistance
			);

			_slider.localPosition =
				Vector3.Lerp(
					_slider.localPosition,
					new Vector3(
						_slider.localPosition.x,
						_slider.localPosition.y,
						_currentDistance
					),
					deltaTime
				);
		}
	}
}