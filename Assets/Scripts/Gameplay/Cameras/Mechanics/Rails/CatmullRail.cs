using System;
using UnityEngine;

namespace Gameplay.Cameras.Mechanics.Rails
{
	public class CatmullRail : MonoBehaviour
	{
		[SerializeField] private Transform[] _points = Array.Empty<Transform>();
#if UNITY_EDITOR
		[SerializeField] private int _debugSamples = 10;
		[SerializeField] private int _debugDirectionLenght = 1;
#endif

		public Vector3 GetPathPoint(float progress, out Quaternion rotation)
		{
			rotation = Quaternion.identity;
			progress = Mathf.Clamp01(progress);

			var lastIndex = _points.Length - 1;
			var minDistSegment = progress * lastIndex;

			var indexSegment = Mathf.FloorToInt(minDistSegment);
			var localSegment = minDistSegment - indexSegment;

			var i0 = Mathf.Clamp(indexSegment - 1, 0, lastIndex);
			var i1 = Mathf.Clamp(indexSegment, 0, lastIndex);
			var i2 = Mathf.Clamp(indexSegment + 1, 0, lastIndex);
			var i3 = Mathf.Clamp(indexSegment + 2, 0, lastIndex);

			var p0 = _points[i0].position;
			var p1 = _points[i1].position;
			var p2 = _points[i2].position;
			var p3 = _points[i3].position;

			var position = CatmullRom(
				p0,
				p1,
				p2,
				p3,
				localSegment
			);

			rotation = Quaternion.Slerp(_points[i1].rotation, _points[i2].rotation, localSegment);

			return position;
		}

		/// <summary>
		/// Catmull-Rom формула
		/// </summary>
		private Vector3 CatmullRom(
			Vector3 p0,
			Vector3 p1,
			Vector3 p2,
			Vector3 p3,
			float t
		)
		{
			var t2 = t * t;
			var t3 = t2 * t;

			return 0.5f
					* (
						(2f * p1)
						+ (-p0 + p2) * t
						+ (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2
						+ (-p0 + 3f * p1 - 3f * p2 + p3) * t3
					);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_points == null
				|| _points.Length < 2)
			{
				Debug.LogError("SimpleRail: недостаточно точек для построения пути");
			}
		}

		private void OnDrawGizmos()
		{
			if (_points == null
				|| _points.Length < 2)
				return;

			var colorFrom = Color.green;
			var colorTo = Color.red;
			Gizmos.color = colorFrom;

			var prevPos = GetPathPoint(0f, out _);
			for (var i = 1; i <= _debugSamples; i++)
			{
				var progress = i / (float)_debugSamples;
				var pos = GetPathPoint(progress, out var rotation);
				Gizmos.DrawLine(prevPos, pos);
				Gizmos.color = Color.Lerp(colorFrom, colorTo, progress);
				//Gizmos.DrawRay(pos, rotation * Vector3.forward * _debugDirectionLenght);
				
				prevPos = pos;
			}
		}
#endif
	}
}