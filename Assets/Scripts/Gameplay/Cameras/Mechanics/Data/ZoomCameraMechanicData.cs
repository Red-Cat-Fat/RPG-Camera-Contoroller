using System;
using Gameplay.Cameras.Mechanics.Logic;
using Gameplay.Cameras.Mechanics.Rails;
using Gameplay.InputSystems;
using SerializeReferenceEditor;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics.Data
{
	[Serializable]
	[SRName("Zoom")]
	public class ZoomCameraMechanicData : ICameraMechanicData
	{
		[SerializeField] private CatmullRail _rail;
		[SerializeField] private Transform _zoomPoint;
		[SerializeField] private float _zoomSpeed = 5f;
		[SerializeField] private float _zoomSmoothness = 5f;

		public ICameraMechanic MakeMechanic(IInputService inputService, RigCamera rigCamera)
			=> new ZoomCameraMechanic(
				inputService,
				_zoomPoint,
				_rail,
				_zoomSpeed,
				_zoomSmoothness
			);
	}
}