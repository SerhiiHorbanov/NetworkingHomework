using Unity.Netcode;
using UnityEngine;

public class PlayerState : NetworkBehaviour
{
    [SerializeField] private GameObject _PlayerCharacterPrefab;
    [SerializeField] private GameObject _PlayerControllerPrefab;
    
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            InstantiateCharacterController();
            RequestSpawnCharacterServerRpc();
        }
    }
    
    [ServerRpc]
    private void RequestSpawnCharacterServerRpc(ServerRpcParams rpc = default)
    {
        ulong clientId = rpc.Receive.SenderClientId;
        GameObject character = Instantiate(_PlayerCharacterPrefab);
        NetworkObject netObj = character.GetComponent<NetworkObject>();
        netObj.SpawnWithOwnership(clientId);
    }
    private void InstantiateCharacterController()
    {
        Instantiate(_PlayerControllerPrefab);
    }
}
