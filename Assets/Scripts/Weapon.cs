using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

struct ShotGraphicsInfo : INetworkSerializeByMemcpy
{
	public Vector3 Origin;
	
	public bool DidHit;
	public Vector3 Hit;
	public Vector3 HitNormal;
}

public class Weapon : NetworkBehaviour
{
	[SerializeField] private Transform _ShootingOrigin;
	[SerializeField] private float _Damage;
	[SerializeField] private GameObject _TracePrefab;
	[SerializeField] private GameObject _DecalPrefab;
	
	private const float RaycastLength = 10_000f;
	
	public void Shoot()
	{
		ShootServerRPC();
	}
	
	[ServerRpc]
	private void ShootServerRPC()
	{
		bool didHit = false;
		
		if (Physics.Raycast(_ShootingOrigin.position, _ShootingOrigin.forward, out RaycastHit hit, RaycastLength))
		{
			didHit = true;
			print($"hit {hit.collider.gameObject.name}");
			if (hit.collider.TryGetComponent(out CharacterHealth characterHealth))
			{
				print("and it has health");
				characterHealth.DamageWithSync(_Damage);
			}
		}

		ShotGraphicsInfo shotGraphicsInfo = new()
		{
			Origin = _ShootingOrigin.position,
			DidHit = didHit,
			Hit = hit.point,
			HitNormal = hit.normal,
		};
		
		DoShotGraphicsClientRPC(shotGraphicsInfo);
	}

	[ClientRpc]
	private void DoShotGraphicsClientRPC(ShotGraphicsInfo shotGraphicsInfo)
	{
		print($"origin: {shotGraphicsInfo.Origin}, hit: {shotGraphicsInfo.Hit}");
	}
}