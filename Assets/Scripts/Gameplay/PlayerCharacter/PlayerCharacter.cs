using System;
using Tools;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.PlayerCharacter
{
    public class PlayerCharacter : NetworkBehaviour
    {
        [SerializeField] private GameObject _PlayerCharacterControllerPrefab;
    
        public PlayerLoadout _PlayerLoadout;

        private Action _onSpawned;
        public event Action OnSpawned
        {
            add => _onSpawned += value;
            remove => _onSpawned -= value;
        }
        
        public override void OnNetworkSpawn()
        {
            NetworkObject playerState = this.GetPlayerObjectOfOwner();
            _PlayerLoadout = playerState.GetComponent<PlayerLoadout>();
            
            if (IsOwner)
            {
                GameObject controller = Instantiate(_PlayerCharacterControllerPrefab, transform);
                controller.GetComponent<PlayerCharacterController>();
                OwnedPlayerSpawnEventBus.Invoke(this);
            }

            _onSpawned?.Invoke();
        }
    
        public override void OnNetworkDespawn()
        {
            PlayerCharacterDespawnedEventBus.Invoke(OwnerClientId);
        }
    }
}