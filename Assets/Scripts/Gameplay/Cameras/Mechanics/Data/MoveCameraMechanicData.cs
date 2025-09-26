using System;
using Gameplay.Cameras.Mechanics.Logic;
using Gameplay.Character;
using Gameplay.InputSystems;
using SerializeReferenceEditor;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics.Data
{
	[Serializable]
	[SRName("Move")]
	public class MoveCameraMechanicData : ICameraMechanicData
	{
		[SerializeField] private float _moveSpeed;
		[SerializeField] private float _idleTimer = 2f;
		[SerializeField] private float _maxActiveCharacterDistance= 5f;
		[SerializeField] private AnimationCurve _speedCurveFromDistance;

		public ICameraMechanic MakeMechanic(
			IInputService inputService,
			RigCamera rigCamera,
			ActorSelectorService actorSelector
		)
		{
			return new MoveCameraMechanic(
				inputService,
				rigCamera,
				actorSelector,
				_moveSpeed,
				_maxActiveCharacterDistance,
				_speedCurveFromDistance,
				_idleTimer
			);
		}
	}
}