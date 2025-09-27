using Gameplay.Character;
using Gameplay.InputSystems;
using UnityEditor;
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

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (_rotateTransform == null)
				return;

			Gizmos.color = Color.yellow * 0.7f;
			var start = _rotateTransform.position - _rotateTransform.rotation * _rotateTransform.up * 5;
			var end = _rotateTransform.position + _rotateTransform.rotation * _rotateTransform.up * 5;
			Gizmos.DrawLine(start, end);
			Handles.Label(end, "Rotation Axis");
		}
#endif
	}
}