using Gameplay.Cameras.Mechanics.Data;
using Gameplay.Cameras.Mechanics.Rails;
using Gameplay.InputSystems;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics.Logic
{
	public class ZoomCameraMechanic
		: BaseCameraMechanic
	{
		private readonly IInputService _inputService;
		private readonly Transform _zoomPoint;
		private readonly CatmullRail _rail;
		private readonly float _zoomSpeed;
		private readonly float _zoomSmoothness;

		private float _currentProgress = 10f;

		public ZoomCameraMechanic(
			IInputService inputService,
			Transform zoomPoint,
			CatmullRail rail,
			float zoomSpeed,
			float zoomSmoothness
		)
		{
			_inputService = inputService;
			_zoomPoint = zoomPoint;
			_rail = rail;
			_zoomSpeed = zoomSpeed;
			_zoomSmoothness = zoomSmoothness;
		}

		protected override void DoEnable()
		{
			_currentProgress = 0;
		}

		protected override void DoUpdate(float deltaTime)
		{
			var zoomInput = _inputService.ZoomValue;

			_currentProgress = Mathf.Clamp01(
				_currentProgress + zoomInput * _zoomSpeed
			);

			var newCameraPosition
				= _rail.GetPathPoint(_currentProgress, out var cameraRotation);

			_zoomPoint.position = Vector3.Lerp(
				_zoomPoint.position,
				newCameraPosition,
				deltaTime * _zoomSmoothness
			);

			_zoomPoint.rotation
				= Quaternion.Lerp(
					_zoomPoint.rotation,
					cameraRotation,
					deltaTime * _zoomSmoothness
				);
		}
	}
}