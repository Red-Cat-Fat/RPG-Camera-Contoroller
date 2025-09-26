using Gameplay.InputSystems;

namespace Gameplay.Cameras.Mechanics.Data
{
	public interface ICameraMechanicData
	{
		public ICameraMechanic MakeMechanic(IInputService inputService, RigCamera rigCamera);
	}
}