using System;

public static class PlayerCharacterDespawnedEventBus
{
	private static Action<ulong> _playerDespawnedEvent;
	
	public static event Action<ulong> PlayerCharacterDespawnedEvent
	{
		add => _playerDespawnedEvent += value;
		remove => _playerDespawnedEvent -= value;
	}

	public static void Invoke(ulong clientId)
	{
		_playerDespawnedEvent?.Invoke(clientId);
	}
}