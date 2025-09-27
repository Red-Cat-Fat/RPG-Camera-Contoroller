using Gameplay.InputSystems;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Character
{
	public class Actor : MonoConstruct
	{
		[SerializeField] private ActorMovement _movement;
		
		private IInputService _inputService;
		public bool IsMove => _movement != null && _movement.MoveDirection.sqrMagnitude > 0;

		public void Construct(IInputService inputService)
		{
			_inputService = inputService;
			FinishedInitialization();
		}

		private void OnMovePositionGet(Vector3 moveToPosition)
		{
			_movement.MoveTo(moveToPosition);
		}

		protected override void DoInitialized()
		{
			_inputService.MoveCharacterEvent += OnMovePositionGet;
		}

		protected override void DoDeinitialized()
		{
			_inputService.MoveCharacterEvent -= OnMovePositionGet;
		}
	}
}