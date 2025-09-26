using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Character
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class CharacterMovement : MonoBehaviour
	{
		[SerializeField] private Camera _mainCamera;

		private NavMeshAgent _agent;

		private void Update()
		{
			if (!Input.GetMouseButtonDown(0))
				return;

			var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out var hit))
			{
				_agent.SetDestination(hit.point);
			}
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