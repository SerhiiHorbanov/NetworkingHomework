using Unity.Netcode;
using UnityEngine;

public class CharacterHealth : NetworkBehaviour
{
	[SerializeField] public float _Hp;

	public void DamageWithoutSync(float damage)
	{
		_Hp -= damage;
	}

	public void DamageWithSync(float damage)
	{
		DamageWithoutSync(damage);
		
		if (IsServer)
			SyncHealth();
	}
	
	private void SyncHealth()
		=> SyncHealthClientRPC(_Hp);
	
	[ClientRpc]
	public void SyncHealthClientRPC(float hp)
	{
		_Hp = hp;
	}
}