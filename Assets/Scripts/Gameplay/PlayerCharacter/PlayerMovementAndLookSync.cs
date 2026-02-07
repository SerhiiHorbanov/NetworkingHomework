using Unity.Netcode;
using UnityEngine;

struct SyncData : INetworkSerializeByMemcpy
{
    public Vector3 Position;
    public Vector3 Velocity;
    public Vector3 MoveDir;
    public Vector3 LookDirection;
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerCharacterMovement))]
[RequireComponent(typeof(CharacterLook))]
public class PlayerMovementAndLookSync : NetworkBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerCharacterMovement _movement;
    private CharacterLook _look;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _movement = GetComponent<PlayerCharacterMovement>();
        _look = GetComponent<CharacterLook>();
    }

    void Update()
    {
        SyncData movementSyncData = new()
        {
            Position = transform.position,
            Velocity = _rigidbody.linearVelocity,
            MoveDir = _movement._GlobalMoveDir,
            LookDirection = _look.LookDirectionInEulerAngles,
        };

        if (IsOwner)
        {
            SendToServerRPC(movementSyncData);
        }
    }

    [ServerRpc(Delivery = RpcDelivery.Unreliable)]
    private void SendToServerRPC(SyncData movementData)
    {
        ApplyMovementData(movementData);
        SendToClientsRPC(movementData);
    }

    private void ApplyMovementData(SyncData movementData)
    {
        transform.position = movementData.Position;
        _rigidbody.linearVelocity = movementData.Velocity;
        _movement._GlobalMoveDir = movementData.MoveDir;
        _look.SetLook(movementData.LookDirection);
    }

    [Rpc(SendTo.NotOwner, Delivery = RpcDelivery.Unreliable)]
    private void SendToClientsRPC(SyncData movementData)
    {
        ApplyMovementData(movementData);
    }
}
