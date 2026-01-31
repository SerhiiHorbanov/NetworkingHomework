using Unity.Netcode;
using UnityEngine;

struct PlayerMovementSyncData : INetworkSerializeByMemcpy
{
    public Vector3 Position;
    public Vector3 Velocity;
    public Vector3 MoveDir;
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerCharacterMovement))]
public class PlayerMovementSync : NetworkBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerCharacterMovement _movement;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _movement = GetComponent<PlayerCharacterMovement>();
    }

    void Update()
    {
        PlayerMovementSyncData movementSyncData = new()
        {
            Position = transform.position,
            Velocity = _rigidbody.linearVelocity,
            MoveDir = _movement._GlobalMoveDir
        };

        if (IsOwner)
        {
            SendToServerRPC(movementSyncData);
        }
    }

    [ServerRpc]
    private void SendToServerRPC(PlayerMovementSyncData movementData, ServerRpcParams rpc = default)
    {
        ApplyMovementData(movementData);
        SendToClientsRPC(movementData);
    }

    private void ApplyMovementData(PlayerMovementSyncData movementData)
    {
        transform.position = movementData.Position;
        _rigidbody.linearVelocity = movementData.Velocity;
        _movement._GlobalMoveDir = movementData.MoveDir;
    }

    [Rpc(SendTo.NotOwner)]
    private void SendToClientsRPC(PlayerMovementSyncData movementData)
    {
        ApplyMovementData(movementData);
    }
}
