using Gameplay.Cameras.Mechanics.Data;
using Gameplay.Character;
using Gameplay.InputSystems;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics.Logic
{
	public class MoveCameraMechanic
		: BaseCameraMechanic
	{
		private readonly IInputService _inputService;
		private readonly RigCamera _rigCamera;
		private readonly ActorSelectorService _actorSelector;
		private readonly float _moveSpeed;
		private readonly float _maxActiveCharacterDistance;
		private readonly AnimationCurve _speedCurveFromDistance;
		private readonly float _idleCooldown;


		private float _idleTimer;
		private bool _isMoveToActiveCharacter = false;
		private Actor _activeCharacter;

		public MoveCameraMechanic(
			IInputService inputService,
			RigCamera rigCamera,
			ActorSelectorService actorSelector,
			float moveSpeed,
			float maxActiveCharacterDistance,
			AnimationCurve speedCurveFromDistance,
			float idleCooldown
		)
		{
			_inputService = inputService;
			_rigCamera = rigCamera;
			_actorSelector = actorSelector;
			_moveSpeed = moveSpeed;
			_maxActiveCharacterDistance = maxActiveCharacterDistance;
			_speedCurveFromDistance = speedCurveFromDistance;
			_idleCooldown = idleCooldown;
			_activeCharacter = _actorSelector.GetActiveCharacter();
		}

		protected override void DoEnable()
		{
			_actorSelector.ActorSelectedChangeEvent += OnChangeActor;
			_idleTimer = 0;
		}

		protected override void DoDisable()
		{
			_actorSelector.ActorSelectedChangeEvent -= OnChangeActor;
		}

		protected override void DoUpdate(float deltaTime)
		{
			if (_isMoveToActiveCharacter)
			{
				MoveToActiveCharacter(deltaTime);
			}

			var inputX = _inputService.MoveCameraDirection.x;
			var inputY = _inputService.MoveCameraDirection.y;

			_idleTimer += deltaTime;
			if (Mathf.Abs(inputX) > Mathf.Epsilon
				|| Mathf.Abs(inputY) > Mathf.Epsilon)
			{
				MoveByInput(deltaTime, inputY, inputX);
				_idleTimer = 0f;
				_isMoveToActiveCharacter = false;
			}

			if (_idleTimer > _idleCooldown)
			{
				_isMoveToActiveCharacter = true;
			}
		}

		private void MoveToActiveCharacter(float deltaTime)
		{
			_rigCamera.transform.position = Vector3.Lerp(
				_rigCamera.transform.position,
				_activeCharacter.transform.position,
				_moveSpeed * deltaTime
			);
		}

		private void MoveByInput(float deltaTime, float inputY, float inputX)
		{
			var forward = _rigCamera.CameraLink.transform.forward;
			var right = _rigCamera.CameraLink.transform.right;

			forward.y = 0f;
			right.y = 0f;

			forward.Normalize();
			right.Normalize();

			var moveDirection = forward * inputY + right * inputX;
			_rigCamera.transform.Translate(moveDirection * GetMoveSpeed() * deltaTime, Space.World);
		}

		private float GetMoveSpeed()
		{
			if (_activeCharacter == null)
				return _moveSpeed;
			var distance = Vector3.Distance(_rigCamera.transform.position, _activeCharacter.transform.position);
			return _moveSpeed * _speedCurveFromDistance.Evaluate(distance / _maxActiveCharacterDistance);
		}

		private void OnChangeActor(Actor actor)
		{
			_activeCharacter = actor;
			_isMoveToActiveCharacter = true;
		}
	}
}