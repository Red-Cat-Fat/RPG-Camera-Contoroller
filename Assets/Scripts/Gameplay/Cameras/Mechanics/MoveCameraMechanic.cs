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

		[Tooltip("Дополнительный множитель при движении К персонажу")] [SerializeField]
		private float _dotAdditionalSpeed = 3f;

		[Tooltip("Максимальное расстояние удаления от активного персонажа")] [SerializeField]
		private float _maxActiveCharacterDistance = 9f;

		[Tooltip("Дистанция, с которой камера начинает замедляться")]
		[SerializeField]
		private float _startStartSpeedLowerDistance = 5f;

		[Tooltip("Скорость замедления движения камеры по мере отдаления камеры свыше _startMoveSpeedLowerDistance")]
		[SerializeField]
		private AnimationCurve _speedLowesByDistance;

		[Tooltip("Влияние множителя dot в зависимости от расстояния до персонажа (чем дальше, тем больше влияние)")]
		[SerializeField]
		private AnimationCurve _dotEffectByDistance;

		[Tooltip("Влияние скорости движения по направлению к персонажу (dot из [-1:1] интерполируется на [0:1])")]
		[SerializeField]
		private AnimationCurve _speedMultiplyByDot;

		[Tooltip("Скорость, ниже которой камера останавливается")] [SerializeField]
		private float _stopSpeedTrashHold = 0.1f;

		[Tooltip("Время бездействия игрока при которой камера начинает следовать за активным персонажем")]
		[SerializeField]
		private float _idleCooldown = 5f;

		private IInputService _inputService;
		private ActorSelectorService _actorSelector;

		private Actor _activeCharacter;
		private float _idleTimer;
		private bool _isMoveToActiveCharacterInProgress;
		private bool _lockInputUntilKeyUp;

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
			var distanceToCharacter = _activeCharacter.transform.position - _rigCamera.transform.position;
			var distance = distanceToCharacter.magnitude;

			if (_isMoveToActiveCharacterInProgress)
			{
				MoveToActiveCharacter(deltaTime);
			}
			else
			{
				if (IsNeedMoveToActiveCharacter(distance))
				{
					StartFollowActiveCharacter();
					return;
				}
			}

			var inputX = _inputService.MoveCameraDirection.x;
			var inputY = _inputService.MoveCameraDirection.y;

			_idleTimer += deltaTime;

			if (!IsHasInput(inputX, inputY))
			{
				_lockInputUntilKeyUp = false;
				return;
			}

			if (_lockInputUntilKeyUp)
				return;

			MoveByInput(
				deltaTime,
				inputY,
				inputX,
				distance,
				distanceToCharacter
			);
		}

		private bool IsNeedMoveToActiveCharacter(float distance)
			=> distance > _maxActiveCharacterDistance
				&& _activeCharacter != null
				&& _activeCharacter.IsMove
				|| _idleTimer > _idleCooldown
				|| _inputService.LookAtCharacter;

		private void MoveToActiveCharacter(float deltaTime)
		{
			_rigCamera.transform.position = Vector3.Lerp(
				_rigCamera.transform.position,
				_activeCharacter.transform.position,
				_moveSpeed * deltaTime
			);
		}

		private void MoveByInput(
			float deltaTime,
			float inputY,
			float inputX,
			float sqrDistance,
			Vector3 distanceToCharacter
		)
		{
			var forward = _rigCamera.CameraLink.transform.forward;
			var right = _rigCamera.CameraLink.transform.right;

			forward.y = 0f;
			right.y = 0f;

			forward.Normalize();
			right.Normalize();

			var moveDirection = (forward * inputY + right * inputX).normalized;
			_rigCamera.transform.Translate(
				moveDirection * GetMoveSpeed(moveDirection, sqrDistance, distanceToCharacter) * deltaTime,
				Space.World
			);

			_idleTimer = 0f;
			_isMoveToActiveCharacterInProgress = false;
		}

		private static bool IsHasInput(float inputX, float inputY)
		{
			return Mathf.Abs(inputX) > Mathf.Epsilon
					|| Mathf.Abs(inputY) > Mathf.Epsilon;
		}

		private void StartFollowActiveCharacter()
		{
			_isMoveToActiveCharacterInProgress = true;
			_lockInputUntilKeyUp = true;
		}

		private float GetMoveSpeed(Vector3 moveDirection, float distance, Vector3 distanceToCharacter)
		{
			if (_activeCharacter == null
				|| distance < _startStartSpeedLowerDistance)
				return _moveSpeed;

			var outDistance = distance - _startStartSpeedLowerDistance;
			var fullDistance = _maxActiveCharacterDistance - _startStartSpeedLowerDistance;
			
			var distancePercent = outDistance / fullDistance;
			var distanceMultiplier = _speedLowesByDistance.Evaluate(distancePercent);

			var dot = Vector3.Dot(moveDirection.normalized, distanceToCharacter.normalized);
			var dotPoint = (dot + 1) / 2;
			var dotMultiplier = _speedMultiplyByDot.Evaluate(dotPoint);

			var dotEffect = _dotEffectByDistance.Evaluate(distancePercent);
			var resultSpeed = _moveSpeed * (dotEffect * dotMultiplier * _dotAdditionalSpeed + distanceMultiplier);
			return resultSpeed > _stopSpeedTrashHold
				? resultSpeed
				: 0f;
		}

		private void OnChangeActor(Actor actor)
		{
			_activeCharacter = actor;
			_isMoveToActiveCharacterInProgress = true;
		}
	}
}