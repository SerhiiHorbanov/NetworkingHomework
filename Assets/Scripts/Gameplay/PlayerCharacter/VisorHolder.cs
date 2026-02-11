using ScriptableObjects;
using UnityEngine;

namespace Gameplay.PlayerCharacter
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class VisorHolder : MonoBehaviour
    {
        [SerializeField] private VisorSO _DefaultVisor;
        [SerializeField] private Transform _VisorParent;
        
        private GameObject _visorInstance;
        
        private PlayerCharacter _playerCharacter;
        private PlayerLoadout _loadout;
        
        private void Awake()
        {
            ApplyVisor(_DefaultVisor);
            _playerCharacter = GetComponent<PlayerCharacter>();
            _playerCharacter.OnSpawned += AttachLoadout;
        }

        private void AttachLoadout()
        {
            _loadout = _playerCharacter._PlayerLoadout;
            
            _loadout.OnVisorChanged += ApplyVisor;
        }

        public void OnDestroy()
        {
            _loadout.OnVisorChanged -= ApplyVisor;
            _playerCharacter.OnSpawned -= AttachLoadout;
        }
        
        private void ApplyVisor(VisorSO visor)
        {
            Destroy(_visorInstance);
            
            _visorInstance = Instantiate(visor._Prefab, _VisorParent);
        }
    }
}
