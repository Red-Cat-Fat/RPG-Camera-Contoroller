using Gameplay.Character;
using Gameplay.InputSystems;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Cameras.Mechanics
{
	public class HeightCameraMechanic : BaseCameraMechanic
	{
		[Header("References")] [SerializeField]
		private Transform _rigTransform;

		[SerializeField] private Transform _cameraTransform;

		[Header("Height Settings")] [SerializeField]
		private float _minCameraHeight = 2.0f;

		[SerializeField] private float _minRigHeight;
		[SerializeField] private float _adjustmentSpeedUp = 15f;
		[SerializeField] private float _adjustmentSpeedDown = 5f;

		[Header("NavMesh Settings")] 
		[SerializeField]
		private float _sampleMaxDistance = 20f;

		public override void Construct(IInputService inputService, ActorSelectorService actorSelectorService)
		{
			
		}

		protected override void DoEnable()
		{
			
		}

		protected override void DoUpdate(float deltaTime)
		{
			var targetRigHeight = CalculateTargetRigHeight();
			MoveRigToHeight(targetRigHeight, deltaTime);
		}

		private float CalculateTargetRigHeight()
		{
			var currentCameraDeltaHeight = GetCameraDeltaHeight();
			var currentRigDeltaHeight = GetRigDeltaHeight();

			if (currentCameraDeltaHeight < _minCameraHeight)
			{
				var addOffset = _minCameraHeight - currentCameraDeltaHeight;
				var groundHeight = GetRealGroundHeightAtRigPosition();
				return groundHeight + currentRigDeltaHeight + addOffset;
			}

			if (currentRigDeltaHeight > _minRigHeight)
			{
				var groundHeight = GetRealGroundHeightAtRigPosition();
				return groundHeight + _minRigHeight;
			}

			return _rigTransform.position.y;
		}

		private float GetCameraDeltaHeight()
		{
			var cameraPosition = _cameraTransform.position;
			var groundHeight = GetRealGroundHeightAtPosition(cameraPosition);
			return cameraPosition.y - groundHeight;
		}

		private float GetRigDeltaHeight()
		{
			var rigPosition = _rigTransform.position;
			var groundHeight = GetRealGroundHeightAtRigPosition();
			return rigPosition.y - groundHeight;
		}

		private float GetRealGroundHeightAtRigPosition()
		{
			return GetRealGroundHeightAtPosition(_rigTransform.position);
		}

		private float GetRealGroundHeightAtPosition(Vector3 worldPosition)
		{
			if (NavMesh.SamplePosition(
					worldPosition,
					out var hit,
					_sampleMaxDistance,
					NavMesh.AllAreas
				))
			{
				return hit.position.y;
			}

			return GetGroundHeightWithRaycast(worldPosition);
		}

		private float GetGroundHeightWithRaycast(Vector3 worldPosition)
		{
			var ray = new Ray(worldPosition + Vector3.up * 10f, Vector3.down);

			return Physics.Raycast(ray, out var hitInfo, 100f)
				? hitInfo.point.y
				: 0f;
		}

		private void MoveRigToHeight(float targetHeight, float deltaTime)
		{
			var currentRigHeight = _rigTransform.position.y;

			var speed = currentRigHeight < targetHeight
				? _adjustmentSpeedUp
				: _adjustmentSpeedDown;

			var newHeight = Mathf.Lerp(
				currentRigHeight,
				targetHeight,
				speed * deltaTime
			);

			var newPosition = _rigTransform.position;
			newPosition.y = newHeight;
			_rigTransform.position = newPosition;
		}
	}
}