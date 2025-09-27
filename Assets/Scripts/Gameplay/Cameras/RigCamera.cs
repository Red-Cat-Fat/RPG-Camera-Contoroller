using System;
using Extensions;
using Gameplay.Cameras.Mechanics;
using Gameplay.Character;
using Gameplay.InputSystems;
using Infrastructure;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Cameras
{
	public class RigCamera : UpdatableMonoConstruct
	{
		[SerializeField] private Camera _camera;

		[SerializeField]
		private BaseCameraMechanic[] _cameraMechanics = Array.Empty<BaseCameraMechanic>();

		public Camera CameraLink => _camera;

		public void Construct(IInputService inputService, ActorSelectorService actorSelectorService)
		{
			for (var i = 0; i < _cameraMechanics.Length; i++)
				_cameraMechanics[i].Construct(inputService, actorSelectorService);

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

#if UNITY_EDITOR
		public Transform EditorTargetCameraPosition;
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(transform.position, 0.3f);

			if (EditorTargetCameraPosition == null)
				return;

			Gizmos.color = Color.gray;
			Gizmos.DrawCube(EditorTargetCameraPosition.position, Vector3.one * 0.25f);
			Handles.Label(EditorTargetCameraPosition.position + Vector3.up * 0.25f, "TargetCameraPosition");
		}
#endif
	}
}