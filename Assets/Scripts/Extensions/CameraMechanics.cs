using Gameplay.Cameras.Mechanics;

namespace Extensions
{
	public static class CameraMechanics
	{
		public static void Enable(this ICameraMechanic[] mechanics)
		{
			for (var i = 0; i < mechanics.Length; i++)
				mechanics[i].Enable();
		}
		public static void Update(this ICameraMechanic[] mechanics, float deltaTime)
		{
			for (var i = 0; i < mechanics.Length; i++)
				mechanics[i].UpdateMechanic(deltaTime);
		}
		public static void Disable(this ICameraMechanic[] mechanics)
		{
			for (var i = 0; i < mechanics.Length; i++)
				mechanics[i].Disable();
		}
	}
}