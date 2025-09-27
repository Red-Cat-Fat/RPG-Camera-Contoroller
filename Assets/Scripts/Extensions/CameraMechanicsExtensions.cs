using Gameplay.Cameras.Mechanics;

namespace Extensions
{
	public static class CameraMechanicsExtensions
	{
		public static void Enable(this BaseCameraMechanic[] mechanics)
		{
			for (var i = 0; i < mechanics.Length; i++)
				mechanics[i].Enable();
		}
		public static void Update(this BaseCameraMechanic[] mechanics, float deltaTime)
		{
			for (var i = 0; i < mechanics.Length; i++)
				mechanics[i].UpdateMechanic(deltaTime);
		}
		public static void Disable(this BaseCameraMechanic[] mechanics)
		{
			for (var i = 0; i < mechanics.Length; i++)
				mechanics[i].Disable();
		}
	}
}