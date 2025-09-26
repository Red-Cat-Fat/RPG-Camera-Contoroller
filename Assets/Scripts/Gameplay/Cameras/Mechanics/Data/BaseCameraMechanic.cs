namespace Gameplay.Cameras.Mechanics.Data
{
	public abstract class BaseCameraMechanic : ICameraMechanic
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

		protected abstract void DoEnable();
		protected abstract void DoUpdate(float deltaTime);

		protected virtual void DoDisable()
		{
		}
	}
}