using Gameplay.Character;
using Gameplay.InputSystems;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics
{
	public class ZoomCameraMechanic
		: BaseCameraMechanic
	{
		private IInputService _inputService;
		[SerializeField] private Transform _zoomPivot;
		[SerializeField] private CatmullRail _rail;
		[SerializeField] private float _zoomSpeed = 5f;
		[SerializeField] private float _zoomSmoothness = 5f;

		private float _currentProgress = 10f;

		public override void Construct(IInputService inputService, ActorSelectorService actorSelectorService)
		{
			_inputService = inputService;
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

			_zoomPivot.position = Vector3.Lerp(
				_zoomPivot.position,
				newCameraPosition,
				deltaTime * _zoomSmoothness
			);

			_zoomPivot.rotation
				= Quaternion.Lerp(
					_zoomPivot.rotation,
					cameraRotation,
					deltaTime * _zoomSmoothness
				);
		}
	}
}