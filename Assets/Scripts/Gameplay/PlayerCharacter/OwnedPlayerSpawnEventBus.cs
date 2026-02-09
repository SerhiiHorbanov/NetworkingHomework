using System;

namespace Gameplay.PlayerCharacter
{
	public static class OwnedPlayerSpawnEventBus
	{
		private static Action<PlayerCharacter> _playerSpawnEvent;
	
		public static event Action<PlayerCharacter> PlayerSpawnEvent
		{
			add => _playerSpawnEvent += value;
			remove => _playerSpawnEvent -= value;
		}

		public static void Invoke(PlayerCharacter playerCharacter)
		{
			_playerSpawnEvent?.Invoke(playerCharacter);
		}
	}
}