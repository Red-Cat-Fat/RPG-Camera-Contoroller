using System;
using Gameplay.Cameras.Mechanics.Logic;
using Gameplay.Character;
using Gameplay.InputSystems;
using SerializeReferenceEditor;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics.Data
{
	[Serializable]
	[SRName("Rotate")]
	public class RotateCameraMechanicData : ICameraMechanicData
	{
		[SerializeField] private Transform _slider;
		[SerializeField] private float _rotateSpeed = 5f;


		public ICameraMechanic MakeMechanic(
			IInputService inputService,
			RigCamera rigCamera,
			ActorSelectorService actorSelector
		)
			=> new RotateCameraMechanic(inputService, _slider, _rotateSpeed);
	}
}