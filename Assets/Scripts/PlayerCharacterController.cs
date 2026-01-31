using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacterController : MonoBehaviour
{
	private PlayerCharacter _playerCharacter;
	private PlayerCharacterMovement _playerCharacterMovement;
	
	private PlayerInput _playerInput;
	
	private void Awake()
	{
		OwnedPlayerSpawnEventBus.PlayerSpawnEvent += AttachToCharacter;
		
		_playerInput = GetComponent<PlayerInput>();
		_playerInput.actions["Move"].performed += Move;
		_playerInput.actions["Move"].canceled += Move;
	}
	
	private void Move(InputAction.CallbackContext context)
	{
		Vector2 relativeDirection = context.ReadValue<Vector2>();
		Vector3 globalMoveDir = transform.forward * relativeDirection.y + transform.right * relativeDirection.x;
		
		_playerCharacterMovement._GlobalMoveDir = globalMoveDir;
	}
	
	private void OnDestroy()
	{
		OwnedPlayerSpawnEventBus.PlayerSpawnEvent -= AttachToCharacter;
	}

	private void AttachToCharacter(PlayerCharacter character)
	{
		_playerCharacter = character;
		_playerCharacterMovement = character.GetComponent<PlayerCharacterMovement>();
	}
}