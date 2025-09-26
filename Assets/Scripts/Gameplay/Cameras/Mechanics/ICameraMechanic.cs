namespace Gameplay.Cameras.Mechanics
{
	public interface ICameraMechanic
	{
		public void Enable();
		public void UpdateMechanic(float deltaTime);
		public void Disable();
	}
}