using Gameplay.PlayerCharacter;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay
{
	public class PlayerCharacterSpawner : MonoBehaviour
	{
		[SerializeField] private GameObject _PlayerCharacterPrefab;
	
		private NetworkManager Nm => NetworkManager.Singleton;
	
		// We use Start here because NetworkManager.Singleton is not available in Awake
		private void Start()
		{
			SubscribeToNetworkManager();

			gameObject.SetActive(false);
		}
	
		private void OnDestroy()
		{
			UnsubscribeFromNetworkManager();
		}
	
		private void SubscribeToNetworkManager()
		{
			if (Nm is null)
				return;

			Nm.OnServerStarted += OnServerStarted;
			Nm.OnServerStopped += OnServerStopped;
		
			Nm.OnClientConnectedCallback += OnClientConnected;
		}

		private void UnsubscribeFromNetworkManager()
		{
			if (Nm is null)
				return;
		
			Nm.OnServerStarted -= OnServerStarted;
			Nm.OnServerStopped -= OnServerStopped;
		
			Nm.OnClientConnectedCallback -= OnClientConnected;
		}
	
		private void OnServerStarted()
		{
			gameObject.SetActive(true);
			PlayerCharacterDespawnedEventBus.PlayerCharacterDespawnedEvent += SpawnCharacterForClient;
		}

		private void OnServerStopped(bool _)
		{
			gameObject.SetActive(false);
			PlayerCharacterDespawnedEventBus.PlayerCharacterDespawnedEvent -= SpawnCharacterForClient;
		}
	
		private void OnClientConnected(ulong clientId)
		{
			if (!Nm.IsServer)
				return;

			SpawnCharacterForClient(clientId);
		}
	
		private void SpawnCharacterForClient(ulong clientId)
		{
			GameObject character = Instantiate(_PlayerCharacterPrefab);
			NetworkObject netObj = character.GetComponent<NetworkObject>();
			netObj.SpawnWithOwnership(clientId);
		}
	}
}
