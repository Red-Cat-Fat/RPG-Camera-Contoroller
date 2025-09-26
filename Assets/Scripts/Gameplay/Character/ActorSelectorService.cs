using System;
using System.Collections.Generic;

namespace Gameplay.Character
{
	public class ActorSelectorService
	{
		public event Action<Actor> ActorSelectedChangeEvent;
		private readonly List<Actor> _actor = new();
		private Actor _selectedActor;

		public void SelectCharacter(Actor actor)
		{
			if (!_actor.Contains(actor))
				return;
			_selectedActor = actor;
			ActorSelectedChangeEvent?.Invoke(_selectedActor);
		}

		public void AddCharacter(Actor actor)
		{
			_actor.Add(actor);
			if (_selectedActor == null)
				SelectCharacter(actor);
		}

		public Actor GetActiveCharacter()
		{
			return _selectedActor;
		}
	}
}