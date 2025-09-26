using Gameplay.Cameras.Mechanics.Data;
using Gameplay.InputSystems;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics.Logic
{
	public class RotateCameraMechanic
		: BaseCameraMechanic
	{
		private readonly IInputService _inputService;
		private readonly Transform _slider;
		private readonly float _rotationSpeed;
		private float _currentRotationAngle;

		public RotateCameraMechanic(
			IInputService inputService,
			Transform slider,
			float rotationSpeed
		)
		{
			_inputService = inputService;
			_slider = slider;
			_rotationSpeed = rotationSpeed;
		}

		protected override void DoEnable() => _currentRotationAngle = 0;

		protected override void DoUpdate(float deltaTime)
		{
			var oldRotationAngle = _currentRotationAngle;
			_currentRotationAngle += _inputService.RotateValue * _rotationSpeed;
			var resultRotationAngle = Mathf.Lerp(oldRotationAngle, _currentRotationAngle, deltaTime);
			_slider.localRotation = Quaternion.Euler(0, resultRotationAngle, 0);
		}
	}
}