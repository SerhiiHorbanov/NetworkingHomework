using Unity.Netcode;
using UnityEngine;

namespace Gameplay.PlayerCharacter
{
	public class CharacterHealth : NetworkBehaviour
	{
		[SerializeField] public float _MaxHp = 100;
		public float Hp { get; private set; }
	
		private ValueChangedDelegate _valueChanged;
		public event ValueChangedDelegate OnValueChanged
		{
			add => _valueChanged += value;
			remove => _valueChanged -= value;
		}
	
		public delegate void ValueChangedDelegate(float oldHp, float newHp);

		private void Awake()
		{
			Hp = _MaxHp;
		}

		public void DamageWithoutSync(float damage)
		{
			float prevHp = Hp;
			Hp -= damage;

			if (prevHp == Hp)
				return;
		
			_valueChanged?.Invoke(prevHp, Hp);

			if (!IsServer)
				return;
		
			if (Hp <= 0)
			{
				Destroy(gameObject);
			}
		}

		public void DamageWithSync(float damage)
		{
			DamageWithoutSync(damage);
		
			if (IsServer)
				SyncHealth();
		}
	
		private void SyncHealth()
			=> SyncHealthClientRPC(Hp);
	
		[ClientRpc]
		public void SyncHealthClientRPC(float hp)
		{
			float prevHp = Hp;
			Hp = hp;

			if (prevHp == Hp)
				return;
		
			_valueChanged?.Invoke(prevHp, Hp);
		}
	}
}