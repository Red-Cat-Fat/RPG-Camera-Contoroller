using System;
using Extensions;
using Gameplay.Cameras.Mechanics;
using Gameplay.Character;
using Gameplay.InputSystems;
using Infrastructure;
using UnityEngine;

namespace Gameplay.Cameras
{
	public class RigCamera : UpdatableMonoConstruct
	{
		[SerializeField] private Camera _camera;
		[SerializeField] 
		private BaseCameraMechanic[] _cameraMechanicsData = Array.Empty<BaseCameraMechanic>();

		public Camera CameraLink => _camera;

		public void Construct(IInputService inputService, ActorSelectorService actorSelectorService)
		{
			for (var i = 0; i < _cameraMechanicsData.Length; i++)
				_cameraMechanicsData[i].Construct(inputService, actorSelectorService);

			FinishedInitialization();
		}

		protected override void DoInitialized()
		{
			_cameraMechanicsData.Enable();
		}

		protected override void DoDeinitialized()
		{
			_cameraMechanicsData.Disable();
		}

		protected override void DoUpdate(float deltaTime)
		{
			_cameraMechanicsData.Update(deltaTime);
		}
	}
}