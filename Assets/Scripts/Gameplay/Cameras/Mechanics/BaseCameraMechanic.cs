using Gameplay.Character;
using Gameplay.InputSystems;
using Infrastructure;

namespace Gameplay.Cameras.Mechanics
{
	public abstract class BaseCameraMechanic : MonoConstruct
	{
		private bool _enabled;

		public void Enable()
		{
			if (_enabled)
				return;

			_enabled = true;
			DoEnable();
		}

		public void UpdateMechanic(float deltaTime)
		{
			if (_enabled)
				DoUpdate(deltaTime);
		}

		public void Disable()
		{
			if (!_enabled)
				return;

			_enabled = false;
			DoDisable();
		}
		
		public abstract void Construct(
			IInputService inputService,
			ActorSelectorService actorSelectorService
		);

		protected abstract void DoUpdate(float deltaTime);
		protected abstract void DoEnable();

		protected virtual void DoDisable()
		{
		}
	}
}