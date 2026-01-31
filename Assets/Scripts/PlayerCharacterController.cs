using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacterController : MonoBehaviour
{
	private PlayerCharacter _playerCharacter;
	private PlayerCharacterMovement _playerCharacterMovement;
	private CharacterLook _characterLook;
	
	private PlayerInput _playerInput;
	
	private void Awake()
	{
		OwnedPlayerSpawnEventBus.PlayerSpawnEvent += AttachToCharacter;
		
		_playerInput = GetComponent<PlayerInput>();
		_playerInput.actions["Move"].performed += Move;
		_playerInput.actions["Move"].canceled += Move;

		_playerInput.actions["Look"].performed += Look;
	}
	
	private void Move(InputAction.CallbackContext context)
	{
		Vector2 relativeDirection = context.ReadValue<Vector2>();
		Vector3 globalMoveDir = transform.forward * relativeDirection.y + transform.right * relativeDirection.x;
		
		_playerCharacterMovement._GlobalMoveDir = globalMoveDir;
	}
	
	private void Look(InputAction.CallbackContext context)
	{
		Vector2 delta = context.ReadValue<Vector2>();
		print(delta);
		_characterLook.RotateLook(delta);
	}
	
	private void OnDestroy()
	{
		OwnedPlayerSpawnEventBus.PlayerSpawnEvent -= AttachToCharacter;
	}

	private void AttachToCharacter(PlayerCharacter character)
	{
		_playerCharacter = character;
		_playerCharacterMovement = character.GetComponent<PlayerCharacterMovement>();
		_characterLook = character.GetComponent<CharacterLook>();
	}
}