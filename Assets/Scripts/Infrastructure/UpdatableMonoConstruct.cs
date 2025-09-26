using UnityEngine;

namespace Infrastructure
{
	public abstract class UpdatableMonoConstruct : MonoConstruct
	{
		private void Update()
		{
			if (IsInitialized)
				DoUpdate(Time.deltaTime);
		}

		protected abstract void DoUpdate(float deltaTime);
	}
}