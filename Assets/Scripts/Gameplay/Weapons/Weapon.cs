using Gameplay.PlayerCharacter;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Weapons
{
	public class Weapon : NetworkBehaviour
	{
		[SerializeField] private Transform _ShootingOrigin;
		[SerializeField] private float _Damage;
		[SerializeField] private GameObject _ShotGraphicsPrefab;
	
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
				
				if (hit.collider.TryGetComponent(out CharacterHealth characterHealth))
				{
					characterHealth.DamageWithSync(_Damage);
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