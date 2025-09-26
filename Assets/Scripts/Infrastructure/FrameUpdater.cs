using System;
using UnityEngine;

namespace Infrastructure
{
	public class FrameUpdater : MonoBehaviour
	{
		public event Action<float> UpdateEvent;

		public void Update()
		{
			UpdateEvent?.Invoke(Time.deltaTime);
		}
	}
}