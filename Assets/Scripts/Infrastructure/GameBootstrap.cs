using Gameplay.Cameras;
using Gameplay.Character;
using Gameplay.InputSystems;
using UnityEngine;

namespace Infrastructure
{
	[RequireComponent(typeof(FrameUpdater))]
	public class GameBootstrap : MonoBehaviour
	{
		[SerializeField] private GameObject CharacterPrefab;
		[SerializeField] private GameObject CameraRigPrefab;
		[SerializeField] private Transform SpawnPlayerPoint;
		[SerializeField] private FrameUpdater _frameUpdater;

		public void Start()
		{
			var player = Instantiate(CharacterPrefab, SpawnPlayerPoint.position, Quaternion.identity);
			var cameraRig = Instantiate(CameraRigPrefab, SpawnPlayerPoint.position, Quaternion.identity);

			var character = player.GetComponent<Actor>();
			var rig = cameraRig.GetComponent<RigCamera>();

			var actorSelector = new ActorSelectorService();
			actorSelector.AddCharacter(character);
			var input = new UnityEditorInputService(_frameUpdater, rig.CameraLink);

			character.Construct(input);
			rig.Construct(input, actorSelector);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_frameUpdater == null)
				_frameUpdater = GetComponent<FrameUpdater>();
		}
#endif
	}
}