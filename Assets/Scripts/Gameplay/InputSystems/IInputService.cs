using System;
using UnityEngine;

namespace Gameplay.InputSystems
{
	public interface IInputService
	{
		public event Action<Vector3> MoveCharacterEvent;
		public Vector2 MoveCameraDirection { get; }
		public float ZoomValue { get; }
		public int RotateValue { get; }
		bool LookAtCharacter { get; }
	}
}