using UnityEditor;
using UnityEngine;

namespace Gameplay.Cameras
{
	public class RigCameraGizmoDrawer : MonoBehaviour
	{
#if UNITY_EDITOR
		public Transform LookPoint;
		public Transform RotationPivot;
		public Transform TargetCameraPosition;

		private void OnDrawGizmos()
		{
			if (RotationPivot != null)
			{
				Gizmos.color = Color.yellow * 0.7f;
				var start = RotationPivot.position - RotationPivot.rotation * RotationPivot.up * 5;
				var end = RotationPivot.position + RotationPivot.rotation * RotationPivot.up * 5;
				Gizmos.DrawLine(start, end);
				Handles.Label(end, "Rotation Axis");
			}
			
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(transform.position, 0.3f);

			if (LookPoint != null)
			{
				Gizmos.DrawLine(transform.position, LookPoint.position);
				Gizmos.DrawWireSphere(LookPoint.position, 0.3f);
				Handles.Label(LookPoint.position + Vector3.up * 0.25f, "LookPoint");
			}

			if (TargetCameraPosition != null)
			{
				Gizmos.color = Color.gray;
				Gizmos.DrawCube(TargetCameraPosition.position, Vector3.one * 0.25f);
#if UNITY_EDITOR
				Handles.Label(TargetCameraPosition.position + Vector3.up * 0.25f, "TargetCameraPosition");
#endif
			}
		}
#endif
	}
}