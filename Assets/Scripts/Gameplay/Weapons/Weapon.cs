using Gameplay.PlayerCharacter;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Weapons
{
	public class Weapon : NetworkBehaviour
	{
		[SerializeField] private Transform _ShootingOrigin;
		[SerializeField] private float _InstantDamage;
		[SerializeField] private DamageOverTime _DamageOverTime;
		[SerializeField] private float _MinDPSOfDOT;
		[SerializeField] private float _MaxDPSOfDOT;
		[SerializeField] private GameObject _ShotGraphicsPrefab;
	
		private const float RaycastLength = 10_000f;
	
		public void Shoot()
		{
			ShootServerRPC();
		}
	
		[ServerRpc]
		private void ShootServerRPC()
		{
			bool didHit = Physics.Raycast(_ShootingOrigin.position, _ShootingOrigin.forward, out RaycastHit hit, RaycastLength);
		
			if (didHit)
			{
				if (hit.collider.TryGetComponent(out CharacterHealth characterHealth))
				{
					characterHealth.DamageWithSync(_InstantDamage);
					DamageOverTime dot = _DamageOverTime.WithDPS(Random.Range(_MinDPSOfDOT, _MaxDPSOfDOT));
					characterHealth.AddDamageOverTime(dot);
				}
			}

			Vector3 graphicsHit = didHit ? hit.point : _ShootingOrigin.position + _ShootingOrigin.forward * RaycastLength;
			ShotGraphicsInfo shotGraphicsInfo = new()
			{
				Origin = _ShootingOrigin.position,
				DidHit = didHit,
				Hit = graphicsHit,
				HitNormal = hit.normal,
			};
		
			DoShotGraphicsClientRPC(shotGraphicsInfo);
		}

		[ClientRpc]
		private void DoShotGraphicsClientRPC(ShotGraphicsInfo shotGraphicsInfo)
		{
			GameObject graphicsObject = Instantiate(_ShotGraphicsPrefab);
			graphicsObject.GetComponent<ShotGraphics>().Initialize(shotGraphicsInfo);
		}
	}
}