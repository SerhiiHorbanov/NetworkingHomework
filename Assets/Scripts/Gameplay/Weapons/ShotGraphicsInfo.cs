using Unity.Netcode;
using UnityEngine;

namespace Gameplay.Weapons
{
	struct ShotGraphicsInfo : INetworkSerializeByMemcpy
	{
		public Vector3 Origin;
	
		public bool DidHit;
		public Vector3 Hit;
		public Vector3 HitNormal;
	}
}
