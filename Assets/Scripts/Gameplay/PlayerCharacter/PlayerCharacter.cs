using Unity.Netcode;
using UnityEngine;

namespace Gameplay.PlayerCharacter
{
    public class PlayerCharacter : NetworkBehaviour
    {
        [SerializeField] private GameObject _PlayerCharacterControllerPrefab;
    
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                GameObject controller = Instantiate(_PlayerCharacterControllerPrefab, transform);
                controller.GetComponent<PlayerCharacterController>();
                OwnedPlayerSpawnEventBus.Invoke(this);
            }
        }
    
        public override void OnNetworkDespawn()
        {
            PlayerCharacterDespawnedEventBus.Invoke(OwnerClientId);
        }
    }
}