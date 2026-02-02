using System;
using Unity.Netcode;
using UnityEngine;

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
		Nm.OnServerStarted += Enable;
		Nm.OnServerStopped += Disable;
		
		Nm.OnClientConnectedCallback += OnClientConnected;
	}

	private void UnsubscribeFromNetworkManager()
	{
		Nm.OnServerStarted -= Enable;
		Nm.OnServerStopped -= Disable;
		
		Nm.OnClientConnectedCallback -= OnClientConnected;
	}
	
	private void Enable()
		=> gameObject.SetActive(true);
	private void Disable(bool _)
		=> gameObject.SetActive(false);
	
	private void OnClientConnected(ulong clientId)
	{
		GameObject character = Instantiate(_PlayerCharacterPrefab);
		NetworkObject netObj = character.GetComponent<NetworkObject>();
		netObj.SpawnWithOwnership(clientId);
	}
}
