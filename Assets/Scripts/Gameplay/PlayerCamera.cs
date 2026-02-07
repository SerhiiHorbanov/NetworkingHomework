using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private CharacterLook _look;

    private void Awake()
    {
        OwnedPlayerSpawnEventBus.PlayerSpawnEvent += SetCharacter;
    }

    private void SetCharacter(PlayerCharacter character)
    {
        _look = character.GetComponent<CharacterLook>();
    }


    private void LateUpdate()
    {
        if (_look != null)
        {
            Transform viewPoint = _look._Viewpoint;
            transform.position = viewPoint.position;
            transform.rotation = viewPoint.rotation;
        }
    }
}
