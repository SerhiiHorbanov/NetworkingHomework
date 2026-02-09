using Unity.Netcode;

namespace Tools
{
	public static class NetworkingExtensions
	{
		public static NetworkClient GetOwnerClient(this NetworkBehaviour nb)
			=> NetworkManager.Singleton.ConnectedClients[nb.OwnerClientId];
		
		public static NetworkObject GetPlayerObjectOfOwner(this NetworkBehaviour nb)
			=> nb.GetOwnerClient().PlayerObject;
	}
}
