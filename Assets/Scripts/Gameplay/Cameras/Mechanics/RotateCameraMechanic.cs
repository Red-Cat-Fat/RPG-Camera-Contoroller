using Gameplay.Character;
using Gameplay.InputSystems;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics
{
	public class RotateCameraMechanic
		: BaseCameraMechanic
	{
		[SerializeField] private Transform _rotateTransform;
		[SerializeField] private float _rotationSpeed = 5f;
		private float _currentRotationAngle;
		private IInputService _inputService;

		public override void Construct(IInputService inputService, ActorSelectorService actorSelectorService)
		{
			_inputService = inputService;
		}

		protected override void DoEnable() => _currentRotationAngle = 0;


		protected override void DoUpdate(float deltaTime)
		{
			var oldRotationAngle = _currentRotationAngle;
			_currentRotationAngle += _inputService.RotateValue * _rotationSpeed;
			var resultRotationAngle = Mathf.Lerp(oldRotationAngle, _currentRotationAngle, deltaTime);
			_rotateTransform.localRotation = Quaternion.Euler(0, resultRotationAngle, 0);
		}
	}
}