using System;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.PlayerCharacter
{
	[Serializable]
	public struct DamageOverTime : INetworkSerializeByMemcpy
	{
		public float _DPS;
		public float _TimeLeft;

		public DamageOverTime WithTimeLeft(float value)
			=> new() {_DPS = _DPS, _TimeLeft = value};
		
		public DamageOverTime WithDPS(float value)
			=> new() {_DPS = value, _TimeLeft = _TimeLeft};
	}
}
