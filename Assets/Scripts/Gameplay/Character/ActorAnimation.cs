using UnityEngine;

namespace Gameplay.Character
{
	public class ActorAnimation : MonoBehaviour
	{
		private static readonly int IsMove = Animator.StringToHash("IsMove");
		[SerializeField] private ActorMovement _movement;
		[SerializeField] private Animator _animator;

		private void Update()
		{
			_animator.SetBool(IsMove, _movement.MoveDirection.sqrMagnitude > 0);
		}
	}
}