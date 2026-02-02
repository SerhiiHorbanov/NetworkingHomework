using Unity.Netcode;
using UnityEngine;

public class PlayerState : NetworkBehaviour
{
    [SerializeField] private GameObject _PlayerControllerPrefab;
    
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            InstantiateCharacterController();
        }
    }
    
    private void InstantiateCharacterController()
    {
        Instantiate(_PlayerControllerPrefab);
    }
}
