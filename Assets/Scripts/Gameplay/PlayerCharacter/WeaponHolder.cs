using Gameplay.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.PlayerCharacter
{
	public class WeaponHolder : NetworkBehaviour
	{
		[SerializeField] private Weapon _HoldingWeapon;

		public void Attack()
		{
			_HoldingWeapon.Shoot();
		}
	}
}
