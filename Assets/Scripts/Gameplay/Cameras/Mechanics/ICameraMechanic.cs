using Gameplay.Character;
using Gameplay.InputSystems;

namespace Gameplay.Cameras.Mechanics
{
	public interface ICameraMechanic
	{
		void Construct( // так как без DI тут довольно много зависимостей для всех механик
			IInputService inputService,
			ActorSelectorService actorSelectorService
		);

		public void Enable();
		public void UpdateMechanic(float deltaTime);
		public void Disable();
	}
}