using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacterController : MonoBehaviour
{
	private PlayerCharacter _playerCharacter;
	private PlayerCharacterMovement _playerCharacterMovement;
	
	private CharacterLook _characterLook;

	private Weapon _weapon;
	
	private PlayerInput _playerInput;

	private Vector2 _relativeMoveDir;
	
	private void Awake()
	{
		OwnedPlayerSpawnEventBus.PlayerSpawnEvent += AttachToCharacter;
		
		_playerInput = GetComponent<PlayerInput>();
		_playerInput.actions["Move"].performed += Move;
		_playerInput.actions["Move"].canceled += Move;

		_playerInput.actions["Look"].performed += Look;
		
		_playerInput.actions["Attack"].started += Attack;
	}
	
	private void Move(InputAction.CallbackContext context)
	{
		_relativeMoveDir = context.ReadValue<Vector2>();
	}

	private void FixedUpdate()
	{
		_playerCharacterMovement?.SetMoveDirFromRelative(_relativeMoveDir);
	}
	
	private void Look(InputAction.CallbackContext context)
	{
		Vector2 delta = context.ReadValue<Vector2>();
		_characterLook?.RotateLook(delta);
	}
	
	private void Attack(InputAction.CallbackContext obj)
	{
		_weapon?.Shoot();
	}

	private void OnDestroy()
	{
		_playerInput.actions["Move"].performed -= Move;
		_playerInput.actions["Move"].canceled -= Move;

		_playerInput.actions["Look"].performed -= Look;
		
		_playerInput.actions["Attack"].started -= Attack;
		
		OwnedPlayerSpawnEventBus.PlayerSpawnEvent -= AttachToCharacter;
	}

	private void AttachToCharacter(PlayerCharacter character)
	{
		_playerCharacter = character;
		_playerCharacterMovement = character.GetComponent<PlayerCharacterMovement>();
		_characterLook = character.GetComponent<CharacterLook>();
		_weapon = character.GetComponent<Weapon>();
	}
}