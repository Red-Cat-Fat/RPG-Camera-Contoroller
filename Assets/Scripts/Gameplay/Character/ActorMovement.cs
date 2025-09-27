using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Character
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class ActorMovement : MonoBehaviour
	{
		[SerializeField] private NavMeshAgent _agent;

		public Vector3 MoveDirection => _agent.desiredVelocity;

		public void MoveTo(Vector3 moveToPosition)
		{
			_agent.SetDestination(moveToPosition);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_agent == null)
				_agent = GetComponent<NavMeshAgent>();
			
			if (_agent == null)
				Debug.LogErrorFormat("No NavMeshAgent found on {0}", gameObject.name);
		}
#endif
	}
}