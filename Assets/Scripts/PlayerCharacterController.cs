using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacterController : MonoBehaviour
{
	private PlayerCharacter _playerCharacter;
	private PlayerInput _playerInput;
	
	private void Awake()
	{
		OwnedPlayerSpawnEventBus.PlayerSpawnEvent += OnCharacterSpawned;
		
		_playerInput = GetComponent<PlayerInput>();
		_playerInput.actions["Move"].performed += Move;
	}
	
	private void Move(InputAction.CallbackContext c)
	{
		_playerCharacter.transform.position += (Vector3)c.ReadValue<Vector2>() * 0.1f;
	}

	private void OnCharacterSpawned(PlayerCharacter character)
	{
		_playerCharacter = character;
	}
	
	private void OnDestroy()
	{
		OwnedPlayerSpawnEventBus.PlayerSpawnEvent -= OnCharacterSpawned;
	}
}