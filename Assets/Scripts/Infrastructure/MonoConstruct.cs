using System;
using UnityEngine;

namespace Infrastructure
{
	public abstract class MonoConstruct : MonoBehaviour
	{
		private bool _isConstructed;
		protected bool IsInitialized { get; private set; }

		private void OnEnable()
		{
			Initialize();
		}

		private void OnDisable()
		{
			Deinitialize();
		}

		protected void FinishedInitialization()
		{
			_isConstructed = true;
			Initialize();
		}

		private void Initialize()
		{
			if (!_isConstructed || IsInitialized)
				return;

			DoInitialized();
			IsInitialized = true;
		}

		private void Deinitialize()
		{
			if (!_isConstructed || !IsInitialized)
				return;
			DoDeinitialized();
			IsInitialized = false;
		}

		protected virtual void DoInitialized()
		{
		}

		protected virtual void DoDeinitialized()
		{
		}
	}
}