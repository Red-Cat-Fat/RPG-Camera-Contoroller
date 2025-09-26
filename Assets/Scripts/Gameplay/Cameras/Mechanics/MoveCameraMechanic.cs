using Gameplay.Character;
using Gameplay.InputSystems;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics
{
	public class MoveCameraMechanic
		: BaseCameraMechanic
	{
		[SerializeField] private RigCamera _rigCamera;
		[SerializeField] private float _moveSpeed = 3f;
		[SerializeField] private float _maxActiveCharacterDistance = 9f;
		[SerializeField] private AnimationCurve _speedCurveFromDistance;
		[SerializeField] private float _idleCooldown = 5f;

		private IInputService _inputService;
		private ActorSelectorService _actorSelector;
		private float _idleTimer;
		private bool _isMoveToActiveCharacter;
		private Actor _activeCharacter;


		public override void Construct(IInputService inputService, ActorSelectorService actorSelectorService)
		{
			_inputService = inputService;
			_actorSelector = actorSelectorService;
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