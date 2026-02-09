using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Gameplay.PlayerCharacter
{
	public class CharacterHealth : NetworkBehaviour
	{
		[SerializeField] public float _MaxHp = 100;
		public float Hp { get; private set; }

		private readonly List<DamageOverTime> _dots = new(); 
		
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

		private void Update()
		{
			ApplyDOTs();
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
	
		public void AddDamageOverTime(DamageOverTime dot)
		{
			_dots.Add(dot);
			SyncDOTs();
		}
		
		private void ApplyDOTs()
		{
			if (_dots.Count == 0)
				return;
			
			float timePassed = Time.deltaTime;
			float accumulatedDamage = 0;
			
			for (int i = 0; i < _dots.Count; i++)
			{
				DamageOverTime dot = _dots[i];
				if (timePassed > dot._TimeLeft)
				{
					accumulatedDamage += dot._DPS * dot._TimeLeft;
					_dots.RemoveAt(i);
					i--;
					continue;
				}
				
				_dots[i] = dot.WithTimeLeft(dot._TimeLeft - timePassed);
				
				accumulatedDamage += dot._DPS * timePassed;
			}
			
			DamageWithoutSync(accumulatedDamage);
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
		
		private void SyncDOTs()
			=> SyncDOTsClientRPC(_dots.ToArray());
		[ClientRpc]
		private void SyncDOTsClientRPC(DamageOverTime[] dots)
		{
			_dots.Clear();
			_dots.AddRange(dots);
		}
	}
}