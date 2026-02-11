using System;
using System.Collections;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
	struct LoadoutSyncInfo : INetworkSerializeByMemcpy
	{
		public int VisorID;
	}
	
	public class PlayerLoadout : NetworkBehaviour
	{
		private bool _didSynchroniseData;
		
		[SerializeField] private VisorsListSO _VisorsList;
		[SerializeField] private VisorSO _Visor;
		private int _visorID;

		private Action<VisorSO> _visorChanged;
		public event Action<VisorSO> OnVisorChanged
		{
			add => _visorChanged += value;
			remove => _visorChanged -= value;
		}
		
		
		
		public override void OnNetworkSpawn()
		{
			if (IsOwner)
			{
				SendLoadoutToServer();
				
			}
			else if (!_didSynchroniseData)
			{
				RequestLoadoutData();
			}
		}

		private void SendLoadoutToServer()
			=> SendLoadoutToServerServerRPC(CreateSyncInfo());

		[ServerRpc]
		private void SendLoadoutToServerServerRPC(LoadoutSyncInfo syncInfo, ServerRpcParams rpcParams = default)
		{
			SetVisorFromID(syncInfo.VisorID);
			SendLoadoutToClientsClientRPC(syncInfo);
		}
		
		[Rpc(SendTo.NotOwner)]
		private void SendLoadoutToClientsClientRPC(LoadoutSyncInfo syncInfo)
		{
			ApplySyncInfo(syncInfo);
			_didSynchroniseData = true;
		}

		private void RequestLoadoutData()
			=> RequestLoadoutDataServerRPC();

		[ServerRpc]
		private void RequestLoadoutDataServerRPC(ServerRpcParams rpcParams = default)
		{
			ulong requester = rpcParams.Receive.SenderClientId;
			LoadoutSyncInfo syncInfo = CreateSyncInfo();
			SendLoadoutToClientRpc(syncInfo, RpcTarget.Single(requester, RpcTargetUse.Temp));
		}

		[Rpc(SendTo.SpecifiedInParams)]
		private void SendLoadoutToClientRpc(LoadoutSyncInfo syncInfo, RpcParams rpcParams = default)
		{
			ApplySyncInfo(syncInfo);
		}


		
		private LoadoutSyncInfo CreateSyncInfo()
			=> new()
			{
				VisorID = _visorID,
			};
		
		private void ApplySyncInfo(LoadoutSyncInfo syncInfo)
		{
			SetVisorFromID(syncInfo.VisorID);
		}
		
		private void SetVisorFromID(int visorID)
		{
			_visorID = visorID;
			_Visor = _VisorsList.GetByID(visorID);
			_visorChanged?.Invoke(_Visor);
		}
	}
}
