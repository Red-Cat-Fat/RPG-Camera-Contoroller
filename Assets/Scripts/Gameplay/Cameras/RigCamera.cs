using System;
using Extensions;
using Gameplay.Cameras.Mechanics;
using Gameplay.Cameras.Mechanics.Data;
using Gameplay.InputSystems;
using Infrastructure;
using SerializeReferenceEditor;
using UnityEngine;

namespace Gameplay.Cameras
{
	public class RigCamera : UpdatableMonoConstruct
	{
		[SerializeField] private Camera _camera;
		[SerializeReference] [SR]
		private ICameraMechanicData[] _cameraMechanicsData = Array.Empty<ICameraMechanicData>();
		
		private ICameraMechanic[] _cameraMechanics = Array.Empty<ICameraMechanic>();

		public Camera CameraLink => _camera;

		public void Construct(IInputService inputService)
		{
			_cameraMechanics = new ICameraMechanic[_cameraMechanicsData.Length];
			for (var i = 0; i < _cameraMechanicsData.Length; i++)
				_cameraMechanics[i] = _cameraMechanicsData[i].MakeMechanic(inputService, this);

			FinishedInitialization();
		}

		protected override void DoInitialized()
		{
			_cameraMechanics.Enable();
		}

		protected override void DoDeinitialized()
		{
			_cameraMechanics.Disable();
		}

		protected override void DoUpdate(float deltaTime)
		{
			_cameraMechanics.Update(deltaTime);
		}
	}
}