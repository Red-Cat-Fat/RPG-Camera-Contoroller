using System;
using Gameplay.Cameras.Mechanics.Logic;
using Gameplay.InputSystems;
using SerializeReferenceEditor;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics.Data
{
	[Serializable]
	[SRName("Zoom")]
	public class ZoomCameraMechanicData : ICameraMechanicData
	{
		[SerializeField] private Transform _slider;
		[SerializeField] private float _zoomSpeed = 5f;
		[SerializeField] private float _minDistance = 5f;
		[SerializeField] private float _maxDistance = 20f;
		[SerializeField] private float _zoomSmoothness = 5f;

		public ICameraMechanic MakeMechanic(IInputService inputService, RigCamera rigCamera)
			=> new ZoomCameraMechanic(
				inputService,
				rigCamera,
				_slider,
				_zoomSpeed,
				_minDistance,
				_maxDistance
			);
	}
}