using System;
using Infrastructure;
using UnityEngine;

namespace Gameplay.InputSystems
{
	public class UnityEditorInputService : IInputService, IDisposable
	{
		private readonly FrameUpdater _frameUpdater;
		private readonly Camera _renderCamera;

		public event Action<Vector3> MoveCharacterEvent;
		public Vector3 MoveCameraDirection { get; private set; }
		public float ZoomValue { get; private set; }
		public int RotateValue { get; private set; }
		public bool LookAtCharacter { get; private set; }

		public UnityEditorInputService(FrameUpdater frameUpdater, Camera renderCamera)
		{
			_renderCamera = renderCamera;
			_frameUpdater = frameUpdater;
			_frameUpdater.UpdateEvent += OnUpdate;
		}

		public void Dispose()
		{
			_frameUpdater.UpdateEvent -= OnUpdate;
		}

		private void OnUpdate(float deltaTime)
		{
			CheckMoveInput();
			CheckMoveCamera();
			CheckScroll();
			CheckRotate();
			CheckLookAtCharacter();
		}

		private void CheckLookAtCharacter()
		{
			LookAtCharacter = Input.GetKey(KeyCode.Space);
		}

		private void CheckRotate()
		{
			var rotationInput = 0;
			if (Input.GetKey(KeyCode.Q))
				rotationInput++;
			if (Input.GetKey(KeyCode.E))
				rotationInput--;
			RotateValue = rotationInput;
		}

		private void CheckScroll()
		{
			ZoomValue = Input.GetAxis("Mouse ScrollWheel");
		}

		private void CheckMoveCamera()
		{
			var moveCameraDirection = new Vector2();
			if (Input.GetKey(KeyCode.W))
				moveCameraDirection.y += 1;
			if (Input.GetKey(KeyCode.S))
				moveCameraDirection.y -= 1;
			if (Input.GetKey(KeyCode.D))
				moveCameraDirection.x += 1;
			if (Input.GetKey(KeyCode.A))
				moveCameraDirection.x -= 1;
			MoveCameraDirection = moveCameraDirection;
		}

		private void CheckMoveInput()
		{
			if (Input.GetMouseButtonDown(0))
			{
				var ray = _renderCamera.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out var hit))
				{
					MoveCharacterEvent?.Invoke(hit.point);
				}
			}
		}
	}
}