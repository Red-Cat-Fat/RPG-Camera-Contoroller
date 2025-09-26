using Gameplay.InputSystems;
using Infrastructure;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Character
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class CharacterMovement : MonoConstruct
	{
		[SerializeField]
		private NavMeshAgent _agent;
		private IInputService _inputService;

		public void Construct(IInputService inputService)
		{
			_inputService = inputService;
			FinishedInitialization();
		}

		private void OnMovePositionGet(Vector3 moveToPosition)
		{
			_agent.SetDestination(moveToPosition);
		}

		protected override void DoInitialized()
		{
			_inputService.MoveCharacterEvent += OnMovePositionGet;
		}

		protected override void DoDeinitialized()
		{
			_inputService.MoveCharacterEvent -= OnMovePositionGet;
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_agent == null)
				_agent = GetComponent<NavMeshAgent>();
		}
#endif
	}
}